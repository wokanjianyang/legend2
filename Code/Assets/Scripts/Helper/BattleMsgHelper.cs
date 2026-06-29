using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class BattleMsgHelper
    {
        public static string BuildMonsterDeadMessage(APlayer monster, double exp, double gold, List<Item> Drops, int burstMul, double killCount)
        {
            string drops = "";

            if (burstMul > 0)
            {
                drops += "<color=#EE4444>连爆+" + burstMul + "</color>";
            }

            burstMul += 1;

            if (exp > 0)
            {
                drops += ",经验+" + StringHelper.FormatNumber(exp * burstMul);
            }

            if (gold > 0)
            {
                drops += ",金币+" + StringHelper.FormatNumber(gold * burstMul);
            }
            if (killCount > 0)
            {
                drops += ",杀敌+" + killCount;
            }

            if (Drops != null && Drops.Count > 0)
            {
                drops += ",掉落";
                foreach (var drop in Drops)
                {
                    string qt = "";
                    if (drop.Temp_Number > 1 || burstMul > 1)
                    {
                        qt = "*" + drop.Temp_Number * burstMul;
                    }

                    drops += $"<color=#{QualityConfigHelper.GetQualityColor(drop.GetQuality())}>[{drop.GetName()}]</color>" + qt;
                }
            }

            string message = $"<color=#{QualityConfigHelper.GetQualityColor(monster.Quality)}>[{monster.Name}]</color><color=white>死亡{drops}</color>";

            return message;
        }

        public static string BuildRewardMessage(string src, long exp, long gold, List<Item> Drops)
        {
            string drops = src + "";

            if (exp > 0)
            {
                drops += ",经验+" + StringHelper.FormatNumber(exp);
            }

            if (gold > 0)
            {
                drops += ",金币+" + StringHelper.FormatNumber(gold);
            }

            if (Drops != null && Drops.Count > 0)
            {
                drops += ",掉落";
                foreach (var drop in Drops)
                {
                    string qt = "";
                    if (drop.Temp_Number > 1)
                    {
                        qt = "*" + drop.Temp_Number + " ";
                    }

                    drops += $"<color=#{QualityConfigHelper.GetQualityColor(drop.GetQuality())}>[{drop.GetName()}]</color>" + qt;
                }
            }

            return drops;
        }

        public static string BuildAutoRecoveryMessage(int equipQuantity, List<Item> itemList, long gold)
        {
            string message = "回收" + equipQuantity + "件装备，获得";

            foreach (Item item in itemList)
            {
                message += item.Temp_Number + "个" + item.GetName() + "，";
            }

            if (gold > 0)
            {
                message += StringHelper.FormatNumber(gold) + "金币";
            }

            return message;
        }

        public static string BuildAutoCardMessage(List<Item> itemList)
        {
            string message = "自动提交图鉴：";

            foreach (Item item in itemList)
            {
                message += $"<color=#{QualityConfigHelper.GetQualityColor(item.GetQuality())}>[{item.GetName()}]</color>" ;
            }

            return message;
        }

        public static string BuildGiftPackMessage(string src, double exp, double gold, List<Item> items)
        {
            string message = $"<color=#{QualityConfigHelper.GetQualityColor(4)}> {src}";
            if (exp > 0)
            {
                message += $"经验{StringHelper.FormatNumber(exp)}";
            }
            if (gold > 0)
            {
                message += $"金币{StringHelper.FormatNumber(gold)}";
            }

            if (items != null)
            {
                foreach (var item in items)
                {
                    message += $",{item.GetName()}*{item.Temp_Number}";
                }
            }

            return message + "</color>";
        }
    }
}