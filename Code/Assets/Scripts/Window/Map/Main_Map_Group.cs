using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class Main_Map_Group : MonoBehaviour
{
    public Button Btn_Group;

    public Toggle toggle;

    public Text Txt_Name;
    public Text Txt_Desc;
    public Text Txt_Icon;

    public Transform Tf_Item_List;

    private List<Main_Map_Item> Item_List = new List<Main_Map_Item>();

    private bool expend = false;

    private MapGroupConfig Config;

    // Start is called before the first frame update
    void Awake()
    {
        Item_List = Tf_Item_List.GetComponentsInChildren<Main_Map_Item>(true).ToList();

        Btn_Group.onClick.AddListener(OnClick_Name);

        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (this.Config != null)
            {
                AppHelper.SetData.BossOpen[Config.Id] = isOn;
                User_Data_Manager.SettingSave();
            }
        });
    }


    public void SetContent(MapGroupConfig config)
    {
        this.Config = config;

        if (AppHelper.SetData.BossOpen.ContainsKey(config.Id))
        {
            toggle.isOn = AppHelper.SetData.BossOpen[config.Id];
        }

        Txt_Name.text = config.Name;
        Txt_Desc.text = config.Memo;

        List<MapConfig> configs = MapConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.GroupId == config.Id).ToList();

        for (int i = 0; i < Item_List.Count; i++)
        {
            Item_List[i].SetContent(configs[i]);
        }
    }

    private void OnClick_Name()
    {
        expend = !expend;

        if (expend)
        {
            Txt_Icon.text = "▼";
            Tf_Item_List.DOScaleY(1, 0.15f).OnComplete(() =>
            {
                Tf_Item_List.gameObject.SetActive(true);
            }); ;
        }
        else
        {
            Txt_Icon.text = "▶";
            Tf_Item_List.DOScaleY(0, 0.2f).OnComplete(() =>
            {
                Tf_Item_List.gameObject.SetActive(false);
            });


        }


    }

    public void Show()
    {
        User user = User_Data_Manager.Data;
        if (user == null || this.Config == null)
        {
            return;
        }

        int gid = (user.MapId - 1) / 6 + 1;

        if (Config.Id <= gid)
        {
            this.gameObject.SetActive(true);

            foreach (Main_Map_Item item in Item_List)
            {
                item.Show();
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}