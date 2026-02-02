using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HP_Progress : MonoBehaviour
    {
        public Image img_Progress;

        [LabelText("进度文本")]
        public Text tmp_Progress;

        public void SetProgress(double current, double total)
        {
            if (total <= 0)
            {
                total = 1;
            }

            double value = current / total;
            if (value > 1)
            {
                value = 1f;
            }
            this.img_Progress.fillAmount = (float)value;
            this.tmp_Progress.text = string.Format("{0}/{1}", StringHelper.FormatNumber(current), StringHelper.FormatNumber(total));

        }
        public void HideTitle()
        {
            this.tmp_Progress.gameObject.SetActive(false);
        }
        public void ShowTitle()
        {
            this.tmp_Progress.gameObject.SetActive(true);
        }
    }
}