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

        private HorizontalLayoutGroup hlg;

        private void Start()
        {
            hlg = GetComponent<HorizontalLayoutGroup>();

            Deck deck = new Deck();
            deck.AddCards(new Card("Hit", Card.CardType.Hit), 3);
            deck.AddCards(new Card("Dodge", Card.CardType.Miss), 1);
            deck.AddCards(new Card("Glancing Blow", Card.CardType.Armor), 1);
            deck.AddCards(new Card("Defense", Card.CardType.Miss), 2);

            RenderDeck(deck);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DrawCard("Hit"));
            }
        }

        public void RenderDeck(Deck deck)
        {
            ClearDeck();
            foreach (var card in deck.Cards)
            {
                var go = Instantiate(CardPrefab, this.transform);
                var cr = go.GetComponent<CardRenderer>();
                cr.RenderCard(card);
                Debug.Log(card);
            }
        }

        public IEnumerator DrawCard(string name)
        {
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
