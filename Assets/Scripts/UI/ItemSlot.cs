using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        public Image Icon;

        public void OnDrop(PointerEventData eventData)
        {
            var go = eventData.pointerDrag;
            var item = go.GetComponent<DraggableItem>();
            if(item == null)
            {
                Debug.LogError("That is not an item");
                return; //not an item
            }

            Icon.sprite = item.Icon.sprite;
            Destroy(go);

        }
    }
}
