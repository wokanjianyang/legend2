using Game;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_MainStage : MonoBehaviour, IBattleLife
{
    public ScrollRect sr_BattleMsg;

    public Button btn_Exit;

    public Text txt_Name;

    public Text TxtMc1;
    public Text TxtMc2;
    public Text TxtMc3;
    public Text TxtMc4;
    public Text TxtMc5;

    private List<Text> msgPool = new List<Text>();
    private int msgId = 0;

    private int CopyMapId = 0;
    private long MapTime = 0;

    public int Order => (int)ComponentOrder.BattleRule;

    // Start is called before the first frame update
    void Start()
    {
        this.btn_Exit.onClick.AddListener(this.OnClick_Exit);
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
        GameProcessor.Inst.EventCenter.AddListener<StartCopyEvent>(this.OnStartCopy);
        GameProcessor.Inst.EventCenter.AddListener<ShowCopyInfoEvent>(this.OnShowCopyInfoEvent);
        GameProcessor.Inst.EventCenter.AddListener<BattleLoseEvent>(this.OnBattleLoseEvent);

        //ShowMapInfo();
        this.gameObject.SetActive(false);
    }
    private void ShowMapInfo(int rate)
    {
        MapConfig config = MapConfigCategory.Instance.Get(this.CopyMapId);
        txt_Name.text = config.Name;
    }

    public void OnStartCopy(StartCopyEvent e)
    {
        this.gameObject.SetActive(true);

        Debug.Log("start copy");

        this.CopyMapId = e.MapId;
        this.MapTime = TimeHelper.ClientNowSeconds();

        Dictionary<string, object> param = new Dictionary<string, object>();
        param.Add("MapId", e.MapId);
        param.Add("MapTime", MapTime);
        param.Add("MapRate", e.Rate);

        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            GameProcessor.Inst.OnDestroy();
            GameProcessor.Inst.LoadMap(RuleType.MainStage, this.transform, param);
        });

        ShowMapInfo(e.Rate);
    }

    public void OnShowCopyInfoEvent(ShowCopyInfoEvent e)
    {
        TxtMc1.text = "剩余小怪：" + e.Mc1;
        TxtMc2.text = "剩余精英：" + e.Mc2;
        TxtMc3.text = "剩余头目：" + e.Mc3;
        TxtMc4.text = "剩余首领：" + e.Mc4;
        TxtMc5.text = "剩余Boss：" + e.Mc5;
    }

    private void OnBattleMsgEvent(BattleMsgEvent e)
    {
        if (e.Type != RuleType.MainStage)
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


        //var msg = GameObject.Instantiate(this.msgPrefab);
        //msg.transform.SetParent(this.sr_BattleMsg.content);
        //msg.transform.localScale = Vector3.one;

        //msg.GetComponent<Text>().text =e.Message;
        //this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
        //GameProcessor.Inst.EventCenter.Raise(new UpdateTowerWindowEvent());
    }

    private void OnBattleLoseEvent(BattleLoseEvent e)
    {
        if (MapTime == e.Time && e.Type == RuleType.MainStage)
        {
            this.Exit();
        }
    }

    private void OnClick_Exit()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认退出？", true, () =>
         {
             this.Exit();
         }, null);
    }

    private void Exit()
    {
        GameProcessor.Inst.OnDestroy();
        this.gameObject.SetActive(false);
        GameProcessor.Inst.EventCenter.Raise(new BattlerEndEvent() { Type = RuleType.MainStage });
        GameProcessor.Inst.SetGameOver(PlayerType.Hero);
        GameProcessor.Inst.DelayAction(0.1f, () =>
        {
            var map = GameObject.Find("Canvas").GetComponentInChildren<View_Battle>(true).transform;
            GameProcessor.Inst.LoadMap(RuleType.Normal, map, null);
        });
    }
}
