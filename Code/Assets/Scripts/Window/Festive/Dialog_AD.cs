using System;
using System.Collections;
using System.Collections.Generic;
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

    private int CD_Time = 0;

    private int Time_Success = 30;
    private int Time_Error = 3;

    public Button Btn_Close;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Btn_Read1.onClick.AddListener(() => { ReadAd(1); });
        Btn_Read2.onClick.AddListener(() => { ReadAd(2); });
        Btn_Read3.onClick.AddListener(() => { ReadAd(3); });

        Btn_Close.onClick.AddListener(OnClick_Close);

        //string md5 = AppHelper.GetBaseMd5();
        //txt_Rule.text = "md5 length:" + md5.Length + "\n md5:" + md5;
    }

    // Update is called once per frame
    void Update()
    {
        long time = TimeHelper.ClientNowSeconds() - GameProcessor.Inst.User.AdLastTime;
        txt_Time.text = "倒计时:" + Math.Max(0, CD_Time - time);
    }

    public void Open()
    {
        this.UpdateAdData();
        this.gameObject.SetActive(true);
    }

    public void UpdateAdData()
    {
        var @enums = Enum.GetValues(typeof(ADTypeEnum));
        foreach (ADTypeEnum @enum in @enums)
        {
            var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus(@enum);
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

        User user = GameProcessor.Inst.User;
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
        long time = TimeHelper.ClientNowSeconds() - GameProcessor.Inst.User.AdLastTime;

        if (time > CD_Time)
        {
            GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
            return true;
        }

        return false;
    }

    private bool CheckCount(int type)
    {
        var data = GameProcessor.Inst.User.ADShowData?.GetADShowStatus((ADTypeEnum)type);
        if (data == null)
        {
            GameProcessor.Inst.User.ADShowData.ADDatas.Add(new ADData()
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

    private void ReadAd(int type)
    {
        DisableButton();
        GameProcessor.Inst.StartCoroutine(EnableButton());

        //
        if (!CheckCount(type))
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        int skipCount = user.AdData.GetSkipCount();

        if (skipCount > 0 && toggle_Skip.isOn)
        {
            //使用跳过次数
            user.AdData.Use();
            RewardAd(type, true);
        }
        else
        {
            RewardAd(type, false);
        }
    }

    private void ReadAdOld(int type)
    {
        DisableButton();
        GameProcessor.Inst.StartCoroutine(EnableButton());

        //
        if (!CheckCount(type))
        {
            return;
        }

        User user = GameProcessor.Inst.User;

        if (toggle_Skip.isOn || true)
        {
            int skipCount = user.AdData.GetSkipCount();

            if (skipCount > 0 || true)
            {
                //使用跳过次数
                //user.AdData.Use(); 正式之后要改回来
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

        if (toggle_Fail.isOn)
        {   //无法播放,直接给播白屏
            StartCoroutine(ShowFakeAD(() =>
            {
                RewardAd(type, false);
            }));
            return;
        }

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

        GameProcessor.Inst.OnShowVideoAd(des, action, (code) =>
        {
            if (code == (int)AdStateEnum.Reward)
            {
                this.txt_FakeAD.text += "获得奖励";

                RewardAd(type, true);

                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
                this.CD_Time = this.Time_Success;
            }
            else if (code == (int)AdStateEnum.NotSupport || code == (int)AdStateEnum.LoadFail)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "广告加载失败,请稍候再试", ToastType = ToastTypeEnum.Failure });

                GameProcessor.Inst.User.AdLastTime = TimeHelper.ClientNowSeconds();
                this.CD_Time = this.Time_Error;
            }
            else
            {
                //取消的,不处理
            }
        });
    }


    public void RewardAd(int type, bool real)
    {
        User user = GameProcessor.Inst.User;

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


    private void RewardExpAndGold(bool real)  //看的真广告还是假广告
    {
        User user = GameProcessor.Inst.User;

        //发放奖励
        double gold = 100 * 10000;

        double atRate = user.AttributeBonus.CalPanelSingleAttr(AttributeEnum.GoldIncrea);
        atRate = 1 + atRate / 100.0;

        gold = gold * atRate;

        if (real)
        {
            gold = (long)(gold * 1.2);
        }

        user.AddExpAndGold(0, gold);

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, gold, null)
        });
    }

    private void RewardStone(bool real)
    {
        User user = GameProcessor.Inst.User;

        int number = 1000;

        Item item = ItemHelper.BuildMaterial(ItemHelper.Equip_Strong, number);

        List<Item> items = new List<Item>();
        items.Add(item);

        GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent()
        {
            ItemList = items
        });

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Message = BattleMsgHelper.BuildGiftPackMessage("广告奖励", 0, 0, items)
        });
    }
    private void RewardStone1(bool real)
    {
        User user = GameProcessor.Inst.User;

        int number = 100;

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
