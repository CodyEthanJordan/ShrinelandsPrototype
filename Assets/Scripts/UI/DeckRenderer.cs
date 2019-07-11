using ShrinelandsTactics.BasicStructures;
using System;
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

        private void Start()
        {
            Deck deck = new Deck();
            deck.AddCards(new Card("Hit", Card.CardType.Hit), 3);
            deck.AddCards(new Card("Dodge", Card.CardType.Miss), 1);
            deck.AddCards(new Card("Glancing Blow", Card.CardType.Armor), 1);
            deck.AddCards(new Card("Defense", Card.CardType.Miss), 2);

            RenderDeck(deck);
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

        public void DrawCard(string name)
        {

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
