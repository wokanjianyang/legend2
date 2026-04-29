using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Detail_Equip_Special : MonoBehaviour, IBattleLife
    {
        [LabelText("名称")]
        public Text Txt_Name;

        public Text Txt_Require;

        [LabelText("基础属性")]
        public Transform Tf_Base;

        [LabelText("随机属性")]
        public Transform Tf_Random;

        [LabelText("套装属性")]
        public Transform Tf_Set;

        [Title("导航")]

        public Button btn_Equip;
        public Button btn_UnEquip;

        public Button btn_Recovery;
        public Button btn_Restore;

        public Button btn_Lock;
        public Button btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;
        private int equipPositioin;
        private ComBoxType BoxType;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.btn_Restore.onClick.AddListener(this.OnClick_Restore);

            this.btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.btn_Unlock.onClick.AddListener(this.OnClick_Unlock);

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

            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEquipSpecialEvent>(this.OnShowEvent);
        }

        private void OnShowEvent(ShowDetailEquipSpecialEvent e)
        {
            this.gameObject.SetActive(true);
            Tf_Base.gameObject.SetActive(false);
            Tf_Random.gameObject.SetActive(false);
            Tf_Set.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Restore.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            this.boxItem = e.boxItem;
            this.equipPositioin = e.EquipPosition;
            this.BoxType = e.Type;

            var titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);

            Equip_Special equip = this.boxItem.Item as Equip_Special;

            EquipSpeicalConfig config = equip.Config;

            string name = equip.GetName();
            name += "(" + ConfigHelper.LayerChinaList[equip.Layer] + "阶)";

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", titleColor, name);

            User user = GameProcessor.Inst.User;

            string color = user.MagicLevel.Data >= config.LevelRequired ? "green" : "red";
            this.Txt_Require.text = string.Format("<color={0}>需要等级{1}</color>", color, config.LevelRequired);


            IDictionary<int, double> BaseAttrList = equip.GetBaseAttrList();

            if (BaseAttrList != null && BaseAttrList.Count > 0)
            {
                Tf_Base.gameObject.SetActive(true);

                Transform gridBase = Tf_Base.Find("Grid_Base");

                List<KeyValuePair<int, double>> btList = BaseAttrList.ToList();

                for (int index = 0; index < 8; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));

                    if (index < btList.Count())
                    {
                        child.GetComponent<Text>().text = FormatAttrText(btList[index].Key, btList[index].Value, 0);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (equip.AttrEntryList != null && equip.AttrEntryList.Count > 0)
            {
                Tf_Random.gameObject.SetActive(true);
                Transform gridRandom = Tf_Random.Find("Grid_Random");

                var AttrEntryList = equip.AttrEntryList.ToList();

                for (int index = 0; index < 4; index++)
                {
                    var child = gridRandom.Find(string.Format("Attribute_{0}", index));

                    if (index < AttrEntryList.Count)
                    {
                        int attrId = AttrEntryList[index].Key;
                        long attrBaseValue = AttrEntryList[index].Value;

                        child.GetComponent<Text>().text = FormatAttrText(attrId, attrBaseValue, 0);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (config.Cycle > 0)
            {
                Tf_Set.gameObject.SetActive(true);

                EquipSetSuit red = user.GetEquipSet(0, equip.Config.Cycle);

                this.ShowRed(red, config);
            }


            this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);
            this.btn_UnEquip.gameObject.SetActive(this.boxItem.BoxId == -1);
            if (equip.Level > 1 || equip.Layer > 1)
            {
                this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
                this.btn_Recovery.gameObject.SetActive(false);
            }
            else
            {
                this.btn_Restore.gameObject.SetActive(false);
                this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1 && !this.boxItem.Item.IsLock);
            }
            this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
            this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);


            if (equipPositioin < -1 || this.BoxType != ComBoxType.Bag) //不可操作
            {
                this.btn_Equip.gameObject.SetActive(false);
                this.btn_UnEquip.gameObject.SetActive(false);
                this.btn_Recovery.gameObject.SetActive(false);
                this.btn_Restore.gameObject.SetActive(false);
                this.btn_Lock.gameObject.SetActive(false);
                this.btn_Unlock.gameObject.SetActive(false);
            }
        }

        private void ShowRed(EquipSetSuit redSuit, EquipSpeicalConfig config)
        {
            Text redTitle = Tf_Set.Find("Title").GetComponent<Text>();

            string color = QualityConfigHelper.GetQualityColor(config.Quality);

            if (config.Cycle == 101)
            {
                redTitle.text = string.Format("<color=#{0}>[四格套装]</color>", color);
            }

            Item_Equip_Red[] reds = Tf_Set.GetComponentsInChildren<Item_Equip_Red>(true);

            for (int i = 0; i < reds.Length; i++)
            {
                if (i < redSuit.List.Count)
                {
                    reds[i].gameObject.SetActive(true);
                    reds[i].SetEquipSpecial(redSuit.List[i], config);
                }
                else
                {
                    reds[i].gameObject.SetActive(false);
                }
            }
        }

        private string FormatAttrText(int attr, double val, double rise)
        {
            string unit = "";

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList().ToList(); ;

            if (!percents.Contains(attr))
            {
                unit = "%";
            }

            if (rise > 0)
            {
                return PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()] + " + " + StringHelper.FormatNumber(val) + "+" + rise + unit;
            }
            else
            {
                return PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()] + " + " + StringHelper.FormatNumber(val) + unit;
            }
        }



        private void OnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = true,
                BoxItem = this.boxItem,
            });
        }

        private void OnUnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipOneEvent()
            {
                IsWear = false,
                BoxItem = this.boxItem,
                Part = this.equipPositioin,
            });
        }

        private void OnClick_Restore()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能重生", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗5000兆金币，其他材料全额返回。是否确认？", true,
                () =>
                {
                    this.gameObject.SetActive(false);
                    GameProcessor.Inst.EventCenter.Raise(new RestoreEvent()
                    {
                        BoxItem = this.boxItem,
                    });
                }, () =>
                {

                });
        }

        private void OnRecovery()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能回收", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_Lock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = true
            });
        }

        private void OnClick_Unlock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = false
            });
        }
    }
}
