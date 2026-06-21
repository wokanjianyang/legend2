using Game;
using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Dialog_Task : MonoBehaviour
{
    public Button Btn_Close;

    public ScrollRect sr_Left;
    public ScrollRect sr_Right;

    private List<Task_Group> list = new List<Task_Group>();
    private List<Task_Item> items = new List<Task_Item>();

    public Transform Tf_Complete;

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


    private void Init()
    {
        GroupPrefab = Resources.Load<GameObject>("Prefab/Window/Achievement/Task_Group");
        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Achievement/Task_Item");

        List<AchievementTaskGroupConfig> configs = AchievementTaskGroupConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (AchievementTaskGroupConfig config in configs)
        {
            BuildGroupItem(config);
        }

        this.SelectItem(1);
    }

    private void BuildGroupItem(AchievementTaskGroupConfig config)
    {
        var item = GameObject.Instantiate(GroupPrefab);
        var com = item.GetComponent<Task_Group>();

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

        User user = User_Data_Manager.Data;

        AchievementTaskConfig config = AchievementTaskConfigCategory.Instance.GetCurrent(gid, user.TaskLog);

        if (config != null)
        {
            Tf_Complete.gameObject.SetActive(false);
            BuildItem(config);
        }
        else
        {
            Tf_Complete.gameObject.SetActive(true);
        }
    }

    private void BuildItem(AchievementTaskConfig config)
    {
        var item = GameObject.Instantiate(ItemPrefab);
        var com = item.GetComponent<Task_Item>();

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

