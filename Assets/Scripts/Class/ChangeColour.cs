using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


namespace RC2Skills
{
    public class ChangeColour : MonoBehaviour
    {
        Color colour;
        
        public GameObject temp;
        public GameObject obj;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ApplyColour(GameObject obj)
        {
            List<Material> mats = new List<Material>();

            var num = obj.transform.childCount;
            
            for (int i = 0; i < num; i++)
            {
                mats.AddRange(obj.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().materials);                
            }
            mats.AddRange(obj.GetComponent<MeshRenderer>().materials);

            for (int i = 0; i < mats.Count; i++)
            {
                mats[i].color = colour;
            }

        }

        public void SetColour(string color)
        {
            ColorUtility.TryParseHtmlString(color, out colour);

            ApplyColour(temp);
               
        }
    }
}

