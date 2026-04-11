using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Attr : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Value;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(int attrId, double attrValue)
        {
            string name = StringHelper.FormatAttrValueName(attrId);
            if (name.Length == 2)
            {
                name = name.Insert(1, "    ");
            }
            else if (name.Length == 3)
            {
                name = name.Insert(1, " ").Insert(3, " ");
            }

            Txt_Name.text = name + "£º";
            Txt_Value.text = StringHelper.FormatAttrValueText(attrId, (long)attrValue);
        }
    }
}
