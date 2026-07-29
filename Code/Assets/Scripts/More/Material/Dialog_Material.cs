using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Material : MonoBehaviour
{
    public Button btn_FullScreen;

    public Transform Tf_Toggles;
    private List<Toggle> toggles;

    public ScrollRect sr_Boss;
    private List<Item_Material> ItemList = new List<Item_Material>();

    public Text Txt_Count;

    string[] names = { "金币副本", "强化副本", "精炼副本", };

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
        this.Init();
        //this.ChangePanel(1);
    }


    private void ChangePanel(int type)
    {
        User user = User_Data_Manager.Data;

        Materail_Record record = user.MaterailData.GetRecordType(type);
       //this.Txt_Count.text = names[type - 1] + "剩余挑战次数：" + record.Count;

        foreach (var sp in ItemList)
        {
            sp.ShowType(type);
        }
    }

    private void Init()
    {
        this.gameObject.SetActive(true);

        User user = User_Data_Manager.Data;
        user.MaterailData.Check();

        GameObject ItemPrefab = Resources.Load<GameObject>("Prefab/More/Material/Item_Material");

        for (int i = 1; i <= 3; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            var com = item.GetComponentInChildren<Item_Material>();

            com.SetContent(i);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            ItemList.Add(com);
        }

    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
