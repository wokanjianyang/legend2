using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewMap : AViewPage
    {
        public ScrollRect sr_BattleMsg;

        public Button Btn_Map;
        public Dialog_BossInfo BossInfo;

        public Button Btn_Info;

        public Button Btn_Achievement;

        public Text txt_MapName;
        public Text txt_Desc1;

        private GameObject msgPrefab;

        void Start()
        {
            this.Btn_Map.onClick.AddListener(this.OnClick_Map);
            this.Btn_Info.onClick.AddListener(this.OnClick_Info);
            this.Btn_Achievement.onClick.AddListener(this.OnClick_Achievement);
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            return true;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
 
            ShowName();
        }

        private List<Text> msgPool = new List<Text>();
        private int msgId = 0;
        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            if (e.Type != RuleType.Normal)
            {
                return;
            }

            msgId++;
            Text txt_msg = null;
            if (this.sr_BattleMsg.content.childCount > 50)
            {
                txt_msg = msgPool[0];
                msgPool.RemoveAt(0);
                txt_msg.transform.SetAsLastSibling();
            }
            else
            {
                var msg = GameObject.Instantiate(this.msgPrefab);
                msg.transform.SetParent(this.sr_BattleMsg.content);
                msg.transform.localScale = Vector3.one;

                var m = msg.GetComponent<Text>();
                

                txt_msg = m;
            }
            msgPool.Add(txt_msg);

            txt_msg.gameObject.name = $"msg_{msgId}";
            txt_msg.text = e.Message;
            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
        }

        private void ShowName()
        {
            User user = GameProcessor.Inst.User;

            if (user != null)
            {
                txt_MapName.text = user.MagicTowerFloor.Data + "层";
            }

        }

        private void OnClick_Map()
        {
            BossInfo.gameObject.SetActive(true);
        }
        private void OnClick_Info()
        {
      
        }
        private void OnClick_Achievement()
        {
         
        }
    }
}
