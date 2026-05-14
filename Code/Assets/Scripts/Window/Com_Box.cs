using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Com_Box : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        [Title("物品格")]
        [LabelText("道具名")]
        public Text Txt_Name;

        [LabelText("数量")]
        public Text Txt_Count;

        public Image Img_Lock;
        public Image Img_Tag;
        public Image Img_Bg;
        public Image Img_Logo;

        //public Text Tag;
        public Text Txt_Layer;
        public TMP_Text Tmp_Name;

        public BoxItem BoxItem { get; private set; }
        public int boxId { get; private set; }

        public int BagType { get; private set; }

        public int EquipPosition { get; private set; }
        public long Count { get; private set; }

        public ComBoxType Type { get; set; } = ComBoxType.Bag;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {
            this.ShowName();
        }

        private void ShowName()
        {
            if (this.BoxItem != null)
            {
                if (BoxItem.Item.GetItemType() == ItemType.Equip)
                {
                    Equip equip = BoxItem.Item as Equip;
                    if (equip.GetQuality() > 5 && (equip.Part <= 10 || equip.Part >= 21))
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[equip.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }
                }
                else if (BoxItem.Item.GetItemType() == ItemType.EquipSpeical)
                {
                    Equip_Special item = BoxItem.Item as Equip_Special;

                    if (item.Level > 0)
                    {
                        this.Txt_Count.text = item.Level + "级";
                        this.Txt_Count.gameObject.SetActive(true);
                    }
                    if (item.Layer > 0)
                    {
                        this.Txt_Layer.text = ConfigHelper.LayerChinaList[item.Layer] + "阶";
                        this.Txt_Layer.gameObject.SetActive(true);
                    }

                }
                //else if (BoxItem.Item.GetItemType() == ItemType.Shengxiao)
                //{
                //    Shengxiao item = BoxItem.Item as Shengxiao;

                //    if (item.LevelData.Data > 0)
                //    {
                //        this.Txt_Count.text = item.LevelData.Data + "级";
                //        this.Txt_Count.gameObject.SetActive(true);
                //    }
                //    if (item.LayerData.Data > 0)
                //    {
                //        this.Txt_Layer.text = ConfigHelper.LayerChinaList[item.LayerData.Data] + "阶";
                //        this.Txt_Layer.gameObject.SetActive(true);
                //    }

                //}
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.BoxItem == null) return;

            this.BoxItem.Item.IsNew = false;
            this.Img_Tag.gameObject.SetActive(false);


            GameProcessor.Inst.EventCenter.Raise(new ShowDetailEvent()
            {
                Show_Item = this.BoxItem,
                Box_Type = this.Type,
                Show_Type = this.BoxItem.Item.GetShowType(),
                Position = this.EquipPosition,
            });
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }
        public void SetItem(BoxItem item)
        {
            this.Img_Lock.gameObject.SetActive(false);
            this.Img_Tag.gameObject.SetActive(false);
            this.Txt_Count.gameObject.SetActive(false);
            this.Txt_Layer.gameObject.SetActive(false);

            int quality = item.Item.GetQuality();

            this.Txt_Name.text = item.Item.GetName();
            if (Tmp_Name != null)
            {
                this.Tmp_Name.text = item.Item.GetName();
                this.Tmp_Name.faceColor = QualityConfigHelper.GetColor(quality);
            }

            this.BoxItem = item;

            this.Count = item.MagicNubmer.Data;
            this.BagType = item.GetBagType();

            this.Txt_Name.color = QualityConfigHelper.GetColor(quality);
            this.Img_Bg.sprite = PrefabHelper.Instance().GetBoxImage(quality);

            this.Img_Lock.gameObject.SetActive(item.Item.IsLock);

            this.Txt_Count.transform.gameObject.SetActive(this.Count > 1);
            if (this.Count > 999999999)
            {
                this.Txt_Count.text = StringHelper.FormatNumber(this.Count);
            }
            else
            {
                this.Txt_Count.text = this.Count.ToString();
            }

            if (item.Item.GetItemType() == ItemType.Equip)
            {
                Equip equip = item.Item as Equip;

                this.Img_Logo.sprite = PrefabHelper.Instance().GetEquipLog(equip.Config.Role, equip.Config.Part);
            }
            else if (item.Item.GetItemType() == ItemType.EquipSpeical)
            {
                Equip_Special equip = item.Item as Equip_Special;

                this.Img_Logo.sprite = PrefabHelper.Instance().GetEquipLog(0, equip.Config.Part);
            }

            if (item.Item.IsNew && (item.Item.GetItemType() == ItemType.Equip || item.Item.GetItemType() == ItemType.Exclusive))
            {
                if (item.Item.GetItemType() == ItemType.Equip)
                {
                    Equip equip = item.Item as Equip;
                    if (equip.Part > 10 && equip.Part < 20)
                    {
                        item.Item.IsNew = false;
                        return;
                    }
                }

                this.Img_Tag.gameObject.SetActive(true);
            }

            this.ShowName();
        }

        public void SetBoxId(int id)
        {
            this.boxId = id;
        }

        public void SetEquipPosition(int position)
        {
            this.EquipPosition = position;

            if (position > 0)
            {
                this.Type = ComBoxType.OnEquip;
            }
            else
            {
                this.Type = ComBoxType.Bag;
            }
        }

        public void AddStack(long quantity)
        {
            this.Count += quantity;
            this.Txt_Count.transform.gameObject.SetActive(this.Count != 1);

            if (this.Count > 999999999)
            {
                this.Txt_Count.text = StringHelper.FormatNumber(this.Count);
            }
            else
            {
                this.Txt_Count.text = this.Count.ToString();
            }

        }

        public void RemoveStack(long quantity)
        {
            this.Count -= quantity;
            this.Txt_Count.transform.gameObject.SetActive(this.Count != 1);
            if (this.Count > 999999999)
            {
                this.Txt_Count.text = StringHelper.FormatNumber(this.Count);
            }
            else
            {
                this.Txt_Count.text = this.Count.ToString();
            }
        }

        public void SetLock(bool isLock)
        {
            this.Img_Lock.gameObject.SetActive(isLock);
        }

        public void SetType(ComBoxType type)
        {
            this.Type = type;
        }
    }

    public enum ComBoxType
    {
        Bag = 1,
        OnEquip = 2,
        PreView = 3,
    }
}