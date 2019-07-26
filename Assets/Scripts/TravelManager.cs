using Assets.Scripts.UI;
using ShrinelandsTactics;
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

        public TravelMaster TM;
        public Encounter CurrentEncounter;

        public float TimeBetweenEvents;

        private float nextEventDraw = 1;
        private float nighttime = float.PositiveInfinity;

        private float previousSpeed;

        private void Start()
        {
            TM = new TravelMaster();

            //need an event pipeline
            TM.OnEncounterOutcome += EncounterResolved;
        }

        private void EncounterResolved(object sender, string e)
        {
            EventPopup.ShowOutcome(e);
        }

        private void Update()
        {
            float x = Time.time;
            if(Time.time >= nextEventDraw)
            {
                nextEventDraw = float.PositiveInfinity;
                CurrentEncounter = TM.DrawEncounter();
                StartCoroutine(ShowEncounter(CurrentEncounter));
            }
            else if(Time.time >= nighttime)
            {

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

        internal void CarryOn()
        {
            EventPopup.Close();
            StartWalking();
            nextEventDraw = Time.time + TimeBetweenEvents;
        }

        public void ChooseOption(int i)
        {
            TM.ChooseOption(CurrentEncounter, i);
        }

        private void StopWalking()
        {
            previousSpeed = Background.Speed;
            Background.Speed = 0;
            foreach (var go in PartyMembers)
            {
                go.GetComponent<Animator>().SetTrigger("Stop");
            }
        }

        private void StartWalking()
        {
            Background.Speed = previousSpeed;
            foreach (var go in PartyMembers)
            {
                go.GetComponent<Animator>().SetTrigger("Walk");
            }
        }
    }
}
