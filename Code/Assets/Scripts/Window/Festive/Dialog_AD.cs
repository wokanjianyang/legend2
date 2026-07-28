using System;
using System.Collections;
using System.Collections.Generic;
using AnyThinkAds.Api;
using Game;
using Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Dialog_AD : MonoBehaviour
{
    public Text Txt_Count1;
    public Button Btn_Read1;

    public Text Txt_Count2;
    public Button Btn_Read2;

    public Text Txt_Count3;
    public Button Btn_Read3;

    public Toggle toggle_Fail;

    public Transform Tf_Skip;
    public Toggle toggle_Skip;
    public Text txt_Skip;

    public Text txt_Time;
    public Transform tran_FakeAD;
    public Text txt_FakeAD;

    public Text txt_Test;

    private int CD_Time = 0;

    private int Time_Success = 30;
    private int Time_Error = 3;

    public Button Btn_Close;

    private string appId = "a6a59c554cc937";
    private string appKey = "a1bf8be0a390efa79934bd981449f3ec6";
    private string mPlacementId_rewardvideo_all = "b6a59c565b561d";
    private int AdType = 0;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Read1.onClick.AddListener(() => { ReadAdOld(1); });
        Btn_Read2.onClick.AddListener(() => { ReadAdOld(2); });
        Btn_Read3.onClick.AddListener(() => { ReadAdOld(3); });

        Btn_Close.onClick.AddListener(OnClick_Close);

        //string md5 = AppHelper.GetBaseMd5();
        //txt_Rule.text = "md5 length:" + md5.Length + "\n md5:" + md5;
        this.InitAd();
    }

    // Update is called once per frame
    void Update()
    {
        long time = TimeHelper.ClientNowSeconds() - User_Data_Manager.Data.AdLastTime;
        txt_Time.text = "倒计时:" + Math.Max(0, CD_Time - time);
    }

    public void Open()
    {
        this.UpdateAdData();
        this.gameObject.SetActive(true);
    }

    private void InitAd()
    {
        //ATSDKAPI.initSDK(appId, appKey);//Use your own app_id & app_key here

        //加载广告
        ATRewardedVideo.Instance.client.onAdLoadEvent += onAdLoad;
        ATRewardedVideo.Instance.client.onAdLoadFailureEvent += onAdLoadFail;
        ATRewardedVideo.Instance.client.onAdVideoStartEvent += onAdVideoStartEvent;
        ATRewardedVideo.Instance.client.onAdVideoEndEvent += onAdVideoEndEvent;
        ATRewardedVideo.Instance.client.onAdVideoFailureEvent += onAdVideoPlayFail;
        ATRewardedVideo.Instance.client.onAdClickEvent += onAdClick;
        ATRewardedVideo.Instance.client.onRewardEvent += onReward;
        ATRewardedVideo.Instance.client.onAdVideoCloseEvent += onAdVideoClosedEvent;
        //如果需要通过开发者的服务器进行奖励的下发（部分广告平台支持此服务器激励），则需要传递下面两个key
        //ATConst.USERID_KEY必传，用于标识每个用户;ATConst.USER_EXTRA_DATA为可选参数，传入后将透传到开发者的服务器
        //jsonmap.Add(ATConst.USERID_KEY, "test_user_id");
        //jsonmap.Add(ATConst.USER_EXTRA_DATA, "test_user_extra_data");
    }

    public void UpdateAdData()
    {
        var @enums = Enum.GetValues(typeof(ADTypeEnum));
        foreach (ADTypeEnum @enum in @enums)
        {
            var data = User_Data_Manager.Data.ADShowData?.GetADShowStatus(@enum);
            if (data == null)
            {
                continue;
            }
            switch (@enum)
            {
                case ADTypeEnum.GoldCount:
                    this.Txt_Count1.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
                case ADTypeEnum.StoneCount:
                    this.Txt_Count2.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
                case ADTypeEnum.Stone1Count:
                    this.Txt_Count3.text = $"{data.CurrentShowCount}/{data.MaxShowCount}";
                    break;
            }
        }

        User user = User_Data_Manager.Data;
        int skipCount = user.AdData.GetSkipCount();
        txt_Skip.text = skipCount + "";

        if (skipCount > 0)
        {
            toggle_Skip.gameObject.SetActive(true);

            txt_Skip.gameObject.SetActive(true);
        }
        else
        {
            toggle_Skip.gameObject.SetActive(false);
            txt_Skip.gameObject.SetActive(false);
        }
    }

    private bool CheckCd()
    {
        long time = TimeHelper.ClientNowSeconds() - User_Data_Manager.Data.AdLastTime;

        if (time > CD_Time)
        {
            User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
            return true;
        }

        return false;
    }

    private bool CheckCount(int type)
    {
        var data = User_Data_Manager.Data.ADShowData?.GetADShowStatus((ADTypeEnum)type);
        if (data == null)
        {
            User_Data_Manager.Data.ADShowData.ADDatas.Add(new ADData()
            {
                ADType = type,
                CurrentShowCount = 0,
                MaxShowCount = 3
            });
        }

        if (data.CurrentShowCount >= data.MaxShowCount)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告次数已用尽，请观看其它广告或明日再来", ToastType = ToastTypeEnum.Failure });
            return false;
        }

        return true;
    }

    public void DisableButton()
    {
        Btn_Read1.gameObject.SetActive(false);
        Btn_Read2.gameObject.SetActive(false);
        Btn_Read3.gameObject.SetActive(false);
    }

    public IEnumerator EnableButton()
    {
        yield return new WaitForSeconds(3f);

        Btn_Read1.gameObject.SetActive(true);
        Btn_Read2.gameObject.SetActive(true);
        Btn_Read3.gameObject.SetActive(true);
    }

    //private void ReadAd(int type)
    //{
    //    DisableButton();
    //    GameProcessor.Inst.StartCoroutine(EnableButton());

    //    //
    //    if (!CheckCount(type))
    //    {
    //        return;
    //    }

    //    User user = User_Data_Manager.Data;

    //    int skipCount = user.AdData.GetSkipCount();

    //    if (skipCount > 0 && toggle_Skip.isOn)
    //    {
    //        //使用跳过次数
    //        user.AdData.Use();
    //        RewardAd(type, true);
    //    }
    //    else
    //    {
    //        RewardAd(type, false);
    //    }
    //}

    private void ReadAdOld(int type)
    {
        DisableButton();
        GameProcessor.Inst.StartCoroutine(EnableButton());

        //
        if (!CheckCount(type))
        {
            return;
        }

        User user = User_Data_Manager.Data;

        if (toggle_Skip.isOn || ConfigHelper.SrvId >= 98)
        {
            int skipCount = user.AdData.GetSkipCount();

            if (skipCount > 0 || ConfigHelper.SrvId >= 98)
            {
                //使用跳过次数
                user.AdData.Use(); //正式之后要改回来
                RewardAd(type, true);
                return;
            }
        }

        //
        if (!CheckCd())
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告CD间隔" + CD_Time + "S，请稍候", ToastType = ToastTypeEnum.Failure });
            return;
        }

        if (1 == 1)
        {   //没有软著，先播放白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardAd(type, true);
            }));
            return;
        }

        //if (toggle_Fail.isOn)
        //{   //无法播放,直接给播白屏
        //    StartCoroutine(ShowFakeAD(() =>
        //    {
        //        RewardAd(type, false);
        //    }));
        //    return;
        //}

        string des = "";
        string action = "";
        switch (type)
        {
            case 1:
                des = "金币奖励";
                action = "ad1";
                break;
            case 2:
                des = "铜矿石奖励";
                action = "ad2";
                break;
            case 3:
                des = "黑铁矿奖励";
                action = "ad3";
                break;
            default:
                break;
        }


        this.AdType = type;

        Dictionary<string, string> jsonmap = new Dictionary<string, string>();
        ATRewardedVideo.Instance.loadVideoAd(mPlacementId_rewardvideo_all, jsonmap);
        bool hasReady = ATRewardedVideo.Instance.hasAdReady(mPlacementId_rewardvideo_all);
        Debug.Log("hasReady：" + hasReady);

        Debug.Log("Developer show video....");
        ATRewardedVideo.Instance.showAd(mPlacementId_rewardvideo_all);

        //GameProcessor.Inst.OnShowVideoAd(des, action, (code) =>
        //{
        //    if (code == (int)AdStateEnum.Reward)
        //    {
        //        this.txt_FakeAD.text += "获得奖励";

        //        RewardAd(type, true);

        //        User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
        //        this.CD_Time = this.Time_Success;
        //    }
        //    else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
        //    {
        //        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });

        //        User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
        //        this.CD_Time = this.Time_Error;
        //    }
        //    else
        //    {
        //        //取消的,不处理
        //    }
        //});
    }

    //广告加载成功
    public void onAdLoad(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoad :" + erg.placementId);
    }
    //广告加载失败
    public void onAdLoadFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer callback onAdLoadFail :" + erg.placementId + "--erg.code:" + erg.errorCode + "--msg:" + erg.errorMessage);

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });

        this.txt_Test.gameObject.SetActive(true);
        this.txt_Test.text = erg.errorCode + erg.errorMessage;

        User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
        this.CD_Time = this.Time_Error;
    }

    public void onAdVideoStartEvent(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoStartEvent------" + "->" + JsonUtility.ToJson(erg.callbackInfo.toDictionary()));
    }

    public void onAdVideoEndEvent(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoEndEvent------" + "->" + JsonUtility.ToJson(erg.callbackInfo.toDictionary()));
    }


    public void onAdVideoPlayFail(object sender, ATAdErrorEventArgs erg)
    {
        Debug.Log("Developer onAdVideoClosedEvent------" + "->" + JsonUtility.ToJson(erg.errorMessage));

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告播放失败,请稍候再试", ToastType = ToastTypeEnum.Failure });

        User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
        this.CD_Time = this.Time_Error;
    }


    //sender 为广告类型对象，erg为返回信息
    //广告被点击
    public void onAdClick(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer callback onAdClick :" + erg.placementId);
    }

    public void onReward(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onReward------" + "->" + JsonUtility.ToJson(erg.callbackInfo.toDictionary()));

        RewardAd(AdType, true);

        User_Data_Manager.Data.AdLastTime = TimeHelper.ClientNowSeconds();
        this.CD_Time = this.Time_Success;
    }


    public void onAdVideoClosedEvent(object sender, ATAdEventArgs erg)
    {
        Debug.Log("Developer onAdVideoClosedEvent------" + "->" + JsonUtility.ToJson(erg.callbackInfo.toDictionary()));
    }


    public void RewardAd(int type, bool real)
    {
        User user = User_Data_Manager.Data;

        var data = user.ADShowData?.GetADShowStatus((ADTypeEnum)type);

        if (data.CurrentShowCount >= 3)
        {
            return;
        }

        data.CurrentShowCount++;

        if (!user.Record.Check())
        {
            return;
        }

        switch (type)
        {
            case 1:
                RewardExpAndGold(real);
                break;
            case 2:
                RewardStone(real);
                break;
            case 3:
                RewardStone1(real);
                break;
            default:
                break;
        }

        user.Record.AddRecord();

        this.UpdateAdData();
    }

    private double GetMapRate(int mapId)
    {
        int mapRise = mapId - ConfigHelper.MapStartId;

        return mapRise * 5 / 100.0;
    }

    private void RewardExpAndGold(bool real)  //看的真广告还是假广告
    {
        User user = User_Data_Manager.Data;

        //发放奖励
        double gold = 200 * 10000;

        double atRate = user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea);
        atRate = 1 + atRate / 100.0;

        double mapRate = 1 + GetMapRate(user.MapId);

        gold = gold * atRate * mapRate;

        if (real)
        {
            gold = (long)(gold * 1.2);
        }

        user.AddExpAndGold(0, gold);

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Important = 1,
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, gold, null)
        });
    }

    private void RewardStone(bool real)
    {
        User user = User_Data_Manager.Data;

        int number = 2000;

        double mapRate = 1 + GetMapRate(user.MapId);

        number = (int)(number * mapRate);

        Item item = ItemHelper.BuildMaterial(ItemHelper.Equip_Strong, number);

        List<Item> items = new List<Item>();
        items.Add(item);

        GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
        });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Important = 1,
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, 0, items)
        });
    }
    private void RewardStone1(bool real)
    {
        User user = User_Data_Manager.Data;

        int number = 100;

        double mapRate = 1 + GetMapRate(user.MapId);

        number = (int)(number * mapRate);

        if (real)
        {
            number = (int)(number * 1.2);
        }

        Item item = ItemHelper.BuildMaterial(ItemHelper.Equip_Refine, number);

        List<Item> items = new List<Item>();
        items.Add(item);

        GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
        });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Important = 1,
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, 0, items)
        });
    }

    private IEnumerator ShowFakeAD(Action endCallback)
    {
        this.tran_FakeAD.gameObject.SetActive(true);
        var duration = RandomHelper.RandomNumber(45, 60);
        for (int i = duration; i > 0; i--)
        {
            this.txt_FakeAD.text = $"再看{i}秒广告就发奖励";
            yield return new WaitForSeconds(1f);
        }
        this.tran_FakeAD.gameObject.SetActive(false);

        endCallback?.Invoke();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
