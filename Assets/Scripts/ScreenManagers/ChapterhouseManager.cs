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

        private RecruitEntryUI SelectedRecruit = null;

        public int Manpower;
        public int Requisition;

        private void Start()
        {
            ClearRecruits();

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
