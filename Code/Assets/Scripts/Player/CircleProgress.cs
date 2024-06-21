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

        int ts = 0;
        int p = 0;

        void Update()
        {
            ts++;

            //int tp = ts / 50;
            //if (tp != p)
            //{
            //    p = tp;

            //    Debug.Log("p:" + p);

            //    float pc = (float)((100 - p) / 100.0);

            //    this.SetPercent(pc);
            //}

        }

        public void SetPercent(float percent)
        {
            Debug.Log("hp percent:" + percent);

            image.fillAmount = percent;

            Debug.Log("hp percent result:" + image.fillAmount);
        }
    }
}
