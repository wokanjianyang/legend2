using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Babel_Atr : MonoBehaviour
{
    public Button Btn_Close;

    public Transform Tf_Atr_List;
    private Babel_Atr_Item[] AtrList;

    public Transform Tf_Atr_Spe_List;
    private Babel_Atr_Item[] AtrSpeList;


    // Start is called before the first frame update
    void Awake()
    {
        AtrList = Tf_Atr_List.GetComponentsInChildren<Babel_Atr_Item>();
        AtrSpeList = Tf_Atr_Spe_List.GetComponentsInChildren<Babel_Atr_Item>();

        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    // Update is called once per frame
    void Start()
    {
    }

    void OnEnable()
    {
        this.Show();
    }


    private void Show()
    {
        //Log.Debug("ShowStrengthInfo");

        User user = GameProcessor.Inst.User;

        if (user == null)
        {
            return;
        }

        int progress = (int)user.BabelData.Progress.Data;

        List<BabelAtrConfig> configs = BabelAtrConfigCategory.Instance.GetNormalListByProgress(progress + 1);

        for (int i = 0; i < AtrList.Length; i++)
        {
            if (i < configs.Count)
            {
                int attrId = configs[i].AtrId;

                double atrRise = configs[i].AtrValue;

                double attrCurrent = configs[i].AtrValue * ((progress - configs[i].StartLevel) / configs[i].Rate);

                AtrList[i].SetContent(attrId, attrCurrent, atrRise);
                AtrList[i].gameObject.SetActive(true);
            }
            else
            {
                AtrList[i].gameObject.SetActive(false);
            }

        }

        List<BabelAtrConfig> speConfigs = BabelAtrConfigCategory.Instance.GetSpeList();

        for (int i = 0; i < speConfigs.Count; i++)
        {
            if (i < speConfigs.Count)
            {
                int attrId = speConfigs[i].AtrId;
                double atrVue = speConfigs[i].AtrValue;
                int rv = speConfigs[i].StartLevel;

                AtrSpeList[i].SetSpContent(attrId, atrVue, rv);
                AtrSpeList[i].gameObject.SetActive(true);
            }
            else
            {
                AtrSpeList[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}

