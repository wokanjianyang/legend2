using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Main_Map_Dialog : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button Btn_Close;

    public Transform Tf_Layer;

    private List<Toggle> tgLevelList;
    private int LevelCount = 12; //每个难度多少个

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    List<Main_Map_Group> items = new List<Main_Map_Group>();

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        tgLevelList = Tf_Layer.GetComponentsInChildren<Toggle>().ToList();
        for (int i = 0; i < tgLevelList.Count; i++)
        {
            int index = i;
            tgLevelList[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangeLevel(index);
            });
        }

        this.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ChangeLevel(0);
    }

    void OnEnable()
    {
        this.Show();
    }
    private void Init()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Map/Main_Map_Group");

        List<MapGroupConfig> list = MapGroupConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (MapGroupConfig config in list)
        {
            BuildItem(config);
        }
    }

    private void BuildItem(MapGroupConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Main_Map_Group>();

        com.SetContent(config);

        item.transform.SetParent(this.sr_Boss.content);
        item.transform.localScale = Vector3.one;
        item.gameObject.SetActive(false);

        items.Add(com);
    }

    private void ChangeLevel(int layer)
    {
        this.SelectLayer = layer;
        this.Show();
    }

    private void Show()
    {
        foreach (var item in items)
        {
            item.gameObject.SetActive(false);
        }

        User user = GameProcessor.Inst.User;
        bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        int goupId = (user.MapId - ConfigHelper.MapStartId) / 6 + 1;

        int layer = goupId / 12;
        this.MaxLayer = layer;

        if (this.SelectLayer < 0)
        {
            this.SelectLayer = Math.Min(this.MaxLayer, tgLevelList.Count - 1);
            tgLevelList[SelectLayer].isOn = true;
        }

        for (int i = 0; i < tgLevelList.Count; i++)
        {
            if (i <= MaxLayer)
            {
                tgLevelList[i].gameObject.SetActive(true);
            }
            else
            {
                tgLevelList[i].gameObject.SetActive(false);
            }
        }

        int count = MapGroupConfigCategory.Instance.GetAll().Where(m => m.Value.Id <= goupId).Count();

        int startIndex = this.SelectLayer * LevelCount;
        int endIndex = startIndex + Math.Min(LevelCount, count - startIndex) - 1;

        Debug.Log("goupId:" + goupId + "SelectLayer: " + SelectLayer);
        Debug.Log("startIndex:" + startIndex + "endIndex: " + endIndex);

        for (int i = 0; i < items.Count; i++)
        {
            if (i >= startIndex && i <= endIndex)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
