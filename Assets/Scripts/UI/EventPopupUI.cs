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
        public Text Outcome;
        public Button CarryOnButton;
        public Transform OptionsPanel;
        public GameObject OptionPrefab;

        public float OpenSpeed;
        public float MaxWidth;

        private RectTransform rt;
        private TravelManager tm;

        private void Start()
        {
            rt = GetComponent<RectTransform>();

            Close();

            tm = GameObject.Find("TravelManager").GetComponent<TravelManager>(); //TODO: is this evil?
        }

        private void Update()
        {
        }

        private void ClearOptions()
        {
            foreach (Transform child in OptionsPanel)
            {
                Destroy(child.gameObject);
            }
        }

        public void CarryOn()
        {
            tm.CarryOn();
        }

        public IEnumerator ShowOptions(IEnumerable<string> options)
        {
            ClearOptions();
            Outcome.enabled = false;
            CarryOnButton.gameObject.SetActive(false);

            int i = 0;
            foreach (var option in options)
            {
                var go = Instantiate(OptionPrefab, OptionsPanel);
                var text = go.GetComponentInChildren<Text>();
                text.text = option;
                var button = go.GetComponent<Button>();
                int o = i;
                button.onClick.AddListener(() => ChooseOption(o));
                i++;
            }
            yield return new WaitForEndOfFrame();
        }

        internal void Close()
        {
            CarryOnButton.gameObject.SetActive(false);
            foreach (var text in GetComponentsInChildren<Text>())
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            }

            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);
            ClearOptions();
            Outcome.enabled = false;

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

        public void ShowOutcome(string outcome)
        {
            ClearOptions();
            Outcome.enabled = true;
            CarryOnButton.gameObject.SetActive(true);
            this.Outcome.text = outcome;
        }

    }
}
