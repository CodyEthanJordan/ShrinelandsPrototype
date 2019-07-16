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
        public Sprite[] CharacterSprites;
   
        public Character CharacterRepresented;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();    
        }

        public void RepresentCharacter(Character c)
        {
            this.CharacterRepresented = c;
            var sprite = CharacterSprites.FirstOrDefault(cs => cs.name.Equals(c.Class, StringComparison.OrdinalIgnoreCase));
            if(sprite != null)
            {
                sr.sprite = sprite;
                return;
            }
            Debug.LogError("No sprite for " + c.Class);
            sr.sprite = CharacterSprites[0];
        }

        internal void UpdatePosition(CombatManager cm)
        {
            var pos = new Vector3(CharacterRepresented.Pos.x, cm.DM.map.Height - CharacterRepresented.Pos.y + 1, 0);
            this.transform.position = pos;
        }
    }
}
