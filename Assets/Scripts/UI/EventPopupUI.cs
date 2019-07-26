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
        public Text Title;
        public Text Body;
        public Transform OptionsPanel;
        public GameObject OptionPrefab;

        public float OpenSpeed;
        public float MaxWidth;

        private RectTransform rt;
        private TravelManager tm;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }

            tm = GameObject.Find("TravelManager").GetComponent<TravelManager>(); //TODO: is this evil?
        }

        private void Update()
        {
        }

        public IEnumerator ShowOptions(IEnumerable<string> options)
        {
            foreach (Transform child in OptionsPanel)
            {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (var option in options)
            {
                var go = Instantiate(OptionPrefab, OptionsPanel);
                var text = go.GetComponentInChildren<Text>();
                text.text = option;
                var button = go.GetComponent<Button>();
                button.onClick.AddListener(() => ChooseOption(i));
                i++; //TODO: does this fail for weird reasons? can never remember
            }
            yield return new WaitForEndOfFrame();
        }

        private void ChooseOption(int i)
        {
            tm.ChooseOption(i);
        }

        public IEnumerator Open(string title, string body)
        {
            this.Title.text = title;
            this.Body.text = body;

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
