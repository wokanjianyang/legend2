using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Progress_Hp : MonoBehaviour
    {
        public Image img_Progress;

        public Text tmp_Progress;

        public int Order => (int)ComponentOrder.Progress;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetProgress(float progress)
        {
            this.img_Progress.fillAmount = progress;
        }

    }
}