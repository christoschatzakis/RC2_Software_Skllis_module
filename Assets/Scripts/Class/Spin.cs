using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RC2Skills
{   
  
    public class Spin : MonoBehaviour
    {
        public bool spin=true;
        float speed;
        public Slider slider;
        private void Start()
        {
            speed = slider.value;
        }
        // Update is called once per frame
        void Update()
        {
            if(spin)
            SpinObject(gameObject, speed);
        }

        public void SpinObject(GameObject obj, float speed)
        {
            Vector3 point = obj.transform.position;
            obj.transform.RotateAround(point, Vector3.up, speed * Time.deltaTime);
        }

        public void SliderSpeed(float value)
        {
            speed = value;
        }
        public void ToggleSpin(bool value)
        {
            spin = value;
        }
    }
}

