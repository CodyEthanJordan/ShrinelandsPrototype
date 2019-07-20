using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TravelTextScroller : MonoBehaviour
    {
        public float Speed;

        private void Update()
        {
            this.transform.Translate(Speed * Time.deltaTime, 0, 0);
        }
    }
}
