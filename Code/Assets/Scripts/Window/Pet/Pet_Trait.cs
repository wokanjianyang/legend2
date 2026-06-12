using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Pet_Trait : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Desc;


        public void SetContent(int tid, int level, int type)
        {
            PetTraitConfig config = PetTraitConfigCategory.Instance.Get(tid);

            string tl = ConfigHelper.LayerAlbList[level - 1];
            if (type == 2)
            {
                tl += "£®±‰“Ï£©";
            }
            this.Txt_Name.text = config.Name + tl;

            string desc = "";
            for (int i = 0; i < config.AtrIdList.Length; i++)
            {
                int vue = type == 1 ? config.AtrVueList[i] : config.AtrVueList1[i];

                desc += StringHelper.FormatAttrText(config.AtrIdList[i], vue, "+");
            }

            this.Txt_Desc.text = desc;
        }
    }
}
