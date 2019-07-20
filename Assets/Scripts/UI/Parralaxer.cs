using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class Parralaxer : MonoBehaviour
    {
        public float Speed;
        public float LayerSpeedRatio;

        private int startingChildren;
        // Use this for initialization
        void Start()
        {
            int childCount = this.transform.childCount;
            startingChildren = childCount;
            List<GameObject> newObjects = new List<GameObject>();
            for (int i = 0; i < childCount; i++)
            {
                var copy = Instantiate(this.transform.GetChild(i).gameObject, this.transform);
                copy.transform.position = new Vector3(29, 0, 0);
                newObjects.Add(copy);
            }

            //for (int i = 0; i < childCount; i++)
            //{
            //    var copy = newObjects[i];
            //    copy.transform.SetParent(this.transform);
            //    int newIndex = 2 * (i - 1) + 1;
            //    copy.transform.SetSiblingIndex(newIndex);
            //    var current = this.transform.GetChild(newIndex - 1).gameObject;
            //    copy.transform.position = current.transform.position + new Vector3(29, 0, 0);
            //}
        }

        // Update is called once per frame
        void Update()
        {

            float currentSpeed = Speed;
            for (int i = 0; i < startingChildren; i++)
            {
                var piece1 = transform.GetChild(i);
                piece1.Translate(currentSpeed * Time.deltaTime, 0, 0);
                var piece2 = transform.GetChild(i + startingChildren);
                piece2.Translate(currentSpeed * Time.deltaTime, 0, 0);

                if (piece1.position.x <= -29)
                {
                    piece1.position = new Vector3(29 + piece2.position.x, 0, 0);
                }
                if (piece2.position.x <= -29)
                {
                    piece2.position = new Vector3(29 + piece1.position.x, 0, 0);
                }

                currentSpeed *= LayerSpeedRatio;
            }


        }
    }
}
