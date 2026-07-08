using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Lottery : MonoBehaviour
{
    public Transform Tf_Nav;
    private List<Toggle> toggleStageList = new List<Toggle>();

    public ScrollRect Sr_Bag;
    private List<Lottery_Item> bagList = new List<Lottery_Item>();

    public Text Txt_Points;
    public Text Txt_Lottery;
    public Text Txt_Pity;

    public Button Btn_OK;
    public Button Btn_Batch;

    private int SelectType = 0;

    private GameObject PrefabItem = null;


    public Dialog_Lottery_Info Dlg_Lottery_Info;
    public Dialog_Lottery_Result Dlg_Lottery_Result;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.Btn_OK.onClick.AddListener(OnClick_Ok);
        this.Btn_Batch.onClick.AddListener(OnClick_Batch);

        PrefabItem = Resources.Load<GameObject>("Prefab/Window/Store/Lottery_Item");

        toggleStageList = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < toggleStageList.Count; i++)
        {
            int index = i;
            toggleStageList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        this.Init();
        //this.ChangePanel(0);
    }

    void OnEnable()
    {
        this.ShowPoints();
    }

    public void Init()
    {
        List<StoreConfig> list = StoreConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            Lottery_Item item = CreateItem();
            item.Init(list[i]);
            this.bagList.Add(item);
        }
    }

    private Lottery_Item CreateItem()
    {
        var go = GameObject.Instantiate(PrefabItem);
        Lottery_Item comItem = go.GetComponent<Lottery_Item>();
        //comItem.SetItem(item, type, cycle);

        comItem.transform.SetParent(Sr_Bag.content);
        comItem.transform.localPosition = Vector3.zero;
        comItem.transform.localScale = Vector3.one;

        return comItem;
    }

    private void ChangePanel(int index)
    {
        this.SelectType = index;

        foreach (Lottery_Item sp in bagList)
        {
            sp.ChangeType(SelectType + 3);
        }
    }

    public void ShowInfo(StoreConfig config)
    {
        Dlg_Lottery_Info.Show(config);
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_Batch.gameObject.SetActive(false);

        if (User_Data_Manager.StoreData.Lottery < 1)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "抽奖次数不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.ToLottery(1);
    }

    public void OnClick_Batch()
    {
        this.Btn_OK.gameObject.SetActive(false);
        this.Btn_Batch.gameObject.SetActive(false);

        if (User_Data_Manager.StoreData.Lottery < 10)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "抽奖次数不足", ToastType = ToastTypeEnum.Failure });
            return;
        }

        this.ToLottery(10);
    }

    private void ToLottery(int count)
    {
        //再加载net数据
        try
        {
            if (User_Data_Manager.Data.Account != "")
            {
                StartCoroutine(NetworkHelper.ToLottery(count,
                    (WebResultWrapper result) =>
                    {
                        if (result.Code == StatusMessage.OK)
                        {
                            JToken lotteryData = result.Extend.SelectToken("LotteryData");
                            Lottery_Result lr = lotteryData.ToObject<Lottery_Result>();

                            JToken store = result.Extend.SelectToken("StoreData");
                            User_Data_Manager.StoreData = store.ToObject<Store_Data>();

                            SuccessResult(lr);
                        }
                        else
                        {
                            ErrorResutlt();
                        }

                    },
                     () =>
                     {
                         ErrorResutlt();
                     }));
            }
        }
        catch (Exception ex)
        {
            ErrorResutlt();
        }
    }

    private void SuccessResult(Lottery_Result lr)
    {
        Dlg_Lottery_Result.ShowResult(lr);

        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_Batch.gameObject.SetActive(true);

        this.ShowPoints();

        GameProcessor.Inst.UpdateInfo();
    }

    private void ShowPoints()
    {
        this.Txt_Lottery.text = "拥有抽奖次数：" + User_Data_Manager.StoreData.Lottery;
        this.Txt_Points.text = "拥有积分：" + User_Data_Manager.StoreData.Points + "";

        string txtPity = "";
        if (User_Data_Manager.StoreData.Pity1 > 0)
        {
            txtPity += User_Data_Manager.StoreData.Pity1 + "次后必中红色";
        }
        if (User_Data_Manager.StoreData.Pity1 > 0)
        {
            txtPity += User_Data_Manager.StoreData.Pity1 + "次后必中金色";
        }

        this.Txt_Pity.text = txtPity;
    }

    private void ErrorResutlt()
    {
        this.Btn_OK.gameObject.SetActive(true);
        this.Btn_Batch.gameObject.SetActive(true);

        GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "抽取失败", ToastType = ToastTypeEnum.Failure });
    }
}
