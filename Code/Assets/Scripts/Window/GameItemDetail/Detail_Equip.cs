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
    public class Detail_Equip : MonoBehaviour, IBattleLife
    {

        [Title("道具数据")]
        public Image img_Background;

        public Text Txt_Name;
        public Text Txt_Require;

        public Transform Tf_Base;

        public Transform Tf_Random;

        public Transform Tf_Quality;

        public Transform Tf_Rune;

        public Transform Tf_Suit;

        public Transform Tf_Set;

        [Title("导航")]

        public Button btn_Equip;
        public Button btn_UnEquip;

        public Button btn_Recovery;
        public Button btn_Card;
        public Button btn_Restore;

        public Button btn_Lock;
        public Button btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;
        private int Positioin;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.btn_Card.onClick.AddListener(this.OnCard);
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
            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEvent>(this.OnShowDetailEvent);
        }

        private void OnShowDetailEvent(ShowDetailEvent e)
        {
            if (e.Show_Type != ShowType.Equip)
            {
                return;
            }

            this.gameObject.SetActive(true);
            Tf_Base.gameObject.SetActive(false);
            Tf_Random.gameObject.SetActive(false);
            Tf_Quality.gameObject.SetActive(false);
            Tf_Rune.gameObject.SetActive(false);
            Tf_Suit.gameObject.SetActive(false);
            Tf_Set.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Card.gameObject.SetActive(false);
            this.btn_Restore.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            // this.transform.position = this.GetBetterPosition(e.Position);
            // this.img_Background.sprite = this.list_BackgroundImgs[this.item.GetQuality() - 1];
            this.boxItem = e.Show_Item;
            this.Positioin = e.Position;

            Equip equip = this.boxItem.Item as Equip;

            string name = equip.GetName();

            //if (equip.Part <= 10 || equip.Part >= 21)
            //{
            //    name += "(" + ConfigHelper.LayerChinaList[equip.Layer] + "阶)";
            //}
            var titleColor = QualityConfigHelper.GetQualityColor(equip.GetQuality());
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", titleColor, name);

            User user = GameProcessor.Inst.User;
            string color = user.MagicLevel.Data >= equip.Config.LevelRequired ? "green" : "red";

            this.Txt_Require.text = string.Format("<color={0}>需要等级{1}</color>", color, equip.Config.LevelRequired);

            long basePercent = 0;
            long qualityPercent = 0;

            long refineLevel = user.GetRefineLevel(e.Position);
            if (refineLevel > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(refineLevel);
                basePercent = refineConfig.GetBaseAttrPercent(refineLevel);
                qualityPercent = refineConfig.GetQualityAttrPercent(refineLevel);
            }
            else if (equip.Part >= 21 && equip.Quality >= 7)
            {
                basePercent = 0;
                qualityPercent = 100 * (equip.Layer - 1);
            }

            IDictionary<int, double> BaseAttrList = equip.GetBaseAttrList();

            if (BaseAttrList != null && BaseAttrList.Count > 0)
            {
                Tf_Base.gameObject.SetActive(true);
                Tf_Base.Find("Tf_Title").Find("Title_Text").GetComponent<Text>().text = "[基础属性]";

                Transform gridBase = Tf_Base.Find("Grid_Base");

                List<KeyValuePair<int, double>> btList = BaseAttrList.ToList();

                for (int index = 0; index < 8; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));

                    if (index < btList.Count())
                    {
                        child.GetComponent<Text>().text = FormatAttrText(btList[index].Key, btList[index].Value, basePercent);
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
                Tf_Random.Find("Tf_Title").Find("Title_Text").GetComponent<Text>().text = "[随机属性]";
                Transform gridRandom = Tf_Random.Find("Grid_Random");

                var AttrEntryList = equip.AttrEntryList.ToList();

                for (int index = 0; index < 6; index++)
                {
                    var child = gridRandom.Find(string.Format("Attribute_{0}", index));

                    if (index < AttrEntryList.Count)
                    {
                        int attrId = AttrEntryList[index].Key;
                        long attrBaseValue = AttrEntryList[index].Value;
                        long attrHoneVal = equip.GetHoneValue(index);
                        long attrRiseValue = (attrBaseValue + attrHoneVal) * qualityPercent / 100;

                        child.GetComponent<Text>().text = FormatEquipAttrText(attrId, attrBaseValue, attrHoneVal, attrRiseValue);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (equip.QualityAttrList != null && equip.QualityAttrList.Count > 0)
            {
                Tf_Quality.gameObject.SetActive(true);
                Tf_Quality.Find("Tf_Title").Find("Title_Text").GetComponent<Text>().text = "[品质属性]";
                Transform gridQuality = Tf_Quality.Find("Grid_Quality");

                var QualityAttrList = equip.QualityAttrList.ToList();

                for (int index = 0; index < 4; index++)
                {
                    var child = gridQuality.Find(string.Format("Attribute_{0}", index));

                    if (index < QualityAttrList.Count)
                    {
                        child.GetComponent<Text>().text = FormatAttrText(QualityAttrList[index].Key, QualityAttrList[index].Value, qualityPercent);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (equip.SkillRuneConfig != null)
            {
                List<int> runeIdList = new List<int>();
                if (equip.RuneConfigId > 0)
                {
                    runeIdList.Add(equip.RuneConfigId);
                }

                ShowRune(runeIdList);
            }

            if (equip.SkillSuitConfig != null)
            {
                int suitCount = user.GetSuitCount(equip.SkillSuitConfig.Id);

                List<int> suitIdList = new List<int>();
                suitIdList.Add(equip.SkillSuitConfig.Id);

                List<int> suitCountList = new List<int>();
                suitCountList.Add(suitCount);

                this.ShowSuit(suitIdList, suitCountList, user.GetSuitMax());
            }

            if (equip.Part >= 0)
            {
                Tf_Set.gameObject.SetActive(true);

                EquipSetSuit red = user.GetEquipSet(equip.Config.Role, equip.Config.Cycle);

                this.ShowRed(red, equip.GetQuality(), equip.Config.LevelRequired);
            }


            if (e.Box_Type == ComBoxType.Bag)
            {
                //包裹中
                this.btn_Equip.gameObject.SetActive(true);
                this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
                this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);

                if (equip.Layer > 1) //升阶过的只能重生
                {
                    this.btn_Restore.gameObject.SetActive(!this.boxItem.Item.IsLock);
                }
                else  //没升阶的只能回收
                {
                    this.btn_Recovery.gameObject.SetActive(!this.boxItem.Item.IsLock);
                }
                if (equip.GetQuality() == equip.Config.CardQuality && user.GetCardEquipLevel(equip.ConfigId) <= 0)
                {
                    this.btn_Card.gameObject.SetActive(true);
                }

            }
            else if (e.Box_Type == ComBoxType.OnEquip)
            {
                //装备栏中
                this.btn_UnEquip.gameObject.SetActive(true);
            }
            else
            {
                //其他地方都不显示任何按钮
                //this.btn_Equip.gameObject.SetActive(false);
                //this.btn_UnEquip.gameObject.SetActive(false);
                //this.btn_Recovery.gameObject.SetActive(false);
                //this.btn_Restore.gameObject.SetActive(false);
                //this.btn_Lock.gameObject.SetActive(false);
                //this.btn_Unlock.gameObject.SetActive(false);
            }
        }

        private void ShowRed(EquipSetSuit redSuit, int quality, int level)
        {
            Text redTitle = Tf_Set.Find("Tf_Title").Find("Title_Text").GetComponent<Text>();

            string color = QualityConfigHelper.GetQualityColor(quality);

            if (quality == 5)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}级橙装]</color>", color, level);
            }
            else if (quality == 6)
            {
                redTitle.text = string.Format("<color=#{0}>[红色套装]</color>", color);
            }
            else if (quality == 7)
            {
                redTitle.text = string.Format("<color=#{0}>[金色套装]</color>", color);
            }
            else if (quality == 8)
            {
                redTitle.text = string.Format("<color=#{0}>[暗金套装]</color>", color);
            }
            else if (quality == 9)
            {
                redTitle.text = string.Format("<color=#{0}>[混沌套装]</color>", color);
            }

            Item_Equip_Red[] reds = Tf_Set.GetComponentsInChildren<Item_Equip_Red>(true);

            for (int i = 0; i < reds.Length; i++)
            {
                if (i < redSuit.List.Count)
                {
                    reds[i].gameObject.SetActive(true);
                    reds[i].SetContent(redSuit.List[i], quality);
                }
                else
                {
                    reds[i].gameObject.SetActive(false);
                }
            }
        }

        private void ShowRune(List<int> runeIdList)
        {
            Item_Rune[] runes = Tf_Rune.GetComponentsInChildren<Item_Rune>(true);

            for (int i = 0; i < runes.Length; i++)
            {
                if (i < runeIdList.Count)
                {
                    runes[i].gameObject.SetActive(true);
                    runes[i].SetContent(runeIdList[i]);
                }
                else
                {
                    runes[i].gameObject.SetActive(false);
                }
            }
            Tf_Rune.gameObject.SetActive(true);
        }

        private void ShowSuit(List<int> suitIdList, List<int> countList, int max)
        {
            Item_Suit[] suits = Tf_Suit.GetComponentsInChildren<Item_Suit>(true);

            for (int i = 0; i < suits.Length; i++)
            {
                if (i < suitIdList.Count)
                {
                    suits[i].gameObject.SetActive(true);
                    suits[i].SetContent(suitIdList[i], countList[i], max);
                }
                else
                {
                    suits[i].gameObject.SetActive(false);
                }
            }
            Tf_Suit.gameObject.SetActive(true);
        }

        private string FormatAttrText(int attr, double val, long percent)
        {
            string unit = "";

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList().ToList(); ;

            if (!percents.Contains(attr))
            {
                unit = "%";
            }

            string refineText = "";
            double refineAttr = val * percent / 100;
            if (refineAttr > 0)
            {
                refineText = "+" + StringHelper.FormatNumber(refineAttr);
            }

            string text = StringHelper.FormatNumber(val) + refineText + unit + PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()];

            return text;
        }

        private string FormatEquipAttrText(int attrId, long baseValue, long riseValue, long percentValue)
        {
            string unit = "";

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList().ToList(); ;

            if (!percents.Contains(attrId))
            {
                unit = "%";
            }

            string text = baseValue + "";
            if (riseValue > 0)
            {
                text = "(" + baseValue + "+" + riseValue + ")";
            }

            if (percentValue > 0)
            {
                text += "+" + percentValue;
            }


            text = text + unit + PlayerHelper.PlayerAttributeMap[((AttributeEnum)attrId).ToString()];

            return text;
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
                Part = this.Positioin,
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

        public void OnCard()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipToCardEvent()
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
