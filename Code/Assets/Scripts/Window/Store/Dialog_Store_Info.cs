using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Store_Info : MonoBehaviour
{

    public Button Btn_Close;

    public Text Txt_Name;
    public Transform Tf_Atr_List;
    private List<Text> Txt_Atr_List;
    public Text Txt_Desc;
    public Text Txt_Fee;

    public Button Btn_Active;
    public Text Txt_Active;


    private int Sid = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_Active.onClick.AddListener(OnClick_Active);
        Txt_Atr_List = Tf_Atr_List.GetComponentsInChildren<Text>().ToList();
    }

    public void Show(StoreConfig config)
    {
        this.gameObject.SetActive(true);
        this.Sid = config.Id;

        Store_Data storeData = User_Data_Manager.StoreData;
        Store_Data_Item data = storeData.StoreList.Where(m => m.StoreId == config.Id).FirstOrDefault();

        for (int i = 0; i < Txt_Atr_List.Count; i++)
        {
            int nb = data != null && data.Number > 1 ? data.Number : 1;
            if (i < config.AtrIdList.Length)
            {
                Txt_Atr_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i] * nb, "+");
                Txt_Atr_List[i].gameObject.SetActive(true);
            }
            else
            {
                Txt_Atr_List[i].gameObject.SetActive(false);
            }
        }

        int quality = config.Quality;

        this.Txt_Name.text = config.Name;
        this.Txt_Name.color = QualityConfigHelper.GetColor(quality);

        if (data == null || data.Number <= 0)
        {
            //未激活
            Txt_Desc.text = config.Des + "（未激活）";
        }
        else
        {
            if (config.SpeId > 0)
            {
                int speLevel = data.Number / config.SpeLevel;
                Txt_Desc.text = StringHelper.FormatAttrText(config.SpeId, config.SpeVue * speLevel, "+");
            }
            else
            {
                Txt_Desc.text = config.Des;
            }
        }

        if (data == null || data.Number < config.Max)
        {
            string color = storeData.Points >= config.Fee ? "#00FF00" : "#FF0000";
            Txt_Fee.text = string.Format("需要积分：<color={0}>{1}</color>（现有：{2})", color, config.Fee, storeData.Points);

            Txt_Fee.gameObject.SetActive(true);
            Btn_Active.gameObject.SetActive(true);
            Txt_Active.gameObject.SetActive(false);
        }
        else
        {
            Txt_Fee.gameObject.SetActive(false);
            Btn_Active.gameObject.SetActive(false);
            Txt_Active.gameObject.SetActive(true);
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Active()
    {
        this.Btn_Active.gameObject.SetActive(false);

        //再加载net数据
        try
        {
            if (User_Data_Manager.Data.Account != "")
            {
                StartCoroutine(NetworkHelper.Convert‌Store(Sid,
                    (WebResultWrapper result) =>
                    {
                        if (result.Code == StatusMessage.OK)
                        {
                            JToken store = result.Extend.SelectToken("StoreData");
                            User_Data_Manager.StoreData = store.ToObject<Store_Data>();

                            Panel_Store panel = this.GetComponentInParent<Panel_Store>();
                            panel.ConvertSuccess(Sid);

                            GameProcessor.Inst.UpdateInfo();

                            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "兑换成功", ToastType = ToastTypeEnum.Success });
                            this.gameObject.SetActive(false);

                        }
                        else
                        {
                            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = result.Msg, ToastType = ToastTypeEnum.Failure });
                        }

                    },
                     () =>
                     {
                         GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获取数据失败", ToastType = ToastTypeEnum.Failure });
                     }));
            }
        }
        catch (Exception ex)
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "获取数据失败", ToastType = ToastTypeEnum.Failure });
        }
    }
}
