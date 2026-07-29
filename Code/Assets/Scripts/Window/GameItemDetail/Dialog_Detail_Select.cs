using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Detail_Select : MonoBehaviour, IBattleLife
    {
        public Button Btn_Close;
        public Button Btn_OK;
        public Button Btn_Query;
        public Button Btn_OK_All;

        public RectTransform Container;
        public ToggleGroup toggleGroup;

        private int ConfigId;
        private BoxItem FromItem;

        private List<Gift_Item> ItemList = new List<Gift_Item>();

        private BoxItem SelectItem;

        public int Order => (int)ComponentOrder.Dialog;

        void Start()
        {
            Btn_Close.onClick.AddListener(OnClick_Close);
            Btn_OK.onClick.AddListener(OnClick_OK);
            Btn_OK_All.onClick.AddListener(OnClick_OK_All);
            Btn_Query.onClick.AddListener(OnClick_Query);
        }


        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEvent>(this.OnShow);
        }

        private void Init()
        {
            //clear
            foreach (var si in ItemList)
            {
                GameObject.Destroy(si.gameObject);
            }
            ItemList.Clear();

            GiftPackConfig config = GiftPackConfigCategory.Instance.Get(this.ConfigId);
            var pref = Resources.Load<GameObject>("Prefab/GameItem/Gift_Item");

            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                var itemUI = GameObject.Instantiate(pref, Container);
                itemUI.transform.localScale = Vector3.one;

                Gift_Item item = itemUI.GetComponent<Gift_Item>();

                Item newItem = ItemHelper.BuildItemByGift((ItemType)config.ItemTypeList[i], config.ItemIdList[i], 1, config.ItemCountList[i]);
                BoxItem newBox = new BoxItem();
                newBox.Item = newItem;
                newBox.MagicNubmer.Data = 1;
                newBox.BoxId = -1;

                item.SetItem(newBox, toggleGroup);

                ItemList.Add(item);
            }

            //Debug.Log(config.Id + " " + config.Name + "" + config.OpenType);
            if (config.OpenType == 1)
            {
                this.Btn_OK_All.gameObject.SetActive(true);
            }
            else
            {
                this.Btn_OK_All.gameObject.SetActive(false);
            }
        }

        public void OnShow(ShowDetailEvent e)
        {
            if (e.Show_Type != ShowType.Select)
            {
                return;
            }

            this.FromItem = e.Show_Item;
            this.ConfigId = this.FromItem.Item.ConfigId;
            this.Init();
            this.gameObject.SetActive(true);
        }

        public void OnSelectItem(BoxItem item)
        {
            this.SelectItem = item;
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_OK()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();

            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //判断空格
            int ic = User_Data_Manager.Data.GetBagIdleCount(SelectItem.GetBagType());
            if (ic < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
                return;
            }


            this.gameObject.SetActive(false);

            //选择第N个装备
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = FromItem,
                Item = SelectItem.Item,
                Nubmer = 1
            });
        }


        public void OnClick_OK_All()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();

            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //判断空格
            int ic = User_Data_Manager.Data.GetBagIdleCount(SelectItem.GetBagType());
            if (ic < 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留10个对应的包裹格子", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            //选择第N个装备
            GameProcessor.Inst.EventCenter.Raise(new SelectGiftEvent()
            {
                BoxItem = FromItem,
                Item = SelectItem.Item,
                Nubmer = FromItem.MagicNubmer.Data
            });
        }

        public void OnClick_Query()
        {
            Gift_Item select = ItemList.Where(m => m.toggle.isOn).FirstOrDefault();
            if (select == null)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请先选择一个道具", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
            {
                Show_Item = SelectItem,
                Show_Type = SelectItem.Item.GetShowType(),
                Box_Type = ComBoxType.PreView,
                Position = -1,
            });
        }
    }
}
