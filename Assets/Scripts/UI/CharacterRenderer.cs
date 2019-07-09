using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ShrinelandsTactics;
using ShrinelandsTactics.World;

namespace Assets.Scripts.UI
{
    public class CharacterRenderer : MonoBehaviour
    {
        public Sprite knightSprite;
        public Sprite slimeSprite;
        public Sprite slimelordSprite;

        public Character CharacterRepresented;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();    
        }

        public void RepresentCharacter(Character c)
        {
            this.CharacterRepresented = c;
            switch(c.Class)
            {
                case "Knight":
                    sr.sprite = knightSprite;
                    break;
                case "Slime":
                    sr.sprite = slimeSprite;
                    break;
                case "Slimelord":
                    sr.sprite = slimelordSprite;
                    break;

                default:
                    throw new NotImplementedException();
            }

        }

        internal void UpdatePosition(CombatManager cm)
        {
            var pos = new Vector3(CharacterRepresented.Pos.x, cm.DM.map.Height - CharacterRepresented.Pos.y + 1, 0);
            this.transform.position = pos;
        }
    }
}
