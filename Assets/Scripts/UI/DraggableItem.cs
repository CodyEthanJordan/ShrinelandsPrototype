using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.ScreenManagers;

namespace Assets.Scripts.UI
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Text Name;
        public Text CostText;

        public string Item;
        public int Cost;

        private ChapterhouseManager cm;
        private Vector3 startingPos;
        private bool validDrag;



        private void Start()
        {
            cm = GameObject.Find("ChapterManager").GetComponent<ChapterhouseManager>();
        }

        public void ShowItem(string item, int cost)
        {
            this.Item = item;
            this.Cost = cost;

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startingPos = transform.position;
            GetComponent<Image>().raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<Image>().raycastTarget = true;
            transform.position = startingPos;
        }
    }
}
