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
            item.Show();
        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
