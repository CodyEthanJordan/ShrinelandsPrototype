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
    public class CardRenderer : MonoBehaviour
    {
        public Image Banner;
        public Image Icon;
        public Text NameText;
        public Text DescriptionText;

        public Sprite CardFront;
        public Sprite CardBack;
        public Sprite[] Sprites;

        private Image image;

        private bool faceUp = true;

        private void Start()
        {
            image = GetComponent<Image>();
            FlipOver();
        }

        public void RenderCard(Card card)
        {
            NameText.text = card.Name;
            DescriptionText.text = card.TypeOfCard.ToString();

            Color c = TypeToColor(card.TypeOfCard);
            DescriptionText.color = c;
            Banner.color = c;

            var sprite = Sprites.FirstOrDefault(s => s.name == card.Name);
            if(sprite != null)
            {
                Icon.sprite = sprite;
            }

        }

        public void FlipOver()
        {
            faceUp = !faceUp;
            NameText.enabled = faceUp;
            DescriptionText.enabled = faceUp;
            Banner.enabled = faceUp;
            Icon.enabled = faceUp;
            if(faceUp)
            {
                image.sprite = CardFront;
            }
            else
            {
                image.sprite = CardBack;
            }

        }

        public static Color TypeToColor(Card.CardType t)
        {
            switch (t)
            {
                case Card.CardType.Hit:
                    return Color.red;
                case Card.CardType.Miss:
                    return Color.blue;
                case Card.CardType.Block:
                    return Color.blue;
                case Card.CardType.Armor:
                    return Color.green;
                default:
                    break;
            }
            Debug.LogError("Missing color data for " + t);
            return Color.white;
        }
    }
}
