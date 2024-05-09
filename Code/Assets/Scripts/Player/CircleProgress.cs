using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CircleProgress : MonoBehaviour
    {
        public Image image;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("CircleProgress Start");

            this.SetPercent(0.5f);
        }



        public void SetPercent(float percent)
        {
            image.fillAmount = percent;
        }
    }
}
