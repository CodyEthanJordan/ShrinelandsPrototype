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
    }
}
