using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CircleProgress : MonoBehaviour
    {
        public Material material;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("CircleProgress Start");

            this.SetPercent(0.5f);
        }



        public void SetPercent(float percent)
        {
            Debug.Log("Set Begin:" + material.GetFloat("_Fill"));

            material.SetFloat("_Fill", percent);

            Debug.Log("Set Over:" + material.GetFloat("_Fill"));
        }
    }
}
