using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Exclusive_Fuse : MonoBehaviour
{

    public Button Btn_Close;

    public Text Txt_Name;
    public Transform Tf_Atr_List;
    private List<Text> Txt_Atr_List;
    public Text Txt_Talent;
    public Text Txt_Require;

    public Transform Tf_Fuse;
    private List<Item_Metail_Need> fuseList;

    public Button Btn_OK;
    public Text Txt_Actived;

    private int Tid = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
        Txt_Atr_List = Tf_Atr_List.GetComponentsInChildren<Text>().ToList();
        fuseList = Tf_Fuse.GetComponentsInChildren<Item_Metail_Need>().ToList();
    }

    public void Open(int tid)
    {
        this.gameObject.SetActive(true);

        this.Tid = tid;
        this.Show();
    }

    public void Show()
    {
        ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        Txt_Name.text = config.Name;

        for (int i = 0; i < config.AtrIdList.Length; i++)
        {
            Txt_Atr_List[i].text = StringHelper.FormatAttrText(config.AtrIdList[i], config.AtrVueList[i]);
        }

        if (config.TalentId == 0)
        {
            Txt_Talent.text = "没有特殊效果";
        }
        else
        {
            Txt_Talent.text = "获得天赋：" + config.Des;
        }

        if (config.RequireId > 0)
        {
            ExclusiveConfig requireConfig = ExclusiveConfigCategory.Instance.Get(config.RequireId);

            string color = user.GetExclusiveLevel(config.RequireId) > 0 ? "#11FF11" : "#FF0000";

            Txt_Require.text = "" + string.Format("<color={0}>需求前置珍宝：{1}</color>", color, requireConfig.Name);
        }
        else
        {
            Txt_Require.text = string.Format("<color={0}>无前置需求</color>", "#11FF11");
        }


        if (user.GetExclusiveLevel(Tid) > 0)
        {
            Btn_OK.gameObject.SetActive(false);
            Txt_Actived.gameObject.SetActive(true);
        }
        else
        {
            Txt_Actived.gameObject.SetActive(false);

            if (config.RequireId == 0 || user.GetExclusiveLevel(config.RequireId) > 0)
            {
                Btn_OK.gameObject.SetActive(true);

                for (int i = 0; i < config.MidList.Length; i++)
                {
                    //Item_Fee
                    if (fuseList.Count < i)
                    {
                        fuseList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!fuseList[i].SetMaterialContent(config.MidList[i], config.McList[i]))
                        {
                            Btn_OK.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                Btn_OK.gameObject.SetActive(false);
            }
        }


    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClick_Ok()
    {
        Btn_OK.gameObject.SetActive(false);

        ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < config.MidList.Length; i++)
        {
            int mid = config.MidList[i];
            int count = config.McList[i];

            long stoneTotal = user.GetHideMaterialCount(mid);
            if (stoneTotal < count)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "材料不足", ToastType = ToastTypeEnum.Failure });
                return;
            }
        }

        for (int i = 0; i < config.MidList.Length; i++)
        {
            int mid = config.MidList[i];
            int count = config.McList[i];

            user.UseHideMaterialCount(mid, count);
        }

        user.ExclusiveDict[Tid] = 1;

        this.gameObject.SetActive(false);

        GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());

        Panel_Exclusive parent = this.GetComponentInParent<Panel_Exclusive>();
        parent.Refresh();

    }
}
