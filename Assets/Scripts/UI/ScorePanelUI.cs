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
    public class ScorePanelUI : MonoBehaviour
    {
        public GameObject TextPrefab;
        private void Start()
        {
            Clear();
        }

        public void ShowScore(List<Side> sides)
        {
            Clear();
            foreach (var side in sides)
            {
                var go = Instantiate(TextPrefab, this.transform);
                var t = go.GetComponent<Text>();
                t.text = side.Name + " : " + side.Score;
            }
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
