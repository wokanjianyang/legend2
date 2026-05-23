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

        public Button Btn_Msg;

        public Button Btn_Achievement;
        public Dialog_Achievement Dlg_Achievement;

        public Button Btn_Task;
        public Dialog_Task Dlg_Task;

        public Button Btn_Stage;

        public Text Txt_MapName;
        public Text Txt_Desc;

        void Start()
        {
            this.Btn_Map.onClick.AddListener(this.OnClick_Map);
            this.Btn_Msg.onClick.AddListener(this.OnClick_Info);
            this.Btn_Achievement.onClick.AddListener(this.OnClick_Achievement);
            this.Btn_Task.onClick.AddListener(this.OnClick_Task);
            this.Btn_Stage.onClick.AddListener(this.OnClick_ToStage);

            this.Init();
        }

        private void Init()
        {
            User user = GameProcessor.Inst.User;
            if (user.OffLineMapId <= 0)
            {
                user.OffLineMapId = 1;
            }

            AppHelper.CurrentMapId = user.OffLineMapId;
            MapConfig config = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);

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


            User user = GameProcessor.Inst.User;

            if (e.Type == RuleType.MainStage)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("MapTime", TimeHelper.ClientNowSeconds());
                param.Add("MapId", user.MapId);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.MainStage, this.transform, param);
                });

                MapConfig config = MapConfigCategory.Instance.Get(user.MapId);
                this.Txt_MapName.text = config.Name + "-关卡挑战";
            }
            if (e.Type == RuleType.Legacy)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("MapTime", TimeHelper.ClientNowSeconds());
                param.Add("Layer", e.MapId);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Legacy, this.transform, param);
                });

                MapConfig config = MapConfigCategory.Instance.Get(user.MapId);
                this.Txt_MapName.text = "传世挑战-" + e.MapId + "阶";
            }
            else
            {
                AppHelper.CurrentMapId = e.MapId;
                user.OffLineMapId = AppHelper.CurrentMapId;

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.SetGameOver(PlayerType.Hero);
                    GameProcessor.Inst.LoadMap(RuleType.Normal, this.transform, null);
                });

                MapConfig config = MapConfigCategory.Instance.Get(e.MapId);
                //this.Txt_Desc.text = "0S击杀0个";
                this.Txt_MapName.text = config.Name;
            }
        }

        private void ShowInfo(ShowMainMapInfoEvent e)
        {
            this.Txt_Desc.text = e.Message;
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
            this.Dlg_Achievement.gameObject.SetActive(true);
        }
        private void OnClick_Task()
        {
            this.Dlg_Task.gameObject.SetActive(true);
        }

        private void OnClick_ToStage()
        {
            User user = GameProcessor.Inst.User;

            int mapId = user.MapId;
            int maxId = MapConfigCategory.Instance.GetMaxMapId();

            if (mapId > maxId)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经超神通关了，请等待开放后续关卡", ToastType = ToastTypeEnum.Failure });
                return;
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ChangeMainMapEvent() { Type = RuleType.MainStage, MapId = mapId });

                //GameProcessor.Inst.EventCenter.Raise(new StartStageEvent());
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();

            //Debug.Log("open view battle");

            //重新计算人物属性
            GameProcessor.Inst.UpdateInfo();
        }
    }
}
