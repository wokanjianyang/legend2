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

        }

        public void SetPercent(float percent)
        {
            Debug.Log("hp percent:" + percent);

            image.fillAmount = percent;
        }
    }
}
