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

    public Text Txt_Name;
    public Text Txt_Desc;
    public Text Txt_Icon;

    public Transform Tf_Item_List;

    private List<Main_Map_Item> Item_List = new List<Main_Map_Item>();

    private bool expend = false;

    // Start is called before the first frame update
    void Awake()
    {
        Item_List = Tf_Item_List.GetComponentsInChildren<Main_Map_Item>(true).ToList();

        Btn_Group.onClick.AddListener(OnClick_Name);
    }

    public void SetContent(MapGroupConfig config)
    {
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
}