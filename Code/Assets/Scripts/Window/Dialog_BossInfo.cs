using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_BossInfo : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button Btn_Close;

    public Toggle toggle_Hide;

    public Transform Tf_Layer;

    private List<Toggle> tgLevelList;
    private int LevelCount = 35; //每个难度多少个
    private int ShowCount = 10; //隐藏的时候显示多少个
    private int MaxCycle = 5; //现在多少个难度-1

    private int MaxLayer = -1;
    private int SelectLayer = -1;

    List<Item_MainMap> items = new List<Item_MainMap>();

    private void Awake()
    {
        Btn_Close.onClick.AddListener(OnClick_Close);

        toggle_Hide.onValueChanged.AddListener((isOn) =>
        {
            this.Show();
        });

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

    }

    void Update()
    {

    }

    private void Init()
    {
        ItemPrefab = Resources.Load<GameObject>("Prefab/Map/Item_MainMap");

        List<MapConfig> list = MapConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (MapConfig config in list)
        {
            BuildItem(config);
        }
    }

    private void BuildItem(MapConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Item_MainMap>();

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
        //bool ac = ConfigHelper.AC == ConfigHelper.Channel_Tap || user.Account == "";

        int MapId = user.MapId;
        int layer = (MapId - ConfigHelper.MapStartId) / 35;
        //layer = ac ? Math.Min(3, layer) : layer;
        this.MaxLayer = layer;

        if (this.SelectLayer < 0)
        {
            this.SelectLayer = Math.Min(this.MaxLayer, MaxCycle);
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

        int count = MapConfigCategory.Instance.GetAll().Where(m => m.Value.Id <= MapId).Count();

        int startIndex = this.SelectLayer * LevelCount;
        int endIndex = startIndex + Math.Min(LevelCount, count - startIndex) - 1;

        int j = 0;
        for (int i = endIndex; i >= startIndex; i--)
        {
            if (j < ShowCount)
            {
                items[i].gameObject.SetActive(true);
            }
            else
            {
                items[i].gameObject.SetActive(!toggle_Hide.isOn);
            }
            j++;
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
