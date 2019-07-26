using Assets.Scripts.UI;
using ShrinelandsTactics.World;
using ShrinelandsTactics.World.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TravelManager : MonoBehaviour
    {
        public EventPopupUI EventPopup;
        public Parralaxer Background;
        public GameObject[] PartyMembers;


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                //trigger event
                var encounter = DebugData.GetMistWolfEncounter();
                StartCoroutine(ShowEncounter(encounter));
            }
        }

        private IEnumerator ShowEncounter(Encounter encounter)
        {
            //halt walking animation
            if (encounter.StopTravel)
            {
                StopWalking();
            }

            EventPopup.enabled = true;
            yield return EventPopup.Open(encounter.Title, encounter.Prompt);
            yield return EventPopup.ShowOptions(encounter.Options.Select(o => o["Prompt"] as string));

        }

        public void ChooseOption(int i)
        {

        }

        private void StopWalking()
        {
            Background.Speed = 0;
            foreach (var go in PartyMembers)
            {
                go.GetComponent<Animator>().SetTrigger("Stop");
            }
        }
    }
}
