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
        public GameObject ShuffleArea;
        public GameObject DrawArea;
        public GameObject CardPrefab;
        public int ShuffledOverlap;
        public float ShuffleSpeed;
        public float SuspenseTime;
        public float WaitTime;

        private GridLayoutGroup gl;

        private void Awake()
        {
            gl = ShuffleArea.GetComponent<GridLayoutGroup>();
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
                var go = Instantiate(CardPrefab, ShuffleArea.transform);
                var cr = go.GetComponent<CardRenderer>();
                cr.RenderCard(card);
            }
        }

        public IEnumerator DrawCard(List<string> names)
        {
            yield return new WaitForSeconds(WaitTime);
            yield return Shuffle();

            foreach (var name in names)
            {
                var cr = GetComponentsInChildren<CardRenderer>().FirstOrDefault(c => c.NameText.text == name);
                if (cr == null)
                {
                    Debug.LogError("Can't draw card " + name);
                    throw new ArgumentException("Can't draw " + name);
                }

                yield return new WaitForSeconds(SuspenseTime);
                cr.transform.SetParent(DrawArea.transform);
                yield return new WaitForSeconds(SuspenseTime/2);
                cr.FlipOver();
            }
            yield return new WaitForSeconds(WaitTime);
            Destroy(this.gameObject);
        }

        public IEnumerator Shuffle()
        {
            foreach (var cr in GetComponentsInChildren<CardRenderer>())
            {
                cr.FlipOver();
            }

            var spacing = gl.spacing;
            float t = 0;
            while(t <= 1)
            {
                var newSpacing = Vector2.Lerp(spacing, -gl.cellSize, t);
                t += 0.1f;

                gl.spacing = newSpacing;

                yield return new WaitForSeconds(ShuffleSpeed);
            }
            gl.spacing = -gl.cellSize;
        }

        public void ClearDeck()
        {
            foreach (Transform child in ShuffleArea.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
