using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.ScreenManagers;
using ShrinelandsTactics.World;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.UI
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Text Name;
        public Text CostText;

        public SpriteHolder Sprites;

        public Item Item;
        public int Cost;

        private ChapterhouseManager cm;
        private Vector3 startingPos;
        public bool validDrag;


        private void Start()
        {
            cm = GameObject.Find("ChapterManager").GetComponent<ChapterhouseManager>();
        }

        public void ShowItem(Item item, int cost)
        {
            this.Item = item;
            this.Cost = cost;

            this.Name.text = item.Name;
            this.CostText.text = cost.ToString();
            this.Icon.sprite = Sprites.GetSpriteByName(item.Name);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(cm.Requisition < Cost)
            {
                validDrag = false;
                return;
            }
            else
            {
                validDrag = true;
            }
            startingPos = transform.position;
            GetComponent<Image>().raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(validDrag)
            {
                transform.position = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<Image>().raycastTarget = true;
            transform.position = startingPos;
        }
    }
}
