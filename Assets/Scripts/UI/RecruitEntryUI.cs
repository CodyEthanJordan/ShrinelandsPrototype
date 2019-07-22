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

        public void Show(Character guy, int cost)
        {
            this.GuyRepresented = guy;
            this.Cost = cost;

        }

    }
}
