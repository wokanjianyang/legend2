using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Atr_Spe : MonoBehaviour
    {
        [LabelText("Txt_Name")]
        public Text Txt_Name;

        [LabelText("Txt_Attr")]
        public Text Txt_Attr;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(int atrId, double atrVue, long requireLevel, long currentLevel)
        {
            string txt = " + " + StringHelper.FormatAttrValueText(atrId, atrVue);

            if (currentLevel >= requireLevel)
            {
                txt += "（已解锁）";
            }
            else
            {
                txt += "（" + requireLevel + "级解锁）";
            }

            this.Txt_Name.text = StringHelper.FormatAttrValueName(atrId);
            this.Txt_Attr.text = txt;
        }
    }
}
