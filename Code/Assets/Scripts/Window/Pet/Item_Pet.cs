using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public class Item_Pet : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Layer;

        public Button Btn_Down;
        public Button Btn_Run;
        public Button Btn_Stop;

        public Button Btn_Image;
        public Image Img_Logo;

        public Pet pet;

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

        }

        private void ShowDetail()
        {
            BoxItem box = new BoxItem();
            box.Item = pet;
            box.BoxId = -1;

            GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
            {
                Show_Item = box,
                Box_Type = ComBoxType.PreView,
                Show_Type = pet.GetShowType(),
                Position = -1,
            });
        }

        private void OnDown()
        {
            if (this.pet.Status == 1)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "出战中，不能下阵", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new PetBattleDownEvent()
            {
                Item = this
            });
        }

        private void OnRun()
        {
            Btn_Run.gameObject.SetActive(false);

            User user = User_Data_Manager.Data;

            int maxCount = (int)user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.PetBattleLimit) + 1;
            int count = user.PetList.Where(m => m.Status == 1).Count();

            if (count >= maxCount)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经出战上限了", ToastType = ToastTypeEnum.Failure });
                return;
            }

            pet.Status = 1;
            Txt_Name.text = pet.Status == 1 ? "出战中" : "备战中";
            Btn_Stop.gameObject.SetActive(true);
        }

        private void OnStop()
        {
            Btn_Stop.gameObject.SetActive(false);

            pet.Status = 0;
            Txt_Name.text = pet.Status == 1 ? "出战中" : "备战中";
            Btn_Run.gameObject.SetActive(true);
        }

        public void Init(Pet pet)
        {
            this.pet = pet;

            if (pet.Status == 0)
            {
                Btn_Run.gameObject.SetActive(true);
                Btn_Stop.gameObject.SetActive(false);
            }
            else
            {
                Btn_Run.gameObject.SetActive(false);
                Btn_Stop.gameObject.SetActive(true);
            }

            Txt_Name.text = pet.Status == 1 ? "出战中" : "备战中";
            Txt_Level.text = pet.PetLevel.Data + "级";
            Txt_Layer.text = pet.PetLayer.Data + "阶";

            Txt_Level.color = ColorHelper.GetColorByQuality(pet.GetQuality());
            Txt_Layer.color = ColorHelper.GetColorByQuality(pet.GetQuality());

            this.Img_Logo.sprite = PrefabHelper.Instance().GetMonster(pet.ConfigId);
        }
    }
}