using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ShrinelandsTactics.World;

namespace Assets.Scripts.UI
{
    public class PartyMemberUI : MonoBehaviour
    {
        public Image Portrait;
        public Text NameText;
        public GameObject GearList;
        public GameObject ItemSlotPrefab;

        public Character Guy;

        private void Start()
        {
            
        }

        public void Show(Character guy)
        {
            this.Guy = guy;
            int itemSlots = 4;
            ClearGearSlots();

            for (int i = 0; i < itemSlots; i++)
            {
                var go = Instantiate(ItemSlotPrefab, GearList.transform);
                var itemSlot = go.GetComponent<ItemSlot>();
            }

        }

        private void ClearGearSlots()
        {
            foreach (Transform child in GearList.transform)
            {
                Destroy(child);
            }
        }


    }
}
