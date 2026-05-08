using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class Achievment_Group : MonoBehaviour
{
    public Button Btn_Group;

    public Text Txt_Name;
    public Text Txt_Icon;

    public Transform Tf_Item_List;

    private List<Achievment_Group_Sub> list = new List<Achievment_Group_Sub>();

    private GameObject ItemPrefab;

    private bool expend = false;

    // Start is called before the first frame update
    void Awake()
    {
        Btn_Group.onClick.AddListener(OnClick_Name);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Achievement/Ach_Group_Sub");
    }

    public void SetContent(AchievementGroupConfig config)
    {
        Txt_Name.text = config.Name;

        List<AchievementGroupConfig> configs = AchievementGroupConfigCategory.Instance.GetListByPid(config.Id);

        foreach (AchievementGroupConfig item in configs)
        {
            BuildItem(item);
        }
    }

    private void BuildItem(AchievementGroupConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Achievment_Group_Sub>();

        com.SetContent(config);

        item.transform.SetParent(this.Tf_Item_List);
        item.transform.localScale = Vector3.one;

        list.Add(com);
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