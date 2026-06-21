using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;
using Newtonsoft.Json;

namespace Game
{
    public class Detail_Normal : MonoBehaviour, IBattleLife
    {
        public Text Txt_Name;

        public Text Txt_Require;
        public Text Txt_Des;

        public Button Btn_Recovery;
        public Button Btn_Recovery_All;
        public Button Btn_Lose;

        public Button Btn_Use;
        public Button Btn_Use_Batch;
        public Button Btn_UseAll;

        public Button Btn_Close;

        private BoxItem Show_Box;
        private ComBoxType BoxType;

        public Transform tf_Count;
        public InputField if_Count;
        public Button Btn_Confirm;
        public Button Btn_Cancle;

        private int Type = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.Btn_Recovery_All.onClick.AddListener(this.OnRecoveryAll);
            this.Btn_Lose.onClick.AddListener(this.OnLose);
            this.Btn_Use.onClick.AddListener(this.OnUse);
            this.Btn_Use_Batch.onClick.AddListener(this.OnUseBatch);
            this.Btn_UseAll.onClick.AddListener(this.OnUseAll);
            this.Btn_Confirm.onClick.AddListener(this.OnConfirm);
            this.Btn_Cancle.onClick.AddListener(this.OnCancle);

            this.Btn_Close.onClick.AddListener(this.OnClick_Close);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEvent>(this.OnShow);
        }

        private void OnShow(ShowDetailEvent e)
        {
            if (e.Show_Type != ShowType.Normal)
            {
                return;
            }

            this.gameObject.SetActive(true);

            this.Btn_Recovery.gameObject.SetActive(false);
            this.Btn_Recovery_All.gameObject.SetActive(false);
            this.Btn_Lose.gameObject.SetActive(false);
            this.Btn_Use.gameObject.SetActive(false);
            this.Btn_UseAll.gameObject.SetActive(false);
            this.Btn_Use_Batch.gameObject.SetActive(false);

            this.tf_Count.gameObject.SetActive(false);
            this.if_Count.text = "";


            this.Show_Box = e.Show_Item;
            this.BoxType = e.Box_Type;

            Item item = Show_Box.Item;

            var titleColor = QualityConfigHelper.GetQualityColor(item.GetQuality());
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", titleColor, item.GetName());

            long number = Show_Box.MagicNubmer.Data;
            int requireLevel = item.GetRequired();
            int configId = item.ConfigId;

            User user = User_Data_Manager.Data;
            string color = user.MagicLevel.Data < requireLevel ? "red" : "green";

            Txt_Des.text = item.GetDes();
            this.Txt_Require.text = string.Format("<color={0}>需要等级{1}</color>", color, requireLevel);

            if (e.Box_Type == ComBoxType.Bag)
            {
                if (item.GetItemType() != ItemType.GiftPack)
                {
                    ItemConfig config = ItemConfigCategory.Instance.Get(configId);
                    if (config.RecoveryItemId > 0)
                    {
                        this.Btn_Recovery.gameObject.SetActive(true);
                        this.Btn_Recovery_All.gameObject.SetActive(true);
                        this.Btn_Lose.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.Btn_Recovery.gameObject.SetActive(false);
                        this.Btn_Recovery_All.gameObject.SetActive(false);
                        this.Btn_Lose.gameObject.SetActive(true);
                    }
                }

                switch (item.GetItemType())
                {
                    case ItemType.SkillBox://技能书
                        {
                            var isLearn = user.SkillList.Find(b => b.SkillId == configId) == null;

                            this.Btn_Use.gameObject.SetActive(true);
                            this.Btn_Use_Batch.gameObject.SetActive(!isLearn);
                            this.Btn_UseAll.gameObject.SetActive(!isLearn);
                        }
                        break;
                    case ItemType.GiftPack:
                        {
                            this.Btn_Use.gameObject.SetActive(true);

                            GiftPackConfig giftPackConfig = GiftPackConfigCategory.Instance.Get(configId);
                            if (giftPackConfig.OpenType == 1)
                            {
                                this.Btn_UseAll.gameObject.SetActive(true);
                            }
                            else
                            {
                                this.Btn_UseAll.gameObject.SetActive(false);
                            }
                        }
                        break;
                    case ItemType.Ticket:
                    case ItemType.Material_Usable:
                        {
                            this.Btn_Use.gameObject.SetActive(true);
                            if (number > 1)
                            {
                                this.Btn_Use_Batch.gameObject.SetActive(true);
                                this.Btn_UseAll.gameObject.SetActive(true);
                            }
                        }
                        break;
                    default:
                        {

                        }
                        break;
                }

            }
        }

        private void OnRecovery()
        {
            if (this.Show_Box.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能回收", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.tf_Count.gameObject.SetActive(true);
            this.Type = 1;

            long count = this.Show_Box.MagicNubmer.Data;
            if_Count.placeholder.GetComponent<Text>().text = "最大输入" + count;
        }

        private void OnRecoveryAll()
        {
            this.gameObject.SetActive(false);
            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                Quantity = -1,
                BoxItem = this.Show_Box,
            });
        }

        private void OnLose()
        {
            if (this.Show_Box.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能丢弃", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (this.Btn_Recovery.IsActive())
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "能回收的不能丢弃", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("不会有任何收益,是否确认丢弃道具？", true, () =>
            {
                GameProcessor.Inst.EventCenter.Raise(new LoseEvent()
                {
                    BoxItem = this.Show_Box,
                });
            }, null);
        }

        private void OnUse()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Number = 1,
                BoxItem = this.Show_Box
            });
        }

        private void OnUseAll()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
            {
                Number = -1,
                BoxItem = this.Show_Box
            });
        }

        private void OnUseBatch()
        {
            this.tf_Count.gameObject.SetActive(true);
            this.Type = 2;

            long count = this.Show_Box.MagicNubmer.Data;
            if_Count.placeholder.GetComponent<Text>().text = "最大输入" + count;
        }

        private void OnConfirm()
        {
            this.tf_Count.gameObject.SetActive(false);
            this.gameObject.SetActive(false);

            int.TryParse(if_Count.text, out int quantity);

            long count = this.Show_Box.MagicNubmer.Data;
            if (quantity > count)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "数量超出了最大值", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (quantity > 0)
            {
                if (this.Type == 1)
                {
                    GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                    {
                        Quantity = quantity,
                        BoxItem = this.Show_Box,
                    });
                }
                else if (Type == 2)
                {

                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                    {
                        Number = quantity,
                        BoxItem = this.Show_Box
                    });
                }
            }
        }

        private void OnCancle()
        {
            this.tf_Count.gameObject.SetActive(false);
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}
