using Newtonsoft.Json;
using SA.Android.Utilities;
using SA.CrossPlatform.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Game.PocketAD;

namespace Game
{
    public class ViewBagProcessor : AViewPage
    {
        [Title("人物属性")]
        public Transform Tf_Attr;
        private List<Item_Attr> Attr_List;

        [Title("包裹")]
        public Transform Tran_Bag_Nav_List;
        private List<Toggle> Toggle_Bag_Nav_List = new List<Toggle>();
        public Button Btn_Reset;

        public Transform Tran_Bag_List;
        private List<ScrollRect> Bag_List = new List<ScrollRect>();


        [Title("方案")]
        public Transform Tran_Plan_List;
        private List<Toggle> Toggle_Plan_List = new List<Toggle>();
        public Button Btn_ReName;

        //public Transform Tran_Equip_List;
        public Equip_Panel EquipPanel;

        public RectTransform EquipInfoSpecial;
        public Transform Tran_Plan;
        public InputField If_Name;
        public Button Btn_Ok;
        public Button Btn_Cancle;

        //public RectTransform Tf_Equip_Golden;


        [Title("功能按钮")]
        public Button Btn_Attr;
        public Button Btn_Achievement;
        public Button Btn_Cycle;

        public Button btn_SoulRing;
        public Button btn_Wing;
        public Button btn_Exclusive;
        public Button btn_Card;
        public Button btn_Shengxiao;

        public Button btn_Fashion;
        public Button btn_Halidom;
        public Button btn_Ring;
        public Button btn_Pill;
        public Button btn_Talent;
        public Button btn_Pet;

        public Button btn_Relic;


        [Title("功能框")]
        public Dialog_Exclusive DialogExclusive;
        public Dialog_Card DialogCard;
        public Dialog_Wing DialogWing;
        public Dialog_Halidom DialogHalidom;
        public Dialog_Ring DialogRing;
        public Dialog_Cycle DialogCycle;
        public Dialog_Pill DialogPill;

        private List<Com_Box> items = new List<Com_Box>();

