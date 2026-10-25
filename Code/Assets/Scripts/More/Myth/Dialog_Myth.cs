using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Myth : MonoBehaviour
{
    public Button btn_FullScreen;

    public Transform Tf_Toggles;
    private List<Toggle> toggles;

    public ScrollRect sr_Boss;
    private List<Item_Myth> ItemList = new List<Item_Myth>();

    private int SelectLayer = 1;

    public Toggle toggle_Auto;
    // Start is called before the first frame update
    void Awake()
    {
        btn_FullScreen.onClick.AddListener(this.OnClick_Close);

        toggles = Tf_Toggles.GetComponentsInChildren<Toggle>().ToList();

        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i + 1;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                this.ChangePanel(index);
            });
        }

        //this.Txt_Count.gameObject.SetActive(false);
    }

    void Start()
    {
        toggle_Auto.onValueChanged.AddListener((isOn) =>
        {
            AppHelper.SetData.Myth_Auto  = isOn;
            User_Data_Manager.SettingSave();
        });

        this.Init();
        //this.ChangePanel(1);
    }

    private void OnEnable()
    {
        this.ShowItemMax();
    }

    private void ShowItemMax()
    {
        User user = User_Data_Manager.Data;

        if (user == null)
        {
            return;
        }

        int max = user.MythData.GetMax();

        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i].SetMax(SelectLayer, max);
        }
    }

    private void ChangePanel(int layer)
    {
        this.SelectLayer = layer;
        this.ShowItemMax();
    }

    private void Init()
    {
        this.gameObject.SetActive(true);

        User user = User_Data_Manager.Data;
        user.MaterailData.Check();

        GameObject ItemPrefab = Resources.Load<GameObject>("Prefab/More/Myth/Item_Myth");

        List<MythConfig> configs = MythConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Myth>();

            com.SetContent(configs[i]);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            ItemList.Add(com);
        }

        this.ShowItemMax();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
