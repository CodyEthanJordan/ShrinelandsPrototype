using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ShrinelandsTactics;
using ShrinelandsTactics.World;

namespace Assets.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        public Dictionary<string, Texture2D> Texture;

        public Character CharacterRepresented;

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();    
        }

        public void RepresentCharacter(Character c)
        {
            this.CharacterRepresented = c;

        }
    }
}
