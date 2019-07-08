using ShrinelandsTactics.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class NameplateUI : MonoBehaviour
    {
        public Text NameText;
        public Text VitText;
        public Text MoveText;
        public Text StaText;
        public Text ManaText;

        private void Start()
        {
            StopShowing();
        }

        public void StopShowing()
        {
            NameText.text = "";
            VitText.text = "";
            MoveText.text = "";
            StaText.text = "";
            ManaText.text = "";
        }

        public void ShowCharacter(Character guy)
        {
            NameText.text = guy.Name;
            VitText.text = "Vit:" + guy.Vitality;
            MoveText.text = "Move:" + guy.Move;
            StaText.text = "Sta:" + guy.Stamina;
            ManaText.text = "Mana:" + guy.Mana;
        }
    }
}
