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

        public Dialog_Drop_Message Dlg_Drop_Message;
        public Button Btn_Msg;

        public Button Btn_Offline;
        public Dialog_State_Offline Dlg_Offline;

        public Button Btn_Mode;
        public Text Txt_Mode;

        public Button Btn_Task;
        public Dialog_Task Dlg_Task;

        public Button Btn_Stage;

        public Text Txt_MapName;
        public Text Txt_Desc;

        void Start()
        {
            this.Btn_Map.onClick.AddListener(this.OnClick_Map);
            this.Btn_Msg.onClick.AddListener(this.OnClick_Msg);
            this.Btn_Offline.onClick.AddListener(this.OnClick_Offline);
            this.Btn_Task.onClick.AddListener(this.OnClick_Task);
            this.Btn_Stage.onClick.AddListener(this.OnClick_ToStage);
            this.Btn_Mode.onClick.AddListener(this.OnClick_ChangeModel);

            this.Init();
        }

        private void Init()
        {
            User user = User_Data_Manager.Data;
            if (user.OffLineMapId <= 0)
            {
                user.OffLineMapId = 1;
            }

            AppHelper.CurrentMapId = user.OffLineMapId;
            MapConfig config = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);

            this.Txt_Desc.text = "0S击杀0个";
            this.Txt_MapName.text = string.Format("{0}（N{1}）", config.Name, AppHelper.CurrentMapModel);
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


            User user = User_Data_Manager.Data;

            AppHelper.CurrentRuleType = e.Type;

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
            else if (e.Type == RuleType.Legacy)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("MapTime", TimeHelper.ClientNowSeconds());
                param.Add("MapId", e.MapId);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Legacy, this.transform, param);
                });

                string type = "普通";
                if (e.MapId == 1)
                {
                    type = "高级";
                }
                else if (e.MapId == 2)
                {
                    type = "超级";
                }

                MapConfig config = MapConfigCategory.Instance.Get(user.MapId);
                this.Txt_MapName.text = "传世挑战(" + type + ")";
            }
            else if (e.Type == RuleType.Defend)
            {
                DefendRecord record = user.DefendData.GetCurrentRecord(AppHelper.DefendLevel);

                if (record == null)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "没有了挑战次数", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("progress", record.Progress);
                param.Add("hp", record.Hp);
                param.Add("count", record.Count);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Defend, this.transform, param);
                });

                this.Txt_MapName.text = "守卫龙城";
            }
            else if (e.Type == RuleType.Materail)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("MapTime", TimeHelper.ClientNowSeconds());
                param.Add("MapId", e.MapId);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Materail, this.transform, param);
                });


                this.Txt_MapName.text = "材料副本";
            }
            else if (e.Type == RuleType.Babel)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Babel, this.transform, param);
                });

                this.Txt_MapName.text = "通天塔";
            }
            else if (e.Type == RuleType.Offline)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("MapTime", TimeHelper.ClientNowSeconds());
                param.Add("MapId", e.MapId);
                param.Add("Model", AppHelper.CurrentMapModel);

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.LoadMap(RuleType.Offline, this.transform, param);
                });

                MapConfig config = MapConfigCategory.Instance.Get(e.MapId);
                this.Txt_MapName.text = "离线记录-" + config.Name + "（N" + AppHelper.CurrentMapModel + "）";
            }
            else
            {
                AppHelper.CurrentMapId = e.MapId;
                AppHelper.CurrentMapModel = 1; //换图默认为N1
                user.OffLineMapId = AppHelper.CurrentMapId;

                GameProcessor.Inst.DelayAction(0.1f, () =>
                {
                    GameProcessor.Inst.OnDestroy();
                    GameProcessor.Inst.SetGameOver(PlayerType.Hero);
                    GameProcessor.Inst.LoadMap(RuleType.Normal, this.transform, null);
                });

                MapConfig config = MapConfigCategory.Instance.Get(e.MapId);
                //this.Txt_Desc.text = "0S击杀0个";
                this.Txt_MapName.text = string.Format("{0}（N{1}）", config.Name, AppHelper.CurrentMapModel);
                this.Txt_Mode.text = "难度N" + AppHelper.CurrentMapModel;
            }
        }

        private void ShowInfo(ShowMainMapInfoEvent e)
        {
            if (!string.IsNullOrEmpty(e.Title))
            {
                this.Txt_MapName.text = e.Title;
            }

            if (!string.IsNullOrEmpty(e.Message))
            {
                this.Txt_Desc.text = e.Message;
            }
        }

        private List<Text> msgPool = new List<Text>();
        private int msgId = 0;
        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            //if (e.Type != RuleType.Normal)
            //{
            //    return;
            //}

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
        private void OnClick_Msg()
        {
            Dlg_Drop_Message.gameObject.SetActive(true);
        }
        private void OnClick_Offline()
        {
            this.Dlg_Offline.gameObject.SetActive(true);
        }
        private void OnClick_Task()
        {
            this.Dlg_Task.gameObject.SetActive(true);
        }

        private void OnClick_ToStage()
        {
            long ns = TimeHelper.ClientNowSeconds();
            if (ns - AppHelper.ChangeMapTime < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请稍后点击，间隔少于10秒", ToastType = ToastTypeEnum.Failure });
                return;
            }
            else
            {
                AppHelper.ChangeMapTime = ns;
            }

            User user = User_Data_Manager.Data;

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

        private void OnClick_ChangeModel()
        {
            if (AppHelper.CurrentRuleType == RuleType.Normal)
            {
                MapConfig config = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);
                //int MaxModel = Mathf.Min(5, config.GroupId) + 1;

                int MaxModel = 6;

                AppHelper.CurrentMapModel = (AppHelper.CurrentMapModel) % MaxModel + 1;

                this.Txt_Mode.text = "难度N" + AppHelper.CurrentMapModel;

                this.Txt_MapName.text = string.Format("{0}（N{1}）", config.Name, AppHelper.CurrentMapModel);
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "只有主线副本可以切换难度", ToastType = ToastTypeEnum.Failure });
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
