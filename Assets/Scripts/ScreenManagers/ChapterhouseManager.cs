using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ShrinelandsTactics;
using ShrinelandsTactics.World;
using Assets.Scripts.UI;

namespace Assets.Scripts.ScreenManagers
{
    public class ChapterhouseManager : MonoBehaviour
    {
        public GameObject RecruitList;
        public GameObject RecruitEntryPrefab;
        public GameObject ItemList;
        public GameObject ItemPrefab;
        public GameObject PartyList;
        public GameObject PartyMemberPrefab;

        private RecruitEntryUI SelectedRecruit = null;

        public int Manpower;
        public int Requisition;

        private void Start()
        {
            ClearRecruits();
            ClearItems();
            ClearParty();

            SetupDebugData();

        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            var hits = Physics.RaycastAll(mousePos, Vector2.zero);

            if (hits != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hits.Any(h => h.collider.tag == "RecruitEntry"))
                    {
                        var hit = hits.First(h => h.collider.tag == "RecruitEntry");
                        var recruit = hit.collider.gameObject.GetComponent<RecruitEntryUI>();
                        recruit.Highlight();

                        if (SelectedRecruit != null)
                        {
                            SelectedRecruit.Deselect();
                        }

                        SelectedRecruit = recruit;
                    }
                }
            }
        }

        private void SetupDebugData()
        {
            var c = DebugData.GetDebugCharacter();
            var guys = new List<Character>() { c };
            var cost = new List<int>() { 3 };

            ShowRecruits(guys, cost);

            Manpower = 20;
            Requisition = 20;

            List<Item> items = new List<Item>();
            List<int> itemCosts = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                items.Add(DebugData.GetProvisions());
                itemCosts.Add(1);
            }
            items.Add(new Item("Longsword"));
            itemCosts.Add(4);
            ShowItems(items, itemCosts);
        }

        private void ClearParty()
        {
            foreach (Transform child in PartyList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void Recruit()
        {

        }

        public void ShowItems(List<Item> items, List<int> cost)
        {
            ClearItems();
            for (int i = 0; i < items.Count; i++)
            {
                var go = Instantiate(ItemPrefab, ItemList.transform);
                var item = go.GetComponent<DraggableItem>();
                item.ShowItem(items[i], cost[i]);
            }
        }

        private void ClearItems()
        {
            foreach (Transform child in ItemList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void ShowRecruits(List<Character> guys, List<int> cost)
        {
            ClearRecruits();

            for (int i = 0; i < guys.Count; i++)
            {
                var go = Instantiate(RecruitEntryPrefab, RecruitList.transform);
                var r = go.GetComponent<RecruitEntryUI>();
                r.Show(guys[i], cost[i]);
            }
        }

        private void ClearRecruits()
        {
            foreach (Transform child in RecruitList.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
