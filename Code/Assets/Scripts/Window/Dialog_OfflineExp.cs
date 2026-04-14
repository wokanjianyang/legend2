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
        [LabelText("离线奖励提示")]
        public Text Txt_Msg;

        [LabelText("领取按钮")]
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

            //items.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Shuye3, 100)); //专属碎片
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
            //items.Add(ItemHelper.BuildItem(ItemType.GiftPack, 22, 1, 1)); //特戒
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
                boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
                boxItem.BoxId = -1;
                user.Bags.Add(boxItem);
            }
        }

        //items.Add(ItemHelper.BuildMaterial(50000105, 10000)); //十五阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000106, 10000)); //十六阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000107, 10000)); //十七阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000108, 10000)); //十八阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000109, 10000)); //十九阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000110, 10000)); //二十阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000111, 10000)); //二一阶装备升阶石
        //items.Add(ItemHelper.BuildMaterial(50000112, 10000)); //二二阶装备升阶石

        //user.SaveItemMeterialCount(1998001, 5); //暗金图鉴青龙
        //user.SaveItemMeterialCount(1998002, 5); //暗金图鉴白虎
        //user.SaveItemMeterialCount(1998003, 5); //暗金图鉴朱雀
        //user.SaveItemMeterialCount(1998004, 5); //暗金图鉴玄武
        //user.SaveItemMeterialCount(1998005, 5); //暗金图鉴麒麟

        //for (int i = 1; i <= 8; i++)
        //{
        //    items.Add(ItemHelper.BuildEquip(21205800 + i, 7, 0, 0));
        //}

        //items.Add(new Equip(21205805, 11010, 11110, 7));
        //items.Add(new Equip(21205807, 11010, 11110, 7));

        //items.Add(new Equip(21105801, 9, 10, 6));
        //items.Add(new Equip(21105802, 9, 10, 6));
        //items.Add(new Equip(21105803, 9, 10, 6));

        //items.Add(ItemHelper.BuildEquip(21105803, 6,0,0));
        //items.Add(ItemHelper.BuildEquip(21105804, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105805, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105805, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105807, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105807, 6, 0, 0));
        //items.Add(ItemHelper.BuildEquip(21105809, 6, 0, 0));
        //items.Add(new Equip(22005707, 16, 10037, 5));
        //items.Add(new Equip(22005709, 23, 15, 5));
        //items.Add(new Equip(22005710, 23, 15, 5));

        //items.Add(ItemHelper.BuildMaterial(50000009, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000011, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000014, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000015, 30)); //3介升阶红石头
        //items.Add(ItemHelper.BuildMaterial(50000016, 30)); //3介升阶红石头

        //int rc = 1;
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190001, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190002, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190003, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190004, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190005, 1, rc));
        //items.Add(ItemHelper.BuildItem(ItemType.Ring, 190006, 1, rc));

        //int ic = 1;
        //items.Add(ItemHelper.BuildMaterial(8001, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8002, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8003, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8004, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8005, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8006, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8007, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8008, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8009, ic)); //神技
        //items.Add(ItemHelper.BuildMaterial(8010, ic)); //神技

        //int ic = 1;
        //items.Add(ItemHelper.BuildMaterial(8101, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8102, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8103, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8104, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8105, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8106, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8107, ic)); //魂骨
        //items.Add(ItemHelper.BuildMaterial(8108, ic)); //魂骨

        //int ic = 10000;
        //items.Add(ItemHelper.BuildMaterial(60000001, ic)); //攻击宝石
        //items.Add(ItemHelper.BuildMaterial(60000002, ic)); //防御宝石
        //items.Add(ItemHelper.BuildMaterial(60000003, ic)); //生命宝石
        //items.Add(ItemHelper.BuildMaterial(60000004, ic)); //命中宝石
        //items.Add(ItemHelper.BuildMaterial(60000005, ic)); //闪避宝石
        //items.Add(ItemHelper.BuildMaterial(60000006, ic)); //物理宝石
        //items.Add(ItemHelper.BuildMaterial(60000007, ic)); //魔法宝石
        //items.Add(ItemHelper.BuildMaterial(60000008, ic)); //道术宝石
        //items.Add(ItemHelper.BuildMaterial(60000009, ic)); //增伤宝石
        //items.Add(ItemHelper.BuildMaterial(60000010, ic)); //韧性宝石

        private List<Item> TestShengxiao(User user)
        {

            List<Item> dropList = new List<Item>();
            for (int i = 0; i < 3600 * 5 * 24 * 10; i++)
            {
                int quality = BuildQuality();
                int configId = RandomHelper.RandomNumber(7, 25) % 12 + 1;

                dropList.AddRange(BuildReword(4, quality, configId));
            }

            Debug.Log("dropList Count:" + dropList.Count);

            //Debug.Log("shengxiao metail Count:" + AppHelper.TempRecord);

            for (int i = 1; i <= 9; i++)
            {
                int count = dropList.Where(m => m.GetQuality() == i).Count();

                Debug.Log("Quality" + i + " Count:" + count);
            }

            List<Item> recoveryList = user.CheckRecovery(dropList, out long recoveryGold, out int recoveryCount);

            //dropList.AddRange(recoveryList);

            return dropList;
        }

        private double[] DropRateList = { 1, 2, 4, 10, 40 };
        private double[] QualityRateList = { 1, 1.1, 1.2, 1.5, 2 };
        private List<Item> BuildReword(int mapId, int Quality, int NameId)
        {
            MonsterShengxiaoConfig config = MonsterShengxiaoConfigCategory.Instance.Get(mapId);

            int maxQuality = config.Id + 5;

            User user = GameProcessor.Inst.User;

            double exp = (config.Exp * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
            double gold = (config.Gold * (100.0 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

            double dropRate = 400.0 / (user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstIncrea) / 750000.0 + 1) / DropRateList[Quality - 1];
            double qualityRate = (user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea) / 750000.0 + 1) * QualityRateList[Quality - 1];

            //生肖掉落
            List<Item> items = new List<Item>();
            //items.Add(ItemHelper.BuildMaterial(ItemHelper.Specail_Shengxiao, Quality * config.Id));

            //Debug.Log("this dropRate :" + dropRate + "  qualityRate:" + qualityRate + " nameId:" + NameId + " - " + Name + " quality:" + Quality);

            if (RandomHelper.RandomResult(dropRate))
            {
                //AppHelper.TempRecord++;
                //Debug.Log("shengxiao count:" + AppHelper.TempRecord);
                //生肖
                items.Add(ShengxiaoConfigCategory.Instance.Build(NameId, qualityRate, maxQuality, 0));
            }

            double rs = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            int itemCount = MathHelper.RandomBurstMul(rs);

            if (itemCount > 0)
            {
                exp += exp * itemCount;
                gold += gold * itemCount;
                items.AddRange(ItemHelper.BurstMul(items, itemCount, qualityRate, RuleType.Normal, maxQuality));
            }

            AppHelper.TempRecord += Quality * config.Id * (1 + itemCount);

            return items;
        }

        private int BuildQuality()
        {
            int rd = RandomHelper.RandomNumber(1, 501);
            if (rd > 499)
            {
                return 5;
            }
            else if (rd > 495)
            {
                return 4;
            }
            else if (rd > 480)
            {
                return 3;
            }
            else if (rd > 400)
            {
                return 2;
            }
            else
            {
                return 1;
            }
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

        public void ShowOffline()
        {
            User user = GameProcessor.Inst.User;

            long currentTick = TimeHelper.ClientNowSeconds();
            long offlineTime = currentTick - user.SecondExpTick;

            long tempTime = Math.Min(offlineTime, ConfigHelper.MaxOfflineTime);

            List<Item> items = new List<Item>();
            long rewardExp = 0;
            long rewardGold = 0;

            string OfflineMessage = "离线时间" + offlineTime + "S";
            if (tempTime < offlineTime)
            {
                OfflineMessage += "，实际计算" + tempTime + "S)";
            }
            OfflineMessage += "\n";

            if (user.OffLineMapId > 0)
            {
                //离线暗殿
                items.AddRange(BuildOfflineAndian(user, tempTime, ref rewardExp, ref rewardGold, ref OfflineMessage));
            }

            //离线经验，金币
            long exp = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondExp) * (offlineTime / 5);
            long gold = user.AttributeBonus.GetTotalAttr(AttributeEnum.SecondGold) * (offlineTime / 5);

            OfflineMessage += "\n离线秒收金币" + StringHelper.FormatNumber(gold) + "，经验" + StringHelper.FormatNumber(exp) + "\n";

            //离线挖矿
            this.BuildOfflineMine(user, tempTime, ref OfflineMessage);

            //测试道具
            this.TestSend(user);

            user.AddExpAndGold(exp + rewardExp, gold + rewardGold);
            user.SecondExpTick = currentTick;
            user.MinerTime = currentTick;

            foreach (var item in items)
            {
                if (item.Type == ItemType.Card || item.Type == ItemType.Fashion || item.Type == ItemType.Spirit || (item.Type == ItemType.Material && item.ConfigId == ItemHelper.SpecialId_Card_Stone))
                {
                    user.SaveItemMeterialCount(item.ConfigId, item.Count);
                }
                else
                {
                    BoxItem boxItem = user.Bags.Find(m => !m.IsFull() && m.Item.Type == item.Type && m.Item.ConfigId == item.ConfigId);  //ͬ

                    if (boxItem != null)
                    {
                        boxItem.AddStack(item.Count);
                    }
                    else
                    {
                        boxItem = new BoxItem();
                        boxItem.Item = item;
                        boxItem.MagicNubmer.Data = Math.Max(1, item.Count);
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
                user.HeroPhatomData.Refresh();
                user.PillTime.Check(user.Cycle.Data);
                user.RedRefreshCount.Data = 0;

                int BabelCount = ConfigHelper.BabelCount;
                if (user.BabelData.Data < 10000)
                {
                    BabelCount = ConfigHelper.BabelCount + 200;
                }
                else if (user.BabelData.Data < 20000)
                {
                    BabelCount = ConfigHelper.BabelCount + 100;
                }

                user.BabelCount.Data = BabelCount;

                user.DataDate = DateTime.Now.Ticks;
                //保存到Tap
            }
            user.WorldData.Check();
            GameProcessor.Inst.SaveData();

            this.gameObject.SetActive(true);

            this.Txt_Msg.text = OfflineMessage;

            //Time.timeScale = 0;
        }


        private List<Item> BuildOfflineSpirit(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message)
        {
            MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

            List<Item> itemList = new List<Item>();

            int mapId = user.SpiritOfflineLog[1];
            int time = user.SpiritOfflineLog[2] + 30;
            int total = user.SpiritOfflineLog[3];

            long count = offlineTime / time;

            SpiritCopyConfig config = SpiritCopyConfigCategory.Instance.Get(mapId);

            message += "\n离线英灵副本(" + config.MapName + ")，计算为通关次数：" + count + "，获得";

            List<SpiritDropConfig> dropList = SpiritDropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.MapId == mapId).ToList();

            //Debug.Log("spirit drop list :" + dropList.Count);

            IDictionary<int, int> dropDict = new Dictionary<int, int>();

            int rate = 1 + Math.Min(4, total / 500);

            foreach (SpiritDropConfig sdpConfig in dropList)
            {
                long dropCount = sdpConfig.DropRate * rate * count / 100;

                DropConfig dropConfig = DropConfigCategory.Instance.Get(sdpConfig.DropId);

                for (int i = 0; i < dropCount; i++)
                {
                    int index = i % dropConfig.ItemIdList.Length;

                    int dropId = dropConfig.ItemIdList[index];

                    if (!dropDict.ContainsKey(dropId))
                    {
                        dropDict[dropId] = 0;
                    }

                    dropDict[dropId]++;
                }
            }

            dropDict.OrderBy(m => m.Key);

            foreach (var sp in dropDict)
            {
                itemList.Add(ItemHelper.BuildItem(ItemType.Spirit, sp.Key, 0, sp.Value));

                ItemConfig spiritConfig = ItemConfigCategory.Instance.Get(sp.Key);

                message += $"，<color=#{QualityConfigHelper.GetQualityColor(spiritConfig.Quality)}>[" + spiritConfig.Name + "]</color>" + sp.Value + "个";
            }

            return itemList;
        }

        private List<Item> BuildOfflineAndian(User user, long offlineTime, ref long rewardExp, ref long rewardGold, ref string message)
        {
            MonsterModelConfig modelConfig = MonsterModelConfigCategory.Instance.Get(1); //暗殿

            List<Item> itemList = new List<Item>();

            long killCountFrom = (long)(offlineTime * 2.5);
            //long realKillCount = (long)(killCount * modelConfig.CountRate);

            double lossRate = 1.1; //损失系数,只有 1/1.1 倍的掉落收益

            double realRate = user.GetRealDropRate() * modelConfig.DropRate;
            double qualityRate = (100 + (int)user.AttributeBonus.GetTotalAttr(AttributeEnum.QualityIncrea)) / 100;
            double realQualityRate = 1 + Math.Log(qualityRate, 13);
            long soulPercent = user.AttributeBonus.GetTotalAttr(AttributeEnum.SoulPercent);
            //Debug.Log("realRate:" + realRate);
            //Debug.Log("qualityRate:" + qualityRate);
            //Debug.Log("realQualityRate:" + realQualityRate);

            int mapId = Math.Max(MapConfigCategory.Instance.GetMinMapId(), user.OffLineMapId);
            mapId = Math.Min(MapConfigCategory.Instance.GetMaxMapId(), mapId);

            MapConfig mapConfig = MapConfigCategory.Instance.Get(mapId);

            MonsterBase monster = MonsterBaseCategory.Instance.GetByMapId(mapId);

            long burstMul = user.AttributeBonus.GetTotalAttr(AttributeEnum.BurstMul);
            long killCount = killCountFrom * (100 + burstMul) / 100;

            message += "\n离线未知暗殿(" + mapConfig.Name + ")，击杀了" + killCountFrom + "(连爆计算为" + killCount + ")个怪物，获得";

            long gold = (long)(monster.Gold * killCount * modelConfig.RewardRate * ((100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100));
            long exp = (long)(monster.Exp * killCount * modelConfig.RewardRate * ((100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100));

            //Debug.Log("monster:" + monster.Name);

            message += "，金币" + StringHelper.FormatNumber(gold) + "，经验" + StringHelper.FormatNumber(exp);

            rewardExp += exp;
            rewardGold += gold;

            //炼魂
            int soulRise = 0;
            if (soulPercent > 0)
            {
                soulRise = user.SoulRingNumber + user.GetArtifactValue(ArtifactType.SoulStone);
                soulRise = (int)(killCount * soulRise * soulPercent * modelConfig.DropRate / 100);
                if (soulRise > 0)
                {
                    itemList.Add(ItemHelper.BuildSoulRingShard(soulRise));
                    message += ",炼魂:<color=#FF6600>魂环碎片</color>*" + soulRise;
                }
            }

            int skillBox = 0;

            for (int i = 0; i < mapConfig.DropIdList.Count(); i++)
            {
                int dropId = mapConfig.DropIdList[i];
                DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                double dropRate = Math.Max(lossRate, mapConfig.DropRateList[i] * lossRate / realRate);

                double killRecord = user.GetKillRecord(dropId);
                int dropCount = MathHelper.CalOfflineDropCount(killRecord, killCount, dropRate);

                if (dropCount > 0)
                {
                    if (dropConfig.ItemType == (int)ItemType.Equip)
                    {   //Auto Recovery
                        if (dropConfig.Id <= 110)
                        {
                            //四格
                            int layer = dropConfig.Id - 100;
                            int baseQuantity = (int)(Math.Pow(2, layer));
                            int speicaStone = dropCount * baseQuantity;
                            itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Equip_Speical_Stone, speicaStone));
                            message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"四格碎片"}]</color>" + speicaStone + "个";

                            //Debug.Log(dropCount + "个四格->" + speicaStone + "个四格碎片");
                        }
                        else
                        {
                            int refineStone = (int)(dropCount * MathHelper.CalRefineStone(mapConfig.DropLevel, user.StoneNumber + user.GetArtifactValue(ArtifactType.RefineStone)) * realQualityRate);
                            itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_EquipRefineStone, refineStone));
                            message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"精炼石"}]</color>" + StringHelper.FormatNumber(refineStone) + "个";

                            //Debug.Log(dropCount + "个装备->" + refineStone + "个精炼石");
                        }
                    }
                    else if (dropConfig.ItemType == (int)ItemType.Exclusive)
                    {
                        int exclusiveStone = (int)(dropCount * realQualityRate);
                        itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Exclusive_Stone, exclusiveStone));
                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"专属碎片"}]</color>" + dropCount + "个";

                        //Debug.Log(dropCount + "个专属->" + exclusiveStone + "个专属精华");
                    }
                    else if (dropConfig.ItemType == (int)ItemType.SkillBox)
                    {
                        skillBox += dropCount * dropConfig.Level / 50;

                    }
                    else
                    {
                        for (int d = 0; d < dropCount; d++)
                        {
                            int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                            itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, dropConfig.ItemIdList[di], 1, 1));
                        }
                        message += $"，<color=#{QualityConfigHelper.GetQualityColor(6)}>[{dropConfig.Name}]</color>" + dropCount + "个";
                    }
                }

                user.SaveKillRecord(dropId, killCount);
            }

            //-------书页汇总-----------
            if (skillBox > 0)
            {
                itemList.Add(ItemHelper.BuildMaterial(ItemHelper.SpecialId_Shuye1, skillBox));
                message += $"，<color=#{QualityConfigHelper.GetQualityColor(3)}>[{"书页"}]</color>" + skillBox + "个";
            }

            List<DropLimitConfig> limits = DropLimitConfigCategory.Instance.GetByMapId((int)DropLimitType.Map, mapId);

            int cardCount = 0;
            int fashionCount = 0;

            string limitMessage = "";
            for (int i = 0; i < limits.Count(); i++)
            {
                DropLimitConfig limitConfig = limits[i];
                int dropId = limitConfig.DropId;
                //Debug.Log("drop Limit Id:" + limitConfig.DropId);

                double dr = limitConfig.ShareRise > 0 ? realRate : 1 * modelConfig.CountRate; //吃爆率用爆率，不吃爆率用数量
                double dropRate = Math.Max(lossRate, (limitConfig.StartRate + limitConfig.Rate) * lossRate / dr);

                //Debug.Log("dropRate:" + dropRate);

                double killRecord = user.GetKillRecord(dropId);
                int dropCount = MathHelper.CalOfflineDropCount(killRecord, killCount, dropRate);

                if (dropCount > 0)
                {
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(limitConfig.DropId);

                    if (dropConfig.ItemType == (int)ItemType.Equip)
                    {   //Auto Recovery
                        //message += "," + dropCount + "个" + limitConfig.Name;

                        for (int d = 0; d < dropCount; d++)
                        {
                            int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                            itemList.Add(ItemHelper.BuildEquip(dropConfig.ItemIdList[di], 0, 1, TimeHelper.TodaySeed()));
                        }
                    }
                    else
                    {
                        for (int d = 0; d < dropCount; d++)
                        {
                            int di = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                            itemList.Add(ItemHelper.BuildItem((ItemType)dropConfig.ItemType, dropConfig.ItemIdList[di], 1, 1));
                        }
                    }

                    if (dropConfig.ItemType == (int)ItemType.Card)
                    {
                        cardCount += dropCount;
                    }
                    else if (dropConfig.ItemType == (int)ItemType.Fashion)
                    {
                        fashionCount += dropCount;
                    }
                    else
                    {
                        int q = limitConfig.Id > 1000 ? 6 : 5;

                        limitMessage += $"，<color=#{QualityConfigHelper.GetQualityColor(q)}>[{limitConfig.Name}]</color>" + dropCount + "个";
                    }

                    //Debug.Log("drop limit " + killRecord + "-" + (killRecord + killCount) + " 掉落" + dropCount + "个" + limitConfig.Name);
                }

                user.SaveKillRecord(dropId, killCount);
            }

            if (cardCount > 0)
            {
                message += $"，<color=#{QualityConfigHelper.GetQualityColor(4)}>[{"图鉴"}]</color>" + cardCount + "个";
            }
            if (fashionCount > 0)
            {
                message += $"，<color=#{QualityConfigHelper.GetQualityColor(5)}>[{"时装"}]</color>" + fashionCount + "个";
            }

            message += limitMessage + "\n";

            return itemList;
        }

        private void BuildOfflineMine(User user, long mineTime, ref string message)
        {
            long runTime = (long)(ConfigHelper.Mine_Time * 100 / (100 + user.AttributeBonus.CalBaseAttr(AttributeEnum.MetailFinal)));

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

        private List<Equip> AddGoldenEquip()
        {
            //定制红
            List<Equip> list = new List<Equip>();

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
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5801, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5802, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5803, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5804, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5805, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5807, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5809, quality, 1, 0)); //
                list.Add(ItemHelper.BuildEquip(20000000 + role * 1000000 + (quality - 5) * 100000 + 5810, quality, 1, 0)); //
            }


            return list;
        }

        private void Test(User user)
        {
            for (int count = 1; count <= 10; count++)
            {
                if (user.InfiniteData.DropList.Count > 0)
                {
                    user.InfiniteData.DropList.RemoveAt(0);
                }
                user.InfiniteData.GetDropId(1);

                List<int> dropList = user.InfiniteData.DropList[0];

                for (int i = 1; i < dropList.Count; i++)
                {
                    if (dropList[i - 1] == 4002)
                    {
                        //Debug.Log(count + "次 - " + i + "层 掉落魂骨");
                    }
                    else if (dropList[i - 1] >= 180001 && dropList[i - 1] <= 180101)
                    {
                        Debug.Log(count + "次 - " + i + "层 掉落法宝");
                    }
                }
            }
        }
    }
}
