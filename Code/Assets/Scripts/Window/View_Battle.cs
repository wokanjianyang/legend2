using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class View_Battle : AViewPage
    {
        public ScrollRect sr_BattleMsg;

        public Button Btn_Map;
        public Main_Map_Dialog MapMain;

        public Button Btn_Info;
        public Button Btn_Achievement;

        public Button Btn_Stage;

        public Text Txt_MapName;
        public Text Txt_Desc;

        void Start()
        {
            this.Btn_Map.onClick.AddListener(this.OnClick_Map);
            this.Btn_Info.onClick.AddListener(this.OnClick_Info);
            this.Btn_Achievement.onClick.AddListener(this.OnClick_Achievement);

            this.Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;

            MapConfig config = MapConfigCategory.Instance.Get(user.MapId);
            this.Txt_Desc.text = "0S击杀0个";
            this.Txt_MapName.text = config.Name;
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            return true;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
            GameProcessor.Inst.EventCenter.AddListener<ChangeMainMapEvent>(this.OnChangeMap);
            GameProcessor.Inst.EventCenter.AddListener<ShowMainMapInfoEvent>(this.ShowInfo);

        }

        private void OnChangeMap(ChangeMainMapEvent e)
        {
            MapConfig config = MapConfigCategory.Instance.Get(e.MapId);
            //this.Txt_Desc.text = "0S击杀0个";
            this.Txt_MapName.text = config.Name;
        }

        private void ShowInfo(ShowMainMapInfoEvent e)
        {
            this.Txt_Desc.text = e.Time + "S击杀" + e.Count + "个";
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
                var msg = GameObject.Instantiate(PrefabHelper.Instance().DropMessagePrefab());
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

        private void OnClick_Map()
        {
            MapMain.gameObject.SetActive(true);
        }
        private void OnClick_Info()
        {

        }
        private void OnClick_Achievement()
        {

        }

        public override void OnOpen()
        {
            base.OnOpen();

            Debug.Log("open view battle");

            //重新计算人物属性
            GameProcessor.Inst.UpdateInfo();
        }
    }
}
