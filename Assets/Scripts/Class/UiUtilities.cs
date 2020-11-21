using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RC2Skills
{
	public class UiUtilities : MonoBehaviour
	{
		public Text text;
		public Slider slider;
		public GameObject rightPanel;
		public GameObject leftPanel;
		public GameObject minimap;
        public Toggle minTog;

        private void Start()
        {
			DisplaySliderValue(slider.value);

		}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rightPanel.activeSelf)
                {
                    rightPanel.SetActive(false);
                    leftPanel.SetActive(false);
                   // minimap.SetActive(false);
                    
                }
                else 
                    rightPanel.SetActive(true);

            }

            


        }
        public void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}

		public void OpenClose (GameObject obj)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
			else
				obj.SetActive(true);
		}

		public void DisplaySliderValue(float value)
        {
			text.text = value.ToString();
        }
	}

}

