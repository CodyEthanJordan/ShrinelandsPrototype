using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ShrinelandsTactics.World;

namespace Assets.Scripts.UI
{
    public class CharacterPopupUI : MonoBehaviour
    {
        public Text NameText;
        public Text VitText;
        public Text MoveText;
        public Text StaText;
        public Text ManaText;
        public GameObject TraitsPane;
        public GameObject AbilityPane;

        public GameObject TextPrefab;

        private void Start()
        {
            Clear();
        }

        public void ShowCharacter(Character guy)
        {
            Clear(); //TODO: show side
            NameText.text = guy.Name;
            VitText.text = "Vit:" + guy.Vitality;
            MoveText.text = "Move:" + guy.Move;
            StaText.text = "Sta:" + guy.Stamina;
            ManaText.text = "Mana:" + guy.Mana;
            foreach (var trait in guy.Traits)
            {
                var text = Instantiate(TextPrefab, TraitsPane.transform);
                text.GetComponent<Text>().text = trait;
            }
            foreach (var ability in guy.Actions)
            {
                var text = Instantiate(TextPrefab, AbilityPane.transform);
                text.GetComponent<Text>().text = ability.Name;
            }
        }

        public void Clear()
        {
            NameText.text = "";
            VitText.text = "";
            MoveText.text = "";
            StaText.text = "";
            ManaText.text = "";
            foreach (Transform child in TraitsPane.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in AbilityPane.transform)
            {
                Destroy(child.gameObject);
            }
    }

    }
}
