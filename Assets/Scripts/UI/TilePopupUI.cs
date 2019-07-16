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
    public class TilePopupUI : MonoBehaviour
    {
        public Text TileName;
        public Text Description;

        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
            Clear();
        }

        public void Clear()
        {
            TileName.text = "";
            Description.text = "";
            image.enabled = false;
        }

        public void Show(Tile tile)
        {
            image.enabled = true;
            TileName.text = tile.Name;
            Description.text = tile.Description;
        }


    }
}
