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

        public Transform Tf_Legend;

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
            Tf_Legend.gameObject.SetActive(false);
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

            User user = User_Data_Manager.Data;
            string color = user.MagicLevel.Data >= equip.Config.LevelRequired ? "green" : "red";

            this.Txt_Require.text = string.Format("<color={0}>需要等级{1}</color>", color, equip.Config.LevelRequired);

            long basePercent = 0;
            long randomPercent = 0;

            long refineLevel = equip.RefineLevel.Data;
            if (refineLevel > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByPart(equip.Config.Part);
                basePercent = refineConfig.GetRisePercent(refineLevel, 1);
                basePercent += (int)user.AttributeBonus.CalPanelAtr(AttributeEnum.EquipBaseIncrea);
                randomPercent = refineConfig.GetRisePercent(refineLevel, 2);
            }

            IDictionary<int, double> BaseAttrList = equip.GetBaseAttrList();

            if (BaseAttrList != null && BaseAttrList.Count > 0)
            {
                Tf_Base.gameObject.SetActive(true);
                Tf_Base.Find("Tf_Title").Find("Title_Text").GetComponent<Text>().text = "[基础属性]";

                Transform gridBase = Tf_Base.Find("Grid_Base");

                List<KeyValuePair<int, double>> btList = BaseAttrList.ToList();
                List<KeyValuePair<int, double>> refineList = equip.GetRefineSpeAtrList().ToList();

                for (int index = 0; index < 6; index++)
                {
                    var child = gridBase.Find(string.Format("Attribute_{0}", index));
                    int bc = btList.Count();

                    if (index < btList.Count())
                    {
                        child.GetComponent<Text>().text = FormatAttrText(btList[index].Key, btList[index].Value, basePercent);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (index < bc + refineList.Count)
                        {
                            string txt = FormatAttrText(refineList[index - bc].Key, refineList[index - bc].Value);
                            child.GetComponent<Text>().text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(2), txt);
                            child.gameObject.SetActive(true);
                        }
                        else
                        {
                            child.gameObject.SetActive(false);
                        }
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
                        long attrRiseValue = (attrBaseValue) * randomPercent / 100;

                        child.GetComponent<Text>().text = FormatAttrText(attrId, attrBaseValue, randomPercent);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            if (equip.LegendData.Key > 0)
            {
                Tf_Legend.gameObject.SetActive(true);

                int lgId = equip.LegendData.Key;
                EquipLegendConfig config = EquipLegendConfigCategory.Instance.Get(lgId);

                List<Equip_Item_Legend> lgs = Tf_Legend.GetComponentsInChildren<Equip_Item_Legend>().ToList();
                lgs[0].ShowBase(config, equip.LegendData.Value);  //显示传奇部件属性

                EquipLegendSet legendSet = user.GetEquipLegendSet(config.SetId);  //显示传奇套装石属性
                lgs[1].ShowSet(legendSet);
            }


            if (equip.SkillRuneConfig != null)
            {
                if (equip.RuneConfigId > 0)
                {
                    ShowRune(equip.RuneConfigId);
                }
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

            if (equip.Config.Cycle < 10)
            {
                Tf_Set.gameObject.SetActive(true);

                EquipSetSuit red = user.GetEquipSet(equip.Config.Role, equip.Config.Cycle);

                this.ShowRed(red, equip.GetQuality(), equip.Config.Role);
            }


            if (e.Box_Type == ComBoxType.Bag)
            {
                //包裹中
                this.btn_Equip.gameObject.SetActive(true);
                this.btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
                this.btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);

                if (equip.RefineLevel.Data >= 1) //升阶过的只能重生
                {
                    this.btn_Restore.gameObject.SetActive(!this.boxItem.Item.IsLock);
                }
                else  //没升阶的只能回收
                {
                    this.btn_Recovery.gameObject.SetActive(!this.boxItem.Item.IsLock);

                    if (equip.Config.CardId > 0 && equip.GetQuality() == equip.Config.CardQuality && !user.IsCardMax(equip.Config.CardId))
                    {
                        this.btn_Card.gameObject.SetActive(!this.boxItem.Item.IsLock);
                    }
                }
            }
            else if (e.Box_Type == ComBoxType.OnEquip)
            {
                //装备栏中
                this.btn_UnEquip.gameObject.SetActive(true);
            }
        }

        private void ShowRed(EquipSetSuit redSuit, int quality, int role)
        {
            Text redTitle = Tf_Set.Find("Tf_Title").Find("Title_Text").GetComponent<Text>();

            string color = QualityConfigHelper.GetQualityColor(quality);
            string rt = ConfigHelper.RoleName[role - 1];

            if (quality == 5)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}橙装]</color>", color, rt);
            }
            else if (quality == 6)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}红装]</color>", color, rt);
            }
            else if (quality == 7)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}金装]</color>", color, rt);
            }
            else if (quality == 8)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}暗金]</color>", color, rt);
            }
            else if (quality == 9)
            {
                redTitle.text = string.Format("<color=#{0}>[{1}混沌]</color>", color, rt);
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

        private void ShowRune(int rid)
        {
            Item_Rune rune = Tf_Rune.GetComponentInChildren<Item_Rune>(true);

            User user = User_Data_Manager.Data;
            int count = user.GetRuneCount(rid);

            rune.gameObject.SetActive(true);
            rune.SetContent(rid, count);

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

        private string FormatAttrText(int attr, double val)
        {
            string text = StringHelper.FormatAttrText(attr, (long)val, "+");

            return text;
        }

        private string FormatAttrText(int attr, double val, long percent)
        {
            string unit = "";

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList().ToList(); ;

            if (!percents.Contains(attr) && attr < 21000)
            {
                unit = "%";
            }

            string refineText = "";
            double refineAttr = val * percent / 100;
            if (refineAttr > 0)
            {
                refineText = "+" + StringHelper.FormatNumber(refineAttr);
            }

            string text = PlayerHelper.PlayerAttributeMap[((AttributeEnum)attr).ToString()] + "+" + StringHelper.FormatNumber(val) + refineText + unit;

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
            this.gameObject.SetActive(false);

            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能重生", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗10000金币，其他材料全额返回。是否确认？", true,
                () =>
                {
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

            Equip equip = this.boxItem.Item as Equip;

            GameProcessor.Inst.EventCenter.Raise(new EquipToCardEvent()
            {
                BoxItem = this.boxItem,
                CardId = equip.Config.CardId
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
