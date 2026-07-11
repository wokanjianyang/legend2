using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Data;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Pet_Forge : MonoBehaviour
{
    public ScrollRect sr_Main;
    public ScrollRect sr_Bag;

    private List<Pet_Forge_Box> mainList = new List<Pet_Forge_Box>();

    private List<Pet_Forge_Box> bagList = new List<Pet_Forge_Box>();

    public Text Txt_Name_Main;
    public Text Txt_Kill_Main;
    public Text Txt_Exp_Main;

    public Text Txt_Name_Bag;
    public Text Txt_Kill_Bag;
    public Text Txt_Exp_Bag;

    public Button Btn_OK;

    public int Order => (int)ComponentOrder.Dialog;

    private const int MaxPet = 4;
    private const int MaxMaterial = 42;

    private int SelectMainIndex = -1;
    private int SelectBagIndex = -1;

    private int BasePercent = 80;

    private void Awake()
    {
        Btn_OK.onClick.AddListener(OnClick_Ok);

        this.Init();
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable");

        this.Show();
    }

    private void Init()
    {
        var emptyPrefab = PrefabHelper.Instance().ComBoxEmpty;

        for (int i = 0; i < MaxPet; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Main.content);
            empty.name = "Main_" + i;
        }

        for (int i = 0; i < MaxMaterial; i++)
        {
            var empty = GameObject.Instantiate(emptyPrefab, this.sr_Bag.content);
            empty.name = "Box_" + i;
            //yield return null;
        }
    }


    private void Show()
    {
        User user = User_Data_Manager.Data;
        if (user == null)
        {
            return;
        }

        this.SelectMainIndex = -1;
        this.SelectBagIndex = -1;

        foreach (var sp in mainList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        mainList.Clear();

        foreach (var sp in bagList)
        {
            GameObject.Destroy(sp.gameObject);
        }
        bagList.Clear();

        Txt_Name_Main.text = "Î´Ñ¡Ôñ";
        Txt_Kill_Main.text = "Î´Ñ¡Ôñ";
        Txt_Exp_Main.text = "Î´Ñ¡Ôñ";

        Txt_Name_Bag.text = "Î´Ñ¡Ôñ";
        Txt_Kill_Bag.text = "Î´Ñ¡Ôñ";
        Txt_Exp_Bag.text = "Î´Ñ¡Ôñ";


        ToggleGroup tgMain = sr_Main.GetComponent<ToggleGroup>();

        for (int i = 0; i < user.PetList.Count; i++)
        {
            var bagBox = this.sr_Main.content.GetChild(i);
            if (bagBox == null)
            {
                continue;
            }

            Pet pet = user.PetList[i];

            Pet_Forge_Box box = PrefabHelper.Instance().CreateBoxSelect(bagBox);
            box.SetItem(pet);
            box.Init(1, i, tgMain);
            this.mainList.Add(box);
        }

        List<BoxItem> list = user.Bags.Where(m => m.Item.GetItemType() == ItemType.Pet).ToList();

        ToggleGroup tgBag = sr_Bag.GetComponent<ToggleGroup>();

        int BoxId = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (BoxId >= MaxMaterial)
            {
                return;
            }

            Pet pet = list[i].Item as Pet;
            if (pet.KillCount.Data <= 0 && pet.LevelExp.Data <= 0 && pet.PetLevel.Data <= 1)
            {
                continue;
            }

            var bagBox = this.sr_Bag.content.GetChild(BoxId);

            Pet_Forge_Box box = PrefabHelper.Instance().CreateBoxSelect(bagBox);
            box.SetItem(pet);
            box.Init(2, BoxId, tgBag);
            this.bagList.Add(box);

            BoxId++;
        }
    }

    private void ShowInfo()
    {
        if (SelectMainIndex >= 0)
        {
            Pet_Forge_Box fm = mainList[SelectMainIndex];

            Pet pet = fm.CurrentItem;

            Txt_Name_Main.text = pet.GetName();
            Txt_Kill_Main.text = "É±µÐÊý£º" + pet.GetTotalKillCount() + "";
            Txt_Exp_Main.text = "µÈ¼¶£º" + pet.Level + "";
        }

        if (SelectBagIndex >= 0)
        {
            Pet_Forge_Box bm = bagList[SelectBagIndex];

            Pet pet = bm.CurrentItem;

            int bp = GetBasePercetn();

            Txt_Name_Bag.text = pet.GetName();
            Txt_Kill_Bag.text = "+" + pet.GetTotalKillCount() + "*" + bp + "% É±µÐ";
            Txt_Exp_Bag.text = "+" + pet.GetTotalExp() + "*" + bp + "% Exp";
        }

        if (SelectMainIndex >= 0 && SelectBagIndex >= 0)
        {
            Btn_OK.gameObject.SetActive(true);
        }
        else
        {
            Btn_OK.gameObject.SetActive(false);
        }
    }

    public void SelectItem(int type, int p)
    {
        if (type == 1)
        {
            this.SelectMainIndex = p;
        }
        else if (type == 2)
        {
            this.SelectBagIndex = p;
        }

        this.ShowInfo();
    }

    private int GetBasePercetn()
    {
        User user = User_Data_Manager.Data;
        int bp = BasePercent + (int)user.AttributeBonus.CalPanelAtr(AttributeEnum.PetInherit);

        return Math.Min(bp, 100);
    }

    public void OnClick_Ok()
    {
        this.Btn_OK.gameObject.SetActive(false);

        Pet_Forge_Box fm = mainList[SelectMainIndex];
        Pet petMain = fm.CurrentItem;

        Pet_Forge_Box bm = bagList[SelectBagIndex];

        Pet petBag = bm.CurrentItem;

        petBag.IsDelete = true;

        //Ïú»Ù
        GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent() { });

        int bp = GetBasePercetn();

        long bk = (long)(petBag.KillCount.Data * bp / 100);
        petMain.AddKillCount(bk);

        long total = petBag.GetTotalExp();
        long bt = total * bp / 100;
        petMain.AddExp(bt);

        this.Show();
    }
}
