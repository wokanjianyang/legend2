using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public class Item_Pet_New : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Exp;
        public Text Txt_Kill;

        public Button Btn_Down;
        public Button Btn_Run;
        public Button Btn_Stop;

        public Button Btn_Image;
        public Image Img_Logo;
        public Text Txt_Logo;

        private int Position = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Image.onClick.AddListener(ShowDetail);
            this.Btn_Down.onClick.AddListener(OnDown);
            this.Btn_Run.onClick.AddListener(OnRun);
            this.Btn_Stop.onClick.AddListener(OnStop);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            this.Show();
        }

        private void ShowDetail()
        {
            Pet pet = GetCurrentPet();

            if (pet == null)
            {
                return;
            }

            BoxItem box = new BoxItem();
            box.Item = pet;
            box.BoxId = -1;

            GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
            {
                Show_Item = box,
                Box_Type = ComBoxType.PreView,
                Show_Type = pet.GetShowType(),
                Position = Position,
            });
        }

        private void OnDown()
        {
            this.Btn_Down.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;
            Dictionary<int, Pet> dict = user.GetCurrentPetList();

            if (!dict.ContainsKey(Position))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "还没有宠物", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //判断空格
            int ic = User_Data_Manager.Data.GetBagIdleCount(3);
            if (ic < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
                return;
            }

            Pet pet = dict[Position];

            dict.Remove(Position);

            List<Item> items = new List<Item>();
            items.Add(pet);
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            this.Show();

            //更新属性面板
            GameProcessor.Inst.UpdateInfo();

            //更新技能描述
            GameProcessor.Inst.EventCenter.Raise(new SkillShowEvent());
        }

        private void OnRun()
        {
            Btn_Run.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;

            foreach (var sp in user.PetData)
            {
                sp.Status = 0;
            }

            Pet_Data data = user.PetData[Position - 1];
            data.Status = 1;

            Panel_Pet_New panel = this.GetComponentInParent<Panel_Pet_New>();
            panel.Refresh();
        }

        private void OnStop()
        {
            Btn_Stop.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;

            Pet_Data data = user.PetData[Position - 1];
            data.Status = 0;

            Panel_Pet_New panel = this.GetComponentInParent<Panel_Pet_New>();
            panel.Refresh();
        }

        public void Init(int p)
        {
            this.Position = p;
            this.Show();
        }

        public void Show()
        {
            if (Position <= 0)
            {
                return;
            }

            User user = User_Data_Manager.Data;

            Pet_Data data = user.PetData[Position - 1];

            if (data.Status == 0)
            {
                Btn_Run.gameObject.SetActive(true);
                Btn_Stop.gameObject.SetActive(false);
                this.Txt_Name.text = "";
            }
            else
            {
                Btn_Run.gameObject.SetActive(false);
                Btn_Stop.gameObject.SetActive(true);
                this.Txt_Name.text = "出战中";
            }

            Pet pet = GetCurrentPet();

            if (pet == null)
            {
                this.Img_Logo.gameObject.SetActive(false);
                this.Txt_Logo.gameObject.SetActive(true);
                this.Btn_Down.gameObject.SetActive(false);
                //this.Txt_Name.text = "";
            }
            else
            {
                this.Img_Logo.gameObject.SetActive(true);
                this.Txt_Logo.gameObject.SetActive(false);
                this.Btn_Down.gameObject.SetActive(true);

                //this.Txt_Name.text = pet.GetName();
                this.Img_Logo.sprite = PrefabHelper.Instance().GetMonster(pet.ConfigId);
            }


            this.Txt_Level.text = "等级：Lv" + data.GetLevel();
            this.Txt_Exp.text = "经验：" + data.GetExp();
            this.Txt_Kill.text = "杀敌数：" + (long)data.Kill.Data;

        }

        public Pet GetCurrentPet()
        {
            User user = User_Data_Manager.Data;
            Dictionary<int, Pet> dict = user.GetCurrentPetList();

            if (!dict.ContainsKey(Position))
            {
                return null;
            }

            return dict[Position];
        }
    }
}