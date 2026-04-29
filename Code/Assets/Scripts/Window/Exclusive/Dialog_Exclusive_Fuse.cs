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
    public Text Txt_Attr;
    public Text Txt_Talent;
    public Text Txt_Require;

    public List<Item_Metail_Need> fuseList;

    public Button Btn_OK;
    public Text Txt_Actived;

    private int Tid = 0;

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);
        Btn_OK.onClick.AddListener(OnClick_Ok);
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

        Txt_Attr.text = StringHelper.FormatAttrText(config.AttrId, config.AttrValue);

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


            Txt_Require.text = "需求前置珍宝：" + requireConfig.Name;
        }
        else
        {
            Txt_Require.text = "无前置需求";
        }

        Btn_OK.gameObject.SetActive(true);

        for (int i = 0; i < config.MaterialIdList.Length; i++)
        {
            //Item_Fee
            if (fuseList.Count < i)
            {
                fuseList[i].gameObject.SetActive(false);
            }
            else
            {
                if (!fuseList[i].SetContent(config.MaterialIdList[i], config.MaterialCountList[i]))
                {
                    Btn_OK.gameObject.SetActive(false);
                }
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

        TalentConfig config = TalentConfigCategory.Instance.Get(this.Tid);

        User user = GameProcessor.Inst.User;

        long total = user.TalentExp.Data / 10000;
        long use = user.TalentPoint;


        long level = user.GetTalentLevel(this.Tid);

        if (level < config.MaxLevel && config.Fee <= (total - use))
        {
            user.AddTalentLevel(Tid, config.Fee);

            this.Show();

            GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());

            Dialog_Talent parent = this.GetComponentInParent<Dialog_Talent>();
            parent.Refresh();
        }
    }
}
