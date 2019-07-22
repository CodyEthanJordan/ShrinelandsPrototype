using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScreenManagers
{
    public class ChapterhouseManager : MonoBehaviour
    {
        public GameObject RecruitList;

        public GameObject RecruitEntryPrefab;

        private void Start()
        {
            ClearRecruits();
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
