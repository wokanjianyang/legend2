using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Dialog_OfflineExp : MonoBehaviour
    {
        public ScrollRect Container;
        public Text Txt_Msg;
        public Text Txt_Name;
        public Text Txt_Kill;
        public Text Txt_Exp;

        public Button Btn_OK;

        public Button Btn_Close;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_OK.onClick.AddListener(this.OnClick_OK);
            this.Btn_Close.onClick.AddListener(this.OnClick_OK);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;


        private void OnClick_OK()
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
            //Time.timeScale = 1;
        }

        private void TestSend(User user)
        {
            List<Item> items = new List<Item>();

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Legacy_Ticket, 300)); //传世卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Copy_Ticket, 6000)); //装备卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Boss_Ticket, 300)); //BOSS卷
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Level_Stone, 20000));  //羽毛
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Wing_Stone, 100));  //幻境劵

            //items.Add(ItemHelper.BuildMaterial(5003, 100)); //专属碎片
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Heart, 100)); //专属之心
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 1999999999)); //四格碎片

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, 999999999)); //精炼石
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Red_Stone, 999)); //红装精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Golden_Stone, 999)); //金色精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Dark_Stone, 999)); //暗金精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Hundun, 999)); //混沌精华

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Golden, 999)); //传奇精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Dark, 999)); //不朽精华
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Stone_Set, 999)); //开孔石

            //items.Add(ItemHelper.BuildMaterial(50000106, 10000)); //十六阶装备升阶石

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecailEquipRefreshId, 99999)); //橙装精华

            //items.Add(ItemHelper.BuildItem(ItemType.Card, 2000010, 10, 5));

            //for (int i = 0; i < 10; i++)
            //{
            //    items.Add(PetConfigCategory.Instance.BuildPet(1, 7));
            //}

            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 1010, 1, 1));
            //items.Add(ItemHelper.BuildItem(ItemType.SkillBox, 3010, 1, 1));

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Pill, 6000000));

            //user.SaveArtifactLevel(180001, 1); //boss杀手
            //user.SaveArtifactLevel(180019, 1); //副本杀手

            //user.SaveArtifactLevel(180005, 10); //卖身契
            //user.SaveArtifactLevel(180006, 10); //万界图
            //user.SaveArtifactLevel(180007, 1); //破限
            //user.SaveArtifactLevel(180008, 1); //魔法
            //user.SaveArtifactLevel(180009, 1); //圣者
            //user.SaveArtifactLevel(180011, 1); //锤子
            //user.SaveArtifactLevel(180013, 1); //金蛟剪
            //user.SaveArtifactLevel(180015, 1); //财富契约
            //user.SaveArtifactLevel(180020, 1); //传世之源
            //user.SaveArtifactLevel(180021, 1); //神戒之源
            //user.SaveArtifactLevel(180030, 45); //极。卖身契
            //user.SaveArtifactLevel(180033, 45); //极。BOSS
            //user.SaveArtifactLevel(180035, 45); //极。魔法

            //user.Record.AddRecord(RecordType.AdReal, -800);
            //user.MagicGold.Data = 100000000000000000L;//10京金币

            //user.SaveItemMeterialCount(1999994, 100000);
            //user.SaveItemMeterialCount(ItemHelper.SpecialId_Card_Stone, 10000000);
            //user.Cycle.Data = 2;
            //items.Add(ItemHelper.BuildMaterial(10001,1)); //时装
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 32, 1, 1)); //普通金宠包
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 33, 1, 1)); //满资质金宠包
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 41, 1, 8)); //神器自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 95, 1, 6)); //传奇自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 90, 1, 6)); //不朽自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 36, 1, 1)); //宝石自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 43, 1, 1)); //生肖自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 44, 1, 1)); //极戒自选

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 26, 1, 1));  //神技

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 28, 1, 6));  //魂骨
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 29, 1, 3));  //11技能
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 42, 1, 1)); //12技能自选

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 106, 1, 3)); //白银
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 107, 1, 1)); //黄金
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 108, 1, 1)); //钻石

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 109, 1, 5));  //战士经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 110, 1, 1)); //法师经验
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 111, 1, 1)); //道士经验

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 112, 1, 10));  //战士输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 113, 1, 1)); //法师输出
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 114, 1, 1)); //道士输出

            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 127, 1, 1)); //金装自选
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 128, 1, 1)); //暗金自选

            //items.AddRange(AddGoldenEquip());
            //items.AddRange(AddExclusiveDaoshi());
            //items.AddRange(AddRedEquip1());

            //user.SaveItemMeterialCount(2100001, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100002, 11125); //英灵刘备
            //user.SaveItemMeterialCount(2100003, 11125); //英灵诸葛
            //user.SaveItemMeterialCount(2100004, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100005, 11125); //英灵关羽
            //user.SaveItemMeterialCount(2100006, 11125); //英灵关羽

            foreach (var item in items)
            {
                BoxItem boxItem = new BoxItem();
                boxItem.Item = item;
                boxItem.MagicNubmer.Data = item.Temp_Number;
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);
            }
        }



        public void ShowOffline()
        {
            User user = GameProcessor.Inst.User;

            if (user.OfflineLog.Count != 2)
            {
                this.Txt_Name.text = "没有设定离线副本";
                this.Txt_Exp.text = "没有收益";
                return;
            }

            List<Item> itemList = new List<Item>();

            long currentTick = TimeHelper.ClientNowSeconds();
            long offlineTime = currentTick - user.SecondExpTick;

            int tempTime = (int)Math.Min(offlineTime, ConfigHelper.MaxOfflineTime);

            int mapId = user.OfflineLog[1];
            int total = user.OfflineLog[2];

            MapConfig mapConfig = MapConfigCategory.Instance.Get(mapId);
            MonsterBase monsterConfig = MonsterBaseCategory.Instance.Get(mapId);

            int killCount = tempTime / ConfigHelper.OfflineTime * total;

            killCount = 3600 * 20 * 5;

            int kc = killCount * (mapConfig.Id + 5);

            List<Item> items = new List<Item>();
            long rewardExp = 0;
            long rewardGold = 0;


            double burstRise = (user.AttributeBonus.CalBattleSingleMul(AttributeEnum.BurstIncrea) + 100) / 100.0;
            double qualityRise = (user.AttributeBonus.CalBattleSingleMul(AttributeEnum.QualityIncrea) + 100) / 100.0;
            double expRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.ExpIncrea) + 100) / 100.0;
            double goldRise = (user.AttributeBonus.CalPanelTotalAttr(AttributeEnum.GoldIncrea) + 100) / 100.0;

            burstRise = 1;
            qualityRise = 1;

            long exp = ((long)(monsterConfig.Exp * expRise)) * killCount;
            long gold = ((long)(monsterConfig.Gold * goldRise)) * killCount;

            items.AddRange(BuildMapReward1(killCount, mapId, burstRise, qualityRise, ref gold));

            //itemList.Add(ItemHelper.BuildItem(dropConfig.ItemType, config.DropIdList, 0, sp.Value));

            //离线经验，金币

            //离线挖矿
            //this.BuildOfflineMine(user, tempTime, ref OfflineMessage);

            //测试道具
            this.TestSend(user);

            user.SecondExpTick = currentTick;
            this.Txt_Name.text = mapConfig.Name + "：离线时间" + tempTime + "秒";
            this.Txt_Kill.text = "击杀怪物：" + killCount + "，杀敌数+" + kc;
            this.Txt_Exp.text = "获得经验：" + exp + "，金币：" + gold;

            user.AddExpAndGold(exp, gold);

            foreach (var item in items)
            {
                Box_Drop box = PrefabHelper.Instance().CreateBoxDrop(Container.content, item);
            }

            foreach (var item in items)
            {

                if (item.GetItemType() == ItemType.Material_Hide)
                {
                    user.SaveHideMaterialCount(item.ConfigId, item.Temp_Number);
                }
                else
                {
                    BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.GetItemType() == item.GetItemType() && m.Item.ConfigId == item.ConfigId);  //ͬ

                    if (boxItem != null)
                    {
                        boxItem.AddStack(item.Temp_Number);
                    }
                    else
                    {
                        boxItem = new BoxItem();
                        boxItem.Item = item;
                        boxItem.MagicNubmer.Data = Math.Max(1, item.Temp_Number);
                        boxItem.BoxId = -1;
                        user.Bags.Add(boxItem);
                    }
                }
            }

            //检查
            DateTime saveDate = new DateTime(user.DataDate);
            if (saveDate.Day < DateTime.Now.Day || saveDate.Month < DateTime.Now.Month || saveDate.Year < DateTime.Now.Year)
            {
                user.DefendData.Refresh();
                user.BabelCount.Data = ConfigHelper.BabelCount;

                //user.HeroPhatomData.Refresh();
                //user.PillTime.Check(user.Cycle.Data);

                user.DataDate = DateTime.Now.Ticks;
                //保存到Tap
            }

            GameProcessor.Inst.SaveData();
            GameProcessor.Inst.SaveNetData();

            this.gameObject.SetActive(true);
            //Time.timeScale = 0;
        }

        private List<Item> BuildMapReward(int killCount, int mapId, double burstRise, double qualityRise)
        {
            User user = GameProcessor.Inst.User;

            List<Item> items = new List<Item>();

            for (int i = 0; i < killCount; i++)
            {
                //生成道具奖励
                items.AddRange(DropConfigCategory.Instance.BuildDropItem(mapId, burstRise, qualityRise));
            }

            List<Item> recoveryList = user.CheckRecovery(items, out long recoveryGold, out int recoveryCount);

            return items;
        }

        private List<Item> BuildMapReward1(int killCount, int mapId, double burstRise, double qualityRise, ref long gold)
        {
            List<Item> itemList = new List<Item>();

            IDictionary<int, int> dropDict = new Dictionary<int, int>();

            MapConfig config = MapConfigCategory.Instance.Get(mapId);

            for (int i = 0; i < config.DropIdList.Length; i++)
            {
                int dropCount = (int)(killCount * burstRise / config.DropRateList[i]);

                if (dropCount > 0)
                {
                    int dropId = config.DropIdList[i];

                    DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                    int baseRateTotal = dropConfig.BaseRateList.Sum();

                    for (int k = 0; k < dropConfig.BaseIdList.Length; k++)
                    {
                        int baseCount = dropCount * dropConfig.BaseRateList[k] / baseRateTotal;

                        if (baseCount > 0)
                        {
                            int baseId = dropConfig.BaseIdList[k];
                            if (!dropDict.ContainsKey(baseId))
                            {
                                dropDict[baseId] = 0;
                            }

                            dropDict[baseId] += baseCount;
                        }
                    }
                }
            }

            for (int i = 0; i < config.BaseIdList.Length; i++)
            {
                int baseCount = (int)(killCount * burstRise / (config.BaseRateList[i]));

                if (baseCount > 0)
                {
                    int baseId = config.BaseIdList[i];
                    if (!dropDict.ContainsKey(baseId))
                    {
                        dropDict[baseId] = 0;
                    }

                    dropDict[baseId] += baseCount;
                }
            }

            var dd = dropDict.OrderByDescending(m => m.Key);

            Dictionary<int, long> recoveryDict = new Dictionary<int, long>();

            foreach (var sp in dd)
            {
                int count = sp.Value;

                DropBaseConfig baseConfig = DropBaseConfigCategory.Instance.Get(sp.Key);

                int ic = baseConfig.ItemIdList.Length;
                int ec = count / ic;

                if (baseConfig.ItemType == (int)ItemType.Equip)
                {
                    //保留橙色，其他全部自动回收
                    int keepCount = EquipConfigCategory.Instance.GetOfflineKeepCount(count);

                    if (keepCount > 0)
                    {
                        keepCount = Math.Min(keepCount, 10);

                        //保留粉色装备
                        for (int k = 0; k < keepCount; k++)
                        {
                            int index = RandomHelper.RandomNumber(0, baseConfig.ItemIdList.Length);
                            int equipId = baseConfig.ItemIdList[index];

                            Item equip = EquipConfigCategory.Instance.BuildOfflineEquip(equipId, 5);
                            itemList.Add(equip);
                        }
                    }

                    long price = EquipConfigCategory.Instance.Get(baseConfig.ItemIdList[0]).Price;
                    gold += (long)((count - keepCount) * price); //期望回收收益为17000
                }
                else if (baseConfig.ItemType == (int)ItemType.EquipSpeical)
                {
                    for (int k = 0; k < ic; k++)
                    {
                        int equipId = baseConfig.ItemIdList[k];
                        Equip_Special equip = ItemHelper.BuildItem(ItemType.EquipSpeical, equipId, qualityRise, 1) as Equip_Special;

                        gold += equip.ToRecoverDict(recoveryDict, ec);
                    }
                }
                else if (baseConfig.ItemType == (int)ItemType.Pet)
                {
                    //宠物,保留橙色，其他全部自动回收
                    int keepCount = PetAtrConfigCategory.Instance.GetOfflineKeepCount(count);
                    if (keepCount > 0)
                    {
                        int petId = baseConfig.ItemIdList[0];
                        //保留宠物
                        for (int k = 0; k < keepCount; k++)
                        {
                            Pet pet = PetAtrConfigCategory.Instance.BuildPet(petId, 0, qualityRise);
                            itemList.Add(pet);
                        }
                    }

                    gold += (long)((count - keepCount) * 17000); //期望回收收益为17000
                }
                else
                {
                    for (int k = 0; k < ic; k++)
                    {
                        itemList.Add(ItemHelper.BuildItem((ItemType)baseConfig.ItemType, baseConfig.ItemIdList[k], qualityRise, ec));
                    }
                }
            }

            foreach (var sp in recoveryDict)
            {
                itemList.Add(ItemHelper.BuildItem(ItemType.Metal, sp.Key, qualityRise, sp.Value));
            }

            return itemList;
        }

        private void BuildOfflineMine(User user, long mineTime, ref string message)
        {
            long runTime = 1; // (long)(ConfigHelper.Mine_Time * 100 / (100 + user.AttributeBonus.CalPanelSingleAttr(AttributeEnum.MetailFinal)));

            runTime = Math.Max(runTime, 6);

            long count = mineTime / runTime;

            if (count <= 0)
            {
                return;
            }

            //miner
            Dictionary<int, int> offlineMetal = MineConfigCategory.Instance.BuildMetal(ref user.MinerSeed, count);

            var sortedDict = offlineMetal.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            message += $"\n离线挖矿收益";
            foreach (var kp in sortedDict)
            {
                var md = user.MetalData;
                int key = kp.Key;
                if (!md.ContainsKey(key))
                {
                    md[key] = new MagicData();
                }

                md[key].Data += kp.Value;

                MetalConfig metalConfig = MetalConfigCategory.Instance.Get(key);

                message += $"<color=#{QualityConfigHelper.GetQualityColor(metalConfig.Quality)}>[{metalConfig.Name}]</color>" + kp.Value + "个";
            }
        }

        private List<Item> AddGoldenEquip()
        {
            //定制红
            List<Item> list = new List<Item>();

            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色战
            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色法
            //list.Add(ItemHelper.BuildEquip(22205801, 6, 1, 0)); //红色道

            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色战
            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色法
            //list.Add(ItemHelper.BuildEquip(22205802, 7, 1, 0)); //金色道

            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金战
            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金法
            //list.Add(ItemHelper.BuildEquip(22205802, 8, 1, 0)); //暗金道

            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌战
            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌法
            //list.Add(ItemHelper.BuildEquip(22205802, 9, 1, 0)); //混沌道

            int role = 3; //战士1，法师2，道士3
            int quality = 8; //6红，7金，8暗金，9混沌

            //金色
            for (int i = 0; i < 1; i++)
            {
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5801, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5802, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5803, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5804, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5809, quality, 0)); //
                list.Add(EquipConfigCategory.Instance.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5810, quality, 0)); //
            }


            return list;
        }

        public void TestInfinityDrop()
        {

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < 20; i++)
            {
                user.InfiniteData.GetDropId(10);
                List<int> drops = user.InfiniteData.DropList[0];
                user.InfiniteData.DropList.RemoveAt(0);

                int countT = drops.Where(m => m >= 180032 && m <= 180034).Count();
                if (countT > 0)
                {
                    Debug.Log(i + " drop Talent :" + countT);
                }
            }
        }
    }
}