        private void Awake()
        {
            Toggle_Bag_Nav_List = Tran_Bag_Nav_List.GetComponentsInChildren<Toggle>().ToList();
            Toggle_Plan_List = Tran_Plan_List.GetComponentsInChildren<Toggle>().ToList();
            Bag_List = Tran_Bag_List.GetComponentsInChildren<ScrollRect>(true).ToList();
            Attr_List = Tf_Attr.GetComponentsInChildren<Item_Attr>(true).ToList();

            for (int i = 0; i < Toggle_Bag_Nav_List.Count; i++)
            {
                int index = i;
                Toggle_Bag_Nav_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ShowBagPanel(index);
                    }
                });
            }
            this.Btn_Cycle.gameObject.SetActive(false);

            this.Btn_Attr.onClick.AddListener(this.OnClick_Attr);
            this.Btn_Achievement.onClick.AddListener(this.OnClick_Achievement);

            this.btn_SoulRing.onClick.AddListener(this.OnClick_RingSoul);
            this.btn_Wing.onClick.AddListener(OnOpenWing);
            this.btn_Exclusive.onClick.AddListener(OnExclusive);
            this.btn_Fashion.onClick.AddListener(OpenFashion);
            this.btn_Card.onClick.AddListener(OnOpenCard);
            this.btn_Halidom.onClick.AddListener(OnOpenHalidom);
            this.btn_Ring.onClick.AddListener(OnOpenRing);
            this.btn_Pill.onClick.AddListener(OnOpenPill);
            this.btn_Talent.onClick.AddListener(OnOpenTalent);
            this.btn_Relic.onClick.AddListener(OnOpenRelic);

            this.Btn_Reset.onClick.AddListener(OnRefreshBag);
            this.Btn_ReName.onClick.AddListener(OnSetPlanName);
            this.Btn_Ok.onClick.AddListener(OnPlanNameOK);
            this.Btn_Cancle.onClick.AddListener(OnPlanNameClose);

            this.btn_Pet.onClick.AddListener(OnOpenPet);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            GameProcessor.Inst.EventCenter.AddListener<EquipOneEvent>(this.OnEquipOneEvent);
            GameProcessor.Inst.EventCenter.AddListener<EquipToCardEvent>(this.OnEquipToCard);
            GameProcessor.Inst.EventCenter.AddListener<SkillBookLearnEvent>(this.OnSkillBookLearn);
            GameProcessor.Inst.EventCenter.AddListener<RecoveryEvent>(this.OnRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<RestoreEvent>(this.OnRestoreEvent);
            GameProcessor.Inst.EventCenter.AddListener<LoseEvent>(this.OnLoseEvent);
            GameProcessor.Inst.EventCenter.AddListener<AutoRecoveryEvent>(this.OnAutoRecoveryEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagUseEvent>(this.OnBagUseEvent);
            GameProcessor.Inst.EventCenter.AddListener<BagRemoveEvent>(this.OnBagRemove);

            GameProcessor.Inst.EventCenter.AddListener<CompositeEvent>(this.OnCompositeEvent);
            GameProcessor.Inst.EventCenter.AddListener<SystemUseEvent>(this.OnSystemUse);
            GameProcessor.Inst.EventCenter.AddListener<SelectGiftEvent>(this.OnSelectGift);
            GameProcessor.Inst.EventCenter.AddListener<EquipLockEvent>(this.OnEquipLockEvent);
            GameProcessor.Inst.EventCenter.AddListener<ExchangeEvent>(this.OnExchangeEvent);
            GameProcessor.Inst.EventCenter.AddListener<UpdateBagPanelUserAttr>(this.UpdateUserAttr);
            GameProcessor.Inst.EventCenter.AddListener<ChangeEquipPlanEvent>(this.OnChangeEquipPlanEvent);

            GameProcessor.Inst.EventCenter.AddListener<PetBattleUpEvent>(this.PetBattleUp);

            int EquipPanelIndex = GameProcessor.Inst.User.EquipPanelIndex;
            Toggle_Plan_List[EquipPanelIndex].isOn = true;
            this.InitPlanName();

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                int index = i;
                Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangePlan(index);
                    }
                });
            }

            this.InitAttr();

            GameProcessor.Inst.StartCoroutine(LoadBox());
        }

        private void InitAttr()
        {
            User user = GameProcessor.Inst.User;
            if (user == null)
            {
                return;
            }

            AttributeEnum[] list = new AttributeEnum[] {
            AttributeEnum.HP, AttributeEnum.Def
            ,AttributeEnum.PhyAtk,  AttributeEnum.MagicAtk
            ,AttributeEnum.SpiritAtk, AttributeEnum.Lucky
            ,AttributeEnum.DamageIncrea,   AttributeEnum.DamageResist
            ,AttributeEnum.CritRate,  AttributeEnum.CritDamage
            ,AttributeEnum.Speed,AttributeEnum.MoveSpeed
            ,AttributeEnum.Accuracy,AttributeEnum.Miss

        };

            for (int i = 0; i < Attr_List.Count; i++)
            {
                Item_Attr item = Attr_List[i];
                if (i < list.Length)
                {
                    item.gameObject.SetActive(true);

                    AttributeEnum attrId = list[i];
                    item.SetContent((int)attrId, user.AttributeBonus.CalPanelTotalAttr(attrId));
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        private void UpdateUserAttr(UpdateBagPanelUserAttr e)
        {
            //Debug.Log("UpdateBagPanelUserAttr");

            this.InitAttr();
        }

        private IEnumerator LoadBox()
        {
            User user = GameProcessor.Inst.User;
            GameProcessor.Inst.EventCenter.AddListener<HeroBagUpdateEvent>(this.OnHeroBagUpdateEvent);

            this.items = new List<Com_Box>();

            var prefab = Resources.Load<GameObject>("Prefab/Window/Box_Info");
            yield return null;

            var slots = EquipPanel.GetComponentsInChildren<SlotBox>();

            for (int p = 0; p < slots.Count(); p++)
            {
                slots[p].Init(p + 1);
            }

            List<SlotBox> sps = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().ToList();
            for (int i = 0; i < sps.Count; i++)
            {
                sps[i].Init(1001 + i);
                yield return null;
            }

            //穿戴装备
            foreach (var kvp in user.EquipPanelList[user.EquipPanelIndex])
            {
                this.CreateEquipPanelItem(kvp.Key, kvp.Value);
                //yield return null;
            }


            //穿戴四格
            foreach (var kvp in user.EquipSpecialList)
            {
                this.CreateEquipPanelItem(kvp.Key, kvp.Value);
                //yield return null;
            }

            //穿戴金装
            //foreach (var kvp in user.EquipPanelGoldenList[user.EquipGoldenIndex])
            //{
            //    this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
            //    //yield return null;
            //}


            var emptyPrefab = PrefabHelper.Instance().ComBoxEmpty;
            yield return null;

            for (int k = 0; k < Bag_List.Count; k++)
            {
                for (var i = 0; i < ConfigHelper.BagCount[k]; i++)
                {
                    var empty = GameObject.Instantiate(emptyPrefab, this.Bag_List[k].content);
                    empty.name = "Box_" + i;
                    //yield return null;
                }
            }

            //先回收,再加载
            this.FirstRecovery();

            RefreshBag();

            //yield return null;
        }

        private void OnRefreshBag()
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
            this.RecoveryAll(recoveryList, RuleType.Normal);

            RefreshBag();
        }

        private void OnSetPlanName()
        {
            this.Tran_Plan.gameObject.SetActive(true);
        }

        private void OnPlanNameOK()
        {
            string name = this.If_Name.text.Trim();

            if (name.Length > 2)
            {
                name = name.Substring(0, 2);
            }

            User user = GameProcessor.Inst.User;

            user.PlanNameList[user.EquipPanelIndex] = name;

            this.InitPlanName();
            this.Tran_Plan.gameObject.SetActive(false);
        }

        private void InitPlanName()
        {
            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                user.PlanNameList.TryGetValue(i, out string name);
                if (name != null)
                {
                    Text tt = Toggle_Plan_List[i].GetComponentInChildren<Text>();
                    tt.text = name;
                }
            }
        }

        private void OnPlanNameClose()
        {
            this.Tran_Plan.gameObject.SetActive(false);
        }

        private void RefreshBag()
        {
            foreach (Com_Box box in items)
            {
                GameObject.Destroy(box.gameObject);
            }
            items.Clear();

            User user = GameProcessor.Inst.User;

            if (user.Bags != null)
            {
                for (int i = 0; i < this.Bag_List.Count; i++)
                {
                    BuildBag(i);
                }
            }
        }

        private void BuildBag(int index)
        {
            ScrollRect bagRect = this.Bag_List[index];
            List<BoxItem> list = GameProcessor.Inst.User.Bags.Where(m => m.GetBagType() == index).OrderBy(m => m.GetBagSort()).ToList();

            for (int BoxId = 0; BoxId < list.Count; BoxId++)
            {
                if (BoxId + 1 > ConfigHelper.BagCount[index])
                {
                    return;
                }

                var bagBox = bagRect.content.GetChild(BoxId);
                if (bagBox == null)
                {
                    return;
                }

                BoxItem item = list[BoxId];
                item.BoxId = BoxId;

                Com_Box box = PrefabHelper.Instance().CreateComBox(item);
                box.transform.SetParent(bagBox);
                box.transform.localPosition = Vector3.zero;
                box.transform.localScale = Vector3.one;
                box.SetBoxId(BoxId);
                this.items.Add(box);
            }
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Bag;
        }

        private void OnCompositeEvent(CompositeEvent e)
        {
            CompositeConfig Config = e.Config;
            long number = e.Number;

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Config.ItemIdList.Length; i++)
            {
                ItemType type = (ItemType)(Config.ItemTypeList[i]);
                int configId = Config.ItemIdList[i];
                int quality = Config.ItemQualityList[i];

                if (type == ItemType.Equip)
                {
                    BoxItem boxItem = user.Bags.Where(m => m.Item.GetItemType() == type && m.Item.ConfigId == configId).FirstOrDefault();

                    GameProcessor.Inst.EventCenter.Raise(new BagUseEvent()
                    {
                        Quantity = 1,
                        BoxItem = boxItem
                    });

                    number = 1; //防止其他地方数量配置不对
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = type,
                        ItemId = configId,
                        Quantity = Config.ItemCountList[i] * number
                    });
                }
            }

            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 1, number);
            AddBoxItem(item);

            GameProcessor.Inst.EventCenter.Raise(new CompositeUIFreshEvent());
        }

        private void OnExchangeEvent(ExchangeEvent e)
        {
            ExchangeConfig Config = e.Config;
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    User user = GameProcessor.Inst.User;
                    BoxItem boxItem = user.Bags.Where(m => m.Item.GetItemType() == ItemType.Exclusive && m.Item.GetQuality() == 5 && !m.Item.IsLock).FirstOrDefault();

                    if (boxItem == null)
                    {
                        return;
                    }

                    GameProcessor.Inst.EventCenter.Raise(new BagRemoveEvent()
                    {
                        BoxItem = boxItem
                    });
                }
                else
                {
                    GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
                    {
                        Type = ItemType.Material,
                        ItemId = Config.ItemId,
                        Quantity = Config.ItemCount
                    });
                }
            }

            List<Item> list = new List<Item>();
            Item item = ItemHelper.BuildItem((ItemType)Config.TargetType, Config.TargetId, 5, 1);
            list.Add(item);
            GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = list });

            GameProcessor.Inst.EventCenter.Raise(new ExchangeUIFreshEvent());
        }

        private void OnSystemUse(SystemUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            List<BoxItem> list = user.Bags.Where(m => m.Item.GetItemType() == e.Type && m.Item.ConfigId == e.ItemId).ToList();

            long count = list.Select(m => m.MagicNubmer.Data).Sum();

            long useCount = Math.Abs(e.Quantity);

            if (count < useCount)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

            foreach (BoxItem boxItem in list)
            {
                long boxUseCount = Math.Min(boxItem.MagicNubmer.Data, useCount);

                Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
                boxItem.RemoveStack(boxUseCount);
                boxUI.RemoveStack(boxUseCount);

                if (boxItem.MagicNubmer.Data <= 0)
                {
                    user.Bags.Remove(boxItem);

                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);

                }

                useCount = useCount - boxUseCount;

                if (useCount <= 0)
                {
                    break;
                }
            }


            List<BoxItem> newList = user.Bags.Where(m => m.Item.GetItemType() == e.Type && m.Item.ConfigId == e.ItemId).ToList();

            long newCount = newList.Select(m => m.MagicNubmer.Data).Sum();
            if (newCount >= count)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

        }

        private void OnSelectGift(SelectGiftEvent e)
        {
            if (UseBoxItem(e.BoxItem, e.Nubmer))
            {
                List<Item> items = new List<Item>();

                e.Item.Count *= e.Nubmer;

                items.Add(e.Item);
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }

        //private void OnChangeExclusiveEvent(ChangeExclusiveEvent e)
        //{
        //    User user = GameProcessor.Inst.User;
        //    user.ExclusiveIndex = e.Index;

        //    for (int i = 15; i <= 20; i++)
        //    {
        //        this.ClearEquipPanelItem(i);
        //    }

        //    foreach (var kvp in user.ExclusivePanelList[e.Index])
        //    {
        //        this.CreateEquipPanelItem(-1, kvp.Key, kvp.Value);
        //    }

        //    //Debug.Log("OnChangeExclusiveEvent");
        //}

        private void OnChangeEquipPlanEvent(ChangeEquipPlanEvent e)
        {
            User user = GameProcessor.Inst.User;

            if (e.Type == 1)
            {
                user.EquipPanelIndex = e.Index;

                for (int i = 1; i <= 10; i++)
                {
                    this.ClearEquipPanelItem(i);
                }

                foreach (var kvp in user.EquipPanelList[e.Index])
                {
                    this.CreateEquipPanelItem(kvp.Key, kvp.Value);
                }
            }
            else if (e.Type == 3)
            {
                user.EquipGoldenIndex = e.Index;

                for (int i = 21; i <= 30; i++)
                {
                    this.ClearEquipPanelItem(i);
                }

                foreach (var kvp in user.EquipPanelGoldenList[e.Index])
                {
                    this.CreateEquipPanelItem(kvp.Key, kvp.Value);
                }
            }
        }


        private void ChangePlan(int index)
        {
            User user = GameProcessor.Inst.User;
            user.EquipPanelIndex = index;

            GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 1, Index = index });

            if (user.EquipGoldenSetting)
            {
                user.EquipGoldenIndex = index;
                GameProcessor.Inst.EventCenter.Raise(new ChangeEquipPlanEvent() { Type = 3, Index = index });
            }

            user.SkillPanelIndex = index;

            GameProcessor.Inst.EventCenter.Raise(new SkillChangePlanEvent());
            GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());
        }

        private void ShowBagPanel(int index)
        {
            for (int i = 0; i < this.Bag_List.Count; i++)
            {
                if (i == index)
                {
                    this.Bag_List[i].gameObject.SetActive(true);
                }
                else
                {
                    this.Bag_List[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnEquipToCard(EquipToCardEvent e)
        {
            User user = GameProcessor.Inst.User;

            Equip equip = e.BoxItem.Item as Equip;

            UseBoxItem(e.BoxItem, 1);

            user.CardEquipDict[equip.ConfigId] = 1;

            GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());
        }


        private void OnEquipOneEvent(EquipOneEvent e)
        {
            User user = GameProcessor.Inst.User;
            int type = e.BoxItem.GetBagType();
            int total = user.GetBagIdleCount(type);

            if (total < 5)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹空额不足了，请先清理包裹", ToastType = ToastTypeEnum.Failure });
                return;
            }


            if (e.IsWear)
            {
                this.WearToPanel(e.BoxItem);
            }
            else
            {
                this.RmoveFromPanel(e.Part, e.BoxItem);
            }
            //UserData.Save();

            //TaskHelper.CheckTask(TaskType.Equip, 1);
        }

        private void PetBattleUp(PetBattleUpEvent e)
        {
            User user = GameProcessor.Inst.User;

            int pg = Math.Min(3, user.GetPetSpeicalGroupLevel() / 3);

            if (user.PetList.Count >= ConfigHelper.PetMax + pg)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "宠物上阵位置已经满了", ToastType = ToastTypeEnum.Failure });
                return;
            }

            Pet pet = e.BoxItem.Item as Pet;

            int roleCount = user.PetList.Where(m => m.Role == pet.Role).Count();
            if (roleCount >= 3)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "同种宠物上阵数量已满", ToastType = ToastTypeEnum.Failure });
                return;
            }

            //从包袱移除
            UseBoxItem(e.BoxItem, 1);

            user.PetList.Add(pet);

            //通知英雄更新属性
            GameProcessor.Inst.EventCenter.Raise(new HeroUseEquipEvent { });
        }




        private void OnSkillBookLearn(SkillBookLearnEvent e)
        {
            User user = GameProcessor.Inst.User;

            if (UseBoxItem(e.BoxItem, 1))
            {
                GameProcessor.Inst.EventCenter.Raise(new HeroUseSkillBookEvent
                {
                    IsLearn = true,
                    BoxItem = e.BoxItem,
                    Quantity = 1,
                });
            }
        }

        private void FirstRecovery()
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
            this.RecoveryAll(recoveryList, RuleType.Normal);
        }

        private void OnRecoveryEvent(RecoveryEvent e)
        {
            //回收部分
            if (e.Quantity > 0)
            {
                this.RecoverySingle(e.BoxItem, e.Quantity);
            }
            //回收全部
            else
            {
                List<BoxItem> recoveryList = new List<BoxItem>();
                recoveryList.Add(e.BoxItem);
                this.RecoveryAll(recoveryList, RuleType.Normal);
            }

        }

        private void OnRestoreEvent(RestoreEvent e)
        {
            BoxItem boxItem = e.BoxItem;
            int bagType = boxItem.GetBagType();

            User user = GameProcessor.Inst.User;

            int haveCount = user.GetBagIdleCount(bagType);

            if (user.MagicGold.Data <= ConfigHelper.RestoreGold)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "金币不足5000兆", ToastType = ToastTypeEnum.Failure });
                return;
            }

            List<Item> newList = new List<Item>();

            if (boxItem.Item.GetItemType() == ItemType.Equip)
            {
                Equip equip = boxItem.Item as Equip;
                int layer = equip.Layer;

                //for (int l = 1; l < layer; l++)
                //{
                //    EquipGradeConfig config = EquipGradeConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Part == equip.Part && m.Layer == l).FirstOrDefault();

                //    Item item = ItemHelper.BuildMaterial(config.MetailId, config.MetailCount);
                //    newList.Add(item);

                //    Item item1 = ItemHelper.BuildMaterial(config.MetailId1, config.MetailCount1);
                //    newList.Add(item1);

                //}

                int redNumber = 0;
                foreach (var kv in equip.HoneList)
                {
                    int honeLevel = kv.Value;
                    redNumber += EquipHoneConfigCategory.Instance.GetTotalNeedNumber(honeLevel);
                }

                if (redNumber > 0)
                {
                    Item redItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, redNumber);
                    newList.Add(redItem);
                }

                if (haveCount < newList.Count)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请保留" + newList.Count + "个包裹空额", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                equip.Layer = 1;
                equip.HoneList = new Dictionary<int, int>();
                newList.Add(equip);
            }
            else if (boxItem.Item.GetItemType() == ItemType.Pet)
            {
                Pet pet = boxItem.Item as Pet;
                long level = pet.PetLevel.Data;
                long layer = pet.PetLayer.Data;
                int quality = pet.GetQuality();

                long expCount = PetConfigCategory.Instance.GetFeeTotal(level) + pet.LevelExp.Data;
                long layerCount = PetConfigCategory.Instance.GetPetLayerFeeTotal(layer);

                pet.PetLayer.Data = 1;
                pet.PetLevel.Data = 1;
                pet.LevelExp.Data = 0;


                //Debug.Log("pet exp count:" + expCount);
                if (expCount > 0)
                {
                    Item levelItem = ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pet_Exp, expCount);
                    newList.Add(levelItem);
                }

                if (layerCount > 0)
                {
                    Item layerItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Pet_Layer[quality - 5], layerCount);
                    newList.Add(layerItem);
                }

                newList.Add(pet);
            }
            else if (boxItem.Item.GetItemType() == ItemType.Shengxiao)
            {
                Shengxiao pet = boxItem.Item as Shengxiao;
                long level = pet.LevelData.Data;
                long layer = pet.LayerData.Data;
                int quality = pet.GetQuality();

                pet.LevelData.Data = 0;
                pet.LayerData.Data = 0;

                //Debug.Log("pet exp count:" + expCount);
                if (level > 0)
                {
                    Item levelItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao, 5000000 * level);
                    newList.Add(levelItem);

                    Item layerItem1 = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao1, 30 * level);
                    newList.Add(layerItem1);
                }

                if (layer > 0)
                {
                    Item layerItem = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao1, 20 * layer);
                    newList.Add(layerItem);

                    Item layerItem1 = ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao2, layer);
                    newList.Add(layerItem1);
                }

                newList.Add(pet);
            }

            //Fee
            user.SubGold(ConfigHelper.RestoreGold);

            //销毁旧的
            user.Bags.Remove(boxItem);
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);

                //生成新的
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = newList });
            }
        }

        private void OnLoseEvent(LoseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = e.BoxItem;

            if (boxItem == null)
            {
                //Log.Debug("此物品已经被使用了");
                return;
            }

            user.Bags.Remove(boxItem);

            //Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                this.items.Remove(boxUI);
                GameObject.Destroy(boxUI.gameObject);
            }
        }

        private void OnAutoRecoveryEvent(AutoRecoveryEvent e)
        {
            User user = GameProcessor.Inst.User;
            List<BoxItem> recoveryList = user.Bags.Where(m => !m.Item.IsLock && user.RecoveryNew.CheckRecovery(m.Item, RecoveryType.Other)).ToList();
            this.RecoveryAll(recoveryList, e.RuleType);
        }

        private void RecoverySingle(BoxItem boxItem, int quantity)
        {
            User user = GameProcessor.Inst.User;

            long gold = 0;

            UseBoxItem(boxItem, quantity);

            List<Item> itemList = new List<Item>();

            Dictionary<int, long> recoveryDict = user.Recovery(boxItem.Item, out long recoveryGold);

            gold += recoveryGold * boxItem.MagicNubmer.Data;

            foreach (var kvp in recoveryDict)
            {
                if (kvp.Value > 0)
                {
                    Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value * quantity);
                    AddBoxItem(recoveryItem);
                    itemList.Add(recoveryItem);
                }
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Type = RuleType.Normal,
                Message = BattleMsgHelper.BuildAutoRecoveryMessage(quantity, itemList, gold)
            });
        }

        private void RecoveryAll(List<BoxItem> recoveryList, RuleType ruleType)
        {
            User user = GameProcessor.Inst.User;

            List<Item> itemList = new List<Item>();

            Dictionary<int, long> recoveryDict = new Dictionary<int, long>();

            long gold = 0;

            foreach (BoxItem box in recoveryList)
            {
                Dictionary<int, long> dict = user.Recovery(box.Item, out long recoveryGold);

                gold += recoveryGold;

                foreach (var sp in dict)
                {
                    if (!recoveryDict.ContainsKey(sp.Key))
                    {
                        recoveryDict[sp.Key] = 0;
                    }

                    long count = Math.Max(1, box.MagicNubmer.Data);
                    recoveryDict[sp.Key] += (sp.Value * count);
                }


                UseBoxItem(box, box.MagicNubmer.Data);
            }

            if (gold > 0)
            {
                user.AddExpAndGold(0, gold);
            }

            foreach (var kvp in recoveryDict)
            {
                if (kvp.Value > 0)
                {
                    Item recoveryItem = ItemHelper.BuildMaterial(kvp.Key, kvp.Value);
                    AddBoxItem(recoveryItem);
                    itemList.Add(recoveryItem);
                }
            }

            if (recoveryList.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Type = ruleType,
                    Message = BattleMsgHelper.BuildAutoRecoveryMessage(recoveryList.Count, itemList, gold)
                });
            }
        }

        private void OnBagUseEvent(BagUseEvent e)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = e.BoxItem;
            long quantity = e.Quantity <= 0 ? boxItem.MagicNubmer.Data : e.Quantity;

            if (boxItem.Item.GetItemType() == ItemType.Ticket && boxItem.Item.ConfigId == ItemHelper.SpecialId_Copy_Ticket && e.Quantity == -1)
            {

            }
            else if (boxItem.Item.GetItemType() == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Level_Stone)
            {
                quantity = Math.Min(quantity, user.GetMaxLevel() - user.MagicLevel.Data);

                if (quantity <= 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                    return;
                }
            }

            if (quantity <= 0)
            {
                throw new Exception();
                //GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
            }

            if (!UseBoxItem(boxItem, quantity))
            {
                return;
            }

            //use logic
            if (boxItem.Item.GetItemType() == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Level_Stone)
            {
                quantity = Math.Min(quantity, user.GetMaxLevel() - user.MagicLevel.Data);

                if (quantity <= 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经满级了", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.MagicLevel.Data += quantity;
                GameProcessor.Inst.EventCenter.Raise(new SetPlayerLevelEvent { Cycle = user.Cycle.Data, Level = user.MagicLevel.Data });
            }
            else if (boxItem.Item.GetItemType() == ItemType.Material_Usable && boxItem.Item.ConfigId == ItemHelper.SpecialId_Talent_Book)
            {
                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                user.TalentExp.Data += quantity * config.UseParam;
            }
            else if (boxItem.Item.GetItemType() == ItemType.SkillBox)
            {
                GameProcessor.Inst.EventCenter.Raise(new HeroUseSkillBookEvent
                {
                    IsLearn = false,
                    BoxItem = boxItem,
                    Quantity = quantity,
                });
            }
            else if (boxItem.Item.GetItemType() == ItemType.ExpPack)
            {
                long exp = (long)(user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SecondExp));

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                exp = exp * quantity * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(exp, 0);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", exp, 0, null)
                });
            }
            else if (boxItem.Item.GetItemType() == ItemType.GoldPack)
            {
                long gold = (long)(user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SecondGold));

                ItemConfig config = ItemConfigCategory.Instance.Get(boxItem.Item.ConfigId);

                gold = gold * quantity * config.UseParam * 720; //3600/5 = 720,配置的是小时

                user.AddExpAndGold(0, gold);
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("道具奖励", 0, gold, null)
                });
            }
            else if (boxItem.Item.GetItemType() == ItemType.GiftPack)
            {
                Gift_Pack giftPack = boxItem.Item as Gift_Pack;
                GiftPackConfig pc = giftPack.Config;

                List<Item> items = new List<Item>();
                for (int i = 0; i < pc.ItemIdList.Length; i++)
                {
                    Item item = ItemHelper.BuildItem((ItemType)pc.ItemTypeList[i], pc.ItemIdList[i], 1, (quantity * pc.ItemCountList[i]));
                    //this.AddBoxItem(item);
                    items.Add(item);
                }
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
                {
                    Message = BattleMsgHelper.BuildGiftPackMessage("礼包奖励:", 0, 0, items)
                });
                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
            else if (boxItem.Item.GetItemType() == ItemType.Ticket)
            {
                if (boxItem.Item.ConfigId == ItemHelper.SpecialId_Legacy_Ticket)
                {
                    user.LegacyTikerCount.Data += quantity;
                }
                else if (boxItem.Item.ConfigId == ItemHelper.SpecialId_Pill_Ticket)
                {
                    user.PillTime.Time.Data += quantity * ConfigHelper.PillDefaultTime;
                }

            }
            else if (boxItem.Item.GetItemType() == ItemType.Pet)
            {
                //build pet
                //string str_json = JsonConvert.SerializeObject(e.Flairs);
                //Debug.Log("build pet flairs:" + str_json);
                List<Item> items = new List<Item>();

                Item item = PetConfigCategory.Instance.BuildPet(boxItem.Item.ConfigId, 5);
                items.Add(item);

                GameProcessor.Inst.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }
        }

        private void OnBagRemove(BagRemoveEvent e)
        {
            if (GameProcessor.Inst.User.RemoveBagItem(e.BoxItem))
            {
                Com_Box boxUI = this.items.Find(m => m.BoxItem == e.BoxItem);
                if (boxUI != null) //上线自动回收，可能还没加载
                {
                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);
                    boxUI = null;
                }
            }
            else
            {
                throw new Exception("道具不存在");
            }
        }


        private void OnEquipLockEvent(EquipLockEvent e)
        {
            var boxItem = e.BoxItem;

            boxItem.Item.IsLock = e.IsLock;

            Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType()); //穿戴的找不到这个UI
            if (boxUI != null)
            {
                boxUI.SetLock(e.IsLock);
            }
        }

        private bool UseBoxItem(BoxItem boxItem, long quantity)
        {
            User user = GameProcessor.Inst.User;

            //逻辑处理

            if (boxItem == null)
            {
                //Log.Debug("此物品已经被使用了");
                return false;
            }

            boxItem.RemoveStack(quantity);

            //Com_Box boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
            Com_Box boxUI = this.items.Find(m => m.BoxItem == boxItem);
            if (boxUI != null) //上线自动回收，可能还没加载
            {
                boxUI.RemoveStack(quantity);
                if (boxItem.MagicNubmer.Data <= 0)
                {
                    this.items.Remove(boxUI);
                    GameObject.Destroy(boxUI.gameObject);
                }
            }

            //用光了，移除队列
            if (boxItem.MagicNubmer.Data <= 0)
            {
                user.Bags.Remove(boxItem);
                boxItem = null;
            }

            return true;
        }

        private void AddBoxItem(Item newItem)
        {
            User user = GameProcessor.Inst.User;

            BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.GetItemType() == newItem.GetItemType() && m.Item.ConfigId == newItem.ConfigId);  //ͬ

            if (boxItem != null)
            {
                boxItem.AddStack(newItem.Count);

                //堆叠UI
                var boxUI = this.items.Find(m => m.boxId == boxItem.BoxId && m.BagType == boxItem.GetBagType());
                if (boxUI != null)
                {
                    boxUI.AddStack(newItem.Count);
                }
            }
            else
            {
                boxItem = new BoxItem();
                boxItem.Item = newItem;
                boxItem.MagicNubmer.Data = newItem.Count;
                boxItem.BoxId = -1;

                int bagType = boxItem.GetBagType();
                int idleCount = user.GetBagIdleCount(bagType);

                if (idleCount < 1)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹" + (bagType + 1) + "已经满了,请清理包裹", ToastType = ToastTypeEnum.Failure });
                    return;
                }

                user.Bags.Add(boxItem);

                int lastBoxId = GetNextBoxId(bagType);
                if (lastBoxId < 0)
                {  //包裹已经满了,不显示，但是实际保留
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "包裹" + (bagType + 1) + "已经满了,请清理包裹", ToastType = ToastTypeEnum.Failure });
                    return;
                }
                boxItem.BoxId = lastBoxId;

                var item = PrefabHelper.Instance().CreateComBox(boxItem);
                item.transform.SetParent(this.Bag_List[bagType].content.GetChild(lastBoxId));
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetBoxId(lastBoxId);
                this.items.Add(item);
            }
        }

        private void WearToPanel(BoxItem boxItem)
        {
            Item wearItem = boxItem.Item;

            int position = 0;

            if (wearItem.GetItemType() == ItemType.Equip)
            {
                Equip equip = wearItem as Equip;

                //增加一次穿戴记录，用做轮流穿戴左右
                position = AppHelper.GetEquipPosition(equip);
            }
            else if (wearItem.GetItemType() == ItemType.EquipSpeical)
            {
                Equip_Special item = wearItem as Equip_Special;
                position = item.Config.Part;
            }


            //从包袱移除
            UseBoxItem(boxItem, 1);

            //如果存在旧装备，增加到包裹
            Item oldEquip = GetEquip(position);
            if (oldEquip != null)
            {
                //装备栏卸载
                SlotBox slot = GetCurrentPanelEquipSolt(position);

                slot.UnEquip();

                AddBoxItem(oldEquip);
            }

            //穿戴到格子上
            this.CreateEquipPanelItem(position, wearItem);

            //记录
            AddEquip(position, wearItem);

            //通知英雄更新属性
            GameProcessor.Inst.EventCenter.Raise(new HeroUseEquipEvent { });
        }

        private void RmoveFromPanel(int position, BoxItem boxItem)
        {
            Item removeItem = boxItem.Item;

            //装备栏卸载
            SlotBox slot = GetCurrentPanelEquipSolt(position);

            slot.UnEquip();

            //装备移动到包裹里面
            AddBoxItem(removeItem);

            //
            RemoveEquip(position);

            //通知英雄更新属性
            GameProcessor.Inst.EventCenter.Raise(new HeroUnUseEquipEvent() { });

            //UserData.Save();
        }

        private void ClearEquipPanelItem(int position)
        {
            SlotBox slot = GetCurrentPanelEquipSolt(position);
            slot.UnEquip();
        }

        private SlotBox GetCurrentPanelEquipSolt(int position)
        {
            SlotBox slot = null;

            if (position <= 10)
            {
                int pi = GameProcessor.Inst.User.EquipPanelIndex;

                slot = EquipPanel.GetComponentsInChildren<SlotBox>().Where(s => s.Part == position).First();
            }
            else if (position >= 1001 && position <= 1004)
            {
                slot = EquipInfoSpecial.GetComponentsInChildren<SlotBox>().Where(s => s.Part == position).First();
            }

            return slot;
        }

        private Item GetEquip(int position)
        {
            User user = GameProcessor.Inst.User;

            if (position <= 10)
            {
                var ep = user.EquipPanelList[user.EquipPanelIndex];
                if (ep.ContainsKey(position))
                {
                    return ep[position];
                }

            }
            else if (position >= 1001 && position <= 1004)
            {
                var ep = user.EquipSpecialList;
                if (ep.ContainsKey(position))
                {
                    return ep[position];
                }
            }


            return null;
        }

        private void RemoveEquip(int position)
        {
            User user = GameProcessor.Inst.User;

            if (position <= 10)
            {
                user.EquipPanelList[user.EquipPanelIndex].Remove(position);

            }
            else if (position >= 1001 && position <= 1004)
            {
                user.EquipSpecialList.Remove(position);
            }
        }

        private void AddEquip(int position, Item equip)
        {
            User user = GameProcessor.Inst.User;

            if (position <= 10)
            {
                user.EquipPanelList[user.EquipPanelIndex][position] = equip as Equip;
            }
            else if (position >= 1001 && position <= 1004)
            {
                user.EquipSpecialList[position] = equip as Equip_Special;
            }

        }

        private void CreateEquipPanelItem(int position, Item equip)
        {
            SlotBox slot = GetCurrentPanelEquipSolt(position);


            if (slot.GetEquip() != null) //防止叠加，无限刷道具
            {
                return;
            }

            //生成格子
            BoxItem boxItem = new BoxItem();
            boxItem.Item = equip;
            boxItem.MagicNubmer.Data = 1;
            boxItem.BoxId = -1;

            Com_Box comItem = PrefabHelper.Instance().CreateComBox(boxItem);
            comItem.transform.SetParent(slot.transform);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;
            comItem.SetBoxId(-1);
            comItem.SetEquipPosition(position);

            //穿戴
            slot.Equip(comItem);
        }


        private void OnHeroBagUpdateEvent(HeroBagUpdateEvent e)
        {
            User user = GameProcessor.Inst.User;
            if (user.Bags != null)
            {
                var newItems = e.ItemList;

                foreach (Item newItem in newItems)
                {
                    if (newItem.GetItemType() == ItemType.Material_Hide)
                    {
                        user.SaveHideMaterialCount(newItem.ConfigId, newItem.Count);
                    }
                    else
                    {
                        AddBoxItem(newItem);
                    }
                }
            }
        }

        public int GetNextBoxId(int bagType)
        {
            int maxNum = ConfigHelper.BagCount[bagType];
            for (int boxId = 0; boxId < maxNum; boxId++)
            {
                if (this.items.Find(m => m.boxId == boxId && m.BagType == bagType) == null)
                {
                    return boxId;
                }
            }
            return -1;
        }

        public void OnClick_RingSoul()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowSoulRingEvent());
        }

        public void OnClick_Attr()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowDialogUserAttrEvent());
        }

        public void OnClick_Achievement()
        {
            GameProcessor.Inst.EventCenter.Raise(new ShowAchievementEvent());
        }
        public void OnClick_Cycle()
        {
            this.DialogCycle.gameObject.SetActive(true);

        }
        public void OnExclusive()
        {
            this.DialogExclusive.gameObject.SetActive(true);
        }

        public void OnOpenCard()
        {
            this.DialogCard.gameObject.SetActive(true);
        }

        public void OnOpenHalidom()
        {
            this.DialogHalidom.gameObject.SetActive(true);
        }


        public void OnOpenRing()
        {
            this.DialogRing.Show();
        }

        public void OnOpenPill()
        {
            this.DialogPill.gameObject.SetActive(true);
        }

        public void OnOpenWing()
        {
            this.DialogWing.gameObject.SetActive(true);
        }

        public void OnOpenTalent()
        {
            GameProcessor.Inst.EventCenter.Raise(new TalentShowEvent());
        }

        public void OnOpenRelic()
        {
            GameProcessor.Inst.EventCenter.Raise(new RelicShowEvent());
        }


        public void OnOpenPet()
        {
            GameProcessor.Inst.EventCenter.Raise(new PetShowEvent());
        }

        public void OpenFashion()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenFashionDialogEvent());
        }

        public override void OnOpen()
        {
            base.OnOpen();

            Debug.Log("open view bag");

            //重新计算人物属性
            GameProcessor.Inst.EventCenter.Raise(new UserAttrChangeEvent());
        }
    }
}