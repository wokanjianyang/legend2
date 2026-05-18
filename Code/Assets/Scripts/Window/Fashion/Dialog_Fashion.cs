using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Fashion : MonoBehaviour
{
    public Button Btn_Close;

    public Transform Tf_Nav;
    private List<Toggle> toggles;

    public ScrollRect sr_Panel;

    private List<Item_Fashion> Items = new List<Item_Fashion>();

    private GameObject ItemPrefab;

    private int Cycle = 1;

    public int Order => (int)ComponentOrder.Dialog;

    private void Awake()
    {
        toggles = Tf_Nav.GetComponentsInChildren<Toggle>().ToList();

        Btn_Close.onClick.AddListener(OnClick_Close);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener((isOn) =>
            {
                ChangePanel(index);
            });
        }

        this.ChangePanel(0);


        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Fashion/Item_Fashion");

        Init();
    }

    private void Init()
    {
        User user = GameProcessor.Inst.User;

        List<FashionConfig> configs = FashionConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        foreach (FashionConfig config in configs)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            item.transform.SetParent(this.sr_Panel.content);
            item.transform.localScale = Vector3.one;

            var com = item.GetComponentInChildren<Item_Fashion>();
            com.SetItem(config);

            Items.Add(com);
        }
    }


    public void Show()
    {

    }

    private void ChangePanel(int cycle)
    {
        this.Cycle = cycle;

        this.Show();
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
