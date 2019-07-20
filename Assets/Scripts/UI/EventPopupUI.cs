using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI
{
    public class EventPopupUI : MonoBehaviour
    {
        public float OpenSpeed;
        public float MaxWidth;

        private RectTransform rt;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Open());
            }
        }

        public IEnumerator Open()
        {
            while(rt.sizeDelta.x < MaxWidth)
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x + Time.deltaTime * OpenSpeed, rt.sizeDelta.y);
                yield return new WaitForEndOfFrame();
            }

            float alpha = 0;
            while(alpha <= 1)
            {
                alpha += Time.deltaTime * OpenSpeed / 100;
                foreach (var text in GetComponentsInChildren<Text>())
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                }
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
