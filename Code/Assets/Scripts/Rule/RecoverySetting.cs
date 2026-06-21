namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RecoverySetting
    {
        public int SetTotal { get; set; } = 0;

        //普通装备
        public int EquipQualityKeep { get; set; } = 0;
        public int GoldTotal { get; set; } = 0;

        public int ExpTotal { get; set; } = 0;

        public int LuckyTotal { get; set; } = 0;

        public int CurseTotal { get; set; } = 0;

        public int SpeedTotal { get; set; } = 0;

        public int CdTotal { get; set; } = 0;

        public int DropRate { get; set; } = 0;

        public int DropQuality { get; set; } = 0;

        public int EquipQualityRecovery { get; set; } = 0;

        public int EquipLevel { get; set; } = 0;

        public Dictionary<int, bool> EquipRole { get; private set; } = new Dictionary<int, bool>();


        //传奇装备
        public int LegendLevel { get; set; } = 0;

        //其他回收
        public int SpecailLevel { get; set; } = 0;

        public int PetQuality { get; set; } = 0;

        private int KeepStartQuality = 3;


        public RecoverySetting()
        {

        }

        public bool CheckRecovery(Item item, RecoveryType type)
        {
            if (item.IsLock)
            {
                return false;
            }

            if (item.GetItemType() == ItemType.Equip)
            {
                Equip equip = item as Equip;

                if (equip.Layer > 1)
                {
                    return false;
                }

                int role = equip.Config.Role;
                int cycle = equip.Config.Cycle;
                int level = equip.Level;
                int quality = equip.GetQuality();
                long ar = equip.GetAttrRateCount();
                bool keepSkill = false;
                if (equip.SkillSuitConfig != null)
                {
                    //keepSkill = User_Data_Manager.Data.CheckKeepSkill(equip.SkillRuneConfig.SkillId, equip.SkillRuneConfig.SkillLayer);
                }


                if (cycle == 1)
                {
                    //普通回收

                    //先判断保留
                    if (EquipQualityKeep > 0 && quality >= EquipQualityKeep + KeepStartQuality)
                    {
                        if (GoldTotal > 0)
                        {
                            long gt = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.GoldIncrea).Select(m => m.Value).Sum();
                            if (gt >= GoldTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (ExpTotal > 0)
                        {
                            long et = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.ExpIncrea).Select(m => m.Value).Sum();
                            if (et >= ExpTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (LuckyTotal > 0)
                        {
                            long lucky = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Lucky).Select(m => m.Value).Sum();
                            if (lucky >= LuckyTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (CurseTotal > 0)
                        {
                            long curse = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Curse).Select(m => m.Value).Sum();
                            if (curse >= LuckyTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (CurseTotal > 0)
                        {
                            long speed = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Speed).Select(m => m.Value).Sum();
                            if (speed >= SpeedTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (CurseTotal > 0)
                        {
                            long cd = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Cd).Select(m => m.Value).Sum();
                            if (cd >= CdTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }
                        if (DropRate > 0)
                        {
                            long rateTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.BurstIncrea).Select(m => m.Value).Sum();
                            if (rateTotal >= DropRate)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (DropQuality > 0)
                        {
                            long qualityTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.QualityIncrea).Select(m => m.Value).Sum();
                            if (qualityTotal >= DropQuality)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (equip.SkillSuitConfig != null)
                        {
                            if (item.Level >= EquipLevel && keepSkill)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                    }

                    if (equip.Level < EquipLevel || EquipRole.GetValueOrDefault(role, false) || quality <= EquipQualityRecovery)
                    {
                        return true;
                    }
                }
                else if (cycle == 10)
                {
                    if (LegendLevel > 0 && equip.Config.LevelRequired < LegendLevel)
                    {
                        return true;
                    }
                }
            }
            else if (item.GetItemType() == ItemType.EquipSpeical)
            {
                //四格回收
                if (item.Layer < SpecailLevel)
                {
                    return true;
                }
            }
            else if (item.GetItemType() == ItemType.Pet)
            {
                Pet pet = item as Pet;
                if (item.GetQuality() <= PetQuality && pet.KillCount.Data <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum RecoveryType
    {
        Drop = 1,//打怪掉落
        Other = 2, //其他
    }
}
