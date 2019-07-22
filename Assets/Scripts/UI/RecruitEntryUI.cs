using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ShrinelandsTactics.World;
using Assets.Scripts.ScriptableObjects;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class RecruitEntryUI : MonoBehaviour
    {
        public Text ClassText;
        public Text CostText;
        public Image Portrait;
        public SpriteHolder Sprites;

        public int Cost;
        public Character GuyRepresented;

        private Image bg;

        private void Awake()
        {
            bg = GetComponent<Image>();
        }

        public void Highlight()
        {
            bg.color = new Color(0, 30, 30);
        }

        public void Deselect()
        {
            bg.color = new Color(0, 0, 0, 0);
        }

        public void Show(Character guy, int cost)
        {
            this.GuyRepresented = guy;
            this.Cost = cost;

            ClassText.text = guy.Class;
            CostText.text = cost.ToString();

            Portrait.sprite = Sprites.GetSpriteByName(guy.Class);
        }

    }
}
