using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Achievement : MonoBehaviour, IBattleLife
{
    public Button Btn_Close;

    public ScrollRect sr_Left;
    public ScrollRect sr_Right;

    private List<Achievment_Group> list = new List<Achievment_Group>();
    private List<Achievment_Item> items = new List<Achievment_Item>();

    private GameObject GroupPrefab;
    private GameObject ItemPrefab;

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Awake()
    {
        this.Btn_Close.onClick.AddListener(OnClick_Close);
    }

    // Update is called once per frame
    void Start()
    {
        this.Init();
    }

    public void OnBattleStart()
    {
        GameProcessor.Inst.EventCenter.AddListener<OpenDialogEvent>(this.Open);
    }

    private void Open(OpenDialogEvent e)
    {
        if (e.Type == DialogType.Achievement)
        {
            this.gameObject.SetActive(true);
        }
    }

    private void Init()
    {
        GroupPrefab = Resources.Load<GameObject>("Prefab/Window/Achievement/Ach_Group");
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Achievement/Ach_Item");

        List<AchievementGroupConfig> configs = AchievementGroupConfigCategory.Instance.GetListByPid(0);

        foreach (AchievementGroupConfig config in configs)
        {
            BuildGroupItem(config);
        }

        this.SelectItem(101);
    }

    private void BuildGroupItem(AchievementGroupConfig config)
    {
        var item = GameObject.Instantiate(GroupPrefab);
        var com = item.GetComponent<Achievment_Group>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Left.content);
        item.transform.localScale = Vector3.one;

        list.Add(com);
    }

    public void SelectItem(int gid)
    {

        foreach (var sp in items)
        {
            GameObject.Destroy(sp.gameObject);
        }
        items.Clear();

        List<AchievementConfig> configs = AchievementConfigCategory.Instance.GetListByGid(gid);

        foreach (AchievementConfig config in configs)
        {
            BuildItem(config);
        }
    }

    private void BuildItem(AchievementConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Achievment_Item>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Right.content);
        item.transform.localScale = Vector3.one;

        items.Add(com);
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}

