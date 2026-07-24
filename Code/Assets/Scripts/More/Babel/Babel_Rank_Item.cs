using Game.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Babel_Rank_Item : MonoBehaviour
    {
        public Text Txt_Rank;

        public Text Txt_Name;

        public Text Txt_Progress;

        public Text Txt_Atr;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private int[] atrVueList = { 20, 15, 10, };  //前面排行的奖励
        private int atrVue = 5;  //最后排行的奖励

        public void SetContent(int rank, BabelRank data)
        {
            this.Txt_Rank.text = rank + "";
            this.Txt_Name.text = data.Name;
            this.Txt_Progress.text = data.Rank + "层";

            int av = rank <= atrVueList.Length ? atrVueList[rank - 1] : atrVue;

            this.Txt_Atr.text = StringHelper.FormatAttrText((int)AttributeEnum.MulAtk, av, "+");
        }
    }
}
