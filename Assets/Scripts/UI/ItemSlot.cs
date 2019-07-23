using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ShrinelandsTactics.World;
using Assets.Scripts.ScreenManagers;

namespace Assets.Scripts.UI
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        public Image Icon;
        private ChapterhouseManager cm;

        public Item ItemHeld = null;

        private void Start()
        {
            cm = GameObject.Find("ChapterManager").GetComponent<ChapterhouseManager>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var go = eventData.pointerDrag;
            var item = go.GetComponent<DraggableItem>();
            if(item == null)
            {
                Debug.LogError("That is not an item");
                return; //not an item
            }

            if(item.validDrag == false)
            {
                //can't purchase
                return;
            }

            if(ItemHeld != null)
            {
                //already has something
                return;
            }

            Icon.sprite = item.Icon.sprite;
            Destroy(go);
            cm.Requisition -= item.Cost;
            this.ItemHeld = item.Item;

        }
    }
}
