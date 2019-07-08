using ShrinelandsTactics.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AbilityPanelUI : MonoBehaviour
    {
        public GameObject ButtonPrefab;

        private Character guy;
        private CombatManager cm;

        private void Awake()
        {
            ClearButtons();
        }

        public void AskToActivate(CombatManager cm, Character guy)
        {
            ClearButtons();
            this.guy = guy;
            this.cm = cm;
            var button = Instantiate(ButtonPrefab, this.transform);
            button.GetComponentInChildren<Text>().text = "Activate";
            button.GetComponent<Button>().onClick.AddListener(ActivateGuy);
        }

        public void ClearButtons()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        void ActivateGuy()
        {
            cm.Activate(guy);
        }
    }
}
