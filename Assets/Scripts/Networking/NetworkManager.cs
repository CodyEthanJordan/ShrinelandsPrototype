using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lidgren;
using UnityEngine.UI;
using Lidgren.Network;
using System.IO;
using ShrinelandsTactics;
using Newtonsoft.Json;
using ShrinelandsTactics.BasicStructures;

namespace Assets.Scripts.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public NetClient Client;
        public bool Connected = false;
        private DungeonMaster DM;
        public readonly int Port = 6356;

        public InputField IPInput;

        private bool SetupCombatManager = false;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void JoinGame()
        {
            JoinGame(IPInput.text);
        }

        public void JoinGame(string IP)
        {
            var config = new NetPeerConfiguration("Shrinelands");
            Client = new NetClient(config);
            Client.Start();
            Client.Connect(host: IP, port: Port);

            Debug.Log("Requesting encounter from server");

            var r = NetSendResult.FailedNotConnected;
            while (r == NetSendResult.FailedNotConnected)
            {
                var message = Client.CreateMessage();
                message.Write("Send DM");
                r = Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
            }
            Connected = true;
        }

        private void Update()
        {
            if (!Connected)
            {
                return;
            }

            if(!SetupCombatManager)
            {
                var cm = GameObject.Find("CombatManager");
                if(cm != null)
                {
                    cm.GetComponent<CombatManager>().Setup(this, DM);
                    SetupCombatManager = true;
                }
            }

            //listen for messages
            NetIncomingMessage message;
            while ((message = Client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ProcessMessage(message.ReadString());
                        break;

                    default:
                        break;
                }
            }
        }

        public void SendAction(Outcome outcome)
        {
            var json = JsonConvert.SerializeObject(outcome);
            var message = Client.CreateMessage("Take action\n" + json);
            Client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
        }

        private void ProcessMessage(string message)
        {
            StringReader sr = new StringReader(message);
            var toDo = sr.ReadLine();
            switch (toDo)
            {
                case "DM":
                    DM = JsonConvert.DeserializeObject<DungeonMaster>(sr.ReadToEnd());
                    Debug.Log("Loaded level from server");
                    LoadCombatScene();
                    break;
                case "Take action":
                    var outcome = JsonConvert.DeserializeObject<Outcome>(sr.ReadToEnd());
                    DM.ApplyOutcome(outcome);
                    break;
                default:
                    Debug.LogError("Unknown action: " + toDo);
                    break;
            }

        }

        private void LoadCombatScene()
        {
            SceneManager.LoadScene("CombatScene");
        }
    }
}
