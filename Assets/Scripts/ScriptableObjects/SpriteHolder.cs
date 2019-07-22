using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpriteHolder", order = 1)]
    public class SpriteHolder : ScriptableObject
    {
        public Sprite[] Sprites;

        public Sprite GetSpriteByName(string name)
        {
            var sprite = Sprites.FirstOrDefault(s => s.name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if(sprite == null)
            {
                Debug.LogError("No sprite for " + name);
                return Sprites[0];
            }

            return sprite;
        }
    }
}
