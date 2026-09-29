using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Legend_Part : MonoBehaviour
    {
        public Image Logo;
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

        public void SetContent(int part, string name, double attrValue)
        {
            Txt_Name.text = name;
            this.Logo.sprite = PrefabHelper.Instance().GetItemLogo("Bag/Equip/Box_Equip_4_" + part);

            if (attrValue > 0)
            {
                Txt_Value.text = "×ÊÖÊ" + attrValue;
            }
            else
            {
                Txt_Value.text = string.Format("<color=#C8C8B4>Î´¼¤»î</color>");
            }
        }
    }
}
