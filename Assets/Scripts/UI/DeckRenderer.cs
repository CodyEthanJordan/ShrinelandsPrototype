using ShrinelandsTactics.BasicStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DeckRenderer : MonoBehaviour
    {
        public GameObject CardPrefab;
        public int ShuffledOverlap;
        public float ShuffleSpeed;
        public float SuspenseTime;
        public float WaitTime;

        private HorizontalLayoutGroup hlg;

        private void Awake()
        {
            hlg = GetComponent<HorizontalLayoutGroup>();
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void RenderDeck(Deck deck)
        {
            ClearDeck();
            foreach (var card in deck.Cards)
            {
                var go = Instantiate(CardPrefab, this.transform);
                var cr = go.GetComponent<CardRenderer>();
                cr.RenderCard(card);
            }
        }

        public IEnumerator DrawCard(string name)
        {
            yield return new WaitForSeconds(WaitTime);
            Debug.Log("Drawing card " + name);
            yield return Shuffle();

            var cr = GetComponentsInChildren<CardRenderer>().FirstOrDefault(c => c.NameText.text == name);
            if(cr == null)
            {
                Debug.LogError("Can't draw card " + name);
                throw new ArgumentException("Can't draw " + name);
            }

            yield return new WaitForSeconds(SuspenseTime);
            cr.transform.SetAsLastSibling();
            cr.FlipOver();
            yield return new WaitForSeconds(WaitTime);
            Destroy(this.gameObject);
        }

        public IEnumerator Shuffle()
        {
            foreach (var cr in GetComponentsInChildren<CardRenderer>())
            {
                cr.FlipOver();
            }

            while(hlg.spacing > ShuffledOverlap)
            {
                hlg.spacing -= 30;
                yield return new WaitForSeconds(ShuffleSpeed);
            }
        }

        public void ClearDeck()
        {
            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
