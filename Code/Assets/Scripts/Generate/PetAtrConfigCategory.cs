using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetAtrConfigCategory
    {
        public Pet BuildPet(int id, int role, int quality)
        {
            if (quality == 0)
            {
                quality = RandomHelper.RandomNumber(1, 6);
            }
            if (role == 0)
            {
                role = RandomHelper.RandomNumber(1, 4);
            }

            Pet pet = new Pet(id, role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;
            pet.Quality = quality;


            List<KeyValuePair<int, int>> flairs = BuildPetFlair(role, quality);

            //资质紫色1，橙色2，红色3，金色4
            foreach (var flair in flairs)
            {
                int attrId = flair.Key;
                MagicData attrValue = new MagicData();
                attrValue.Data = flair.Value;

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            //技能紫色1，橙色2，红色3，金色4
            List<KeyValuePair<int, int>> skills = BuildPetSkill(role, pet.Quality);
            foreach (var skill in skills)
            {
                int skillId = skill.Key;
                MagicData skillLevel = new MagicData();
                skillLevel.Data = skill.Value;

                pet.Skills.Add(new KeyValuePair<int, MagicData>(skillId, skillLevel));
            }

            //技能橙色1，红色2，金色3
            if (quality >= 4)
            {
                List<int> talents = BuildPetTalents(skills.Select(m => m.Key).ToList(), pet.Quality);
                foreach (var telent in talents)
                {
                    pet.Talents.Add(telent);
                }
            }

            return pet;
        }

        public Pet BuildByPack(int configId)
        {

            GiftPackPet config = GiftPackPetCategory.Instance.Get(configId);

            Pet pet = new Pet(config.Mid, config.Role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;
            pet.Quality = config.AttrIdList.Length;

            //杀敌资质
            for (int i = 0; i < config.AttrIdList.Length; i++)
            {
                int attrId = config.AttrIdList[i];
                MagicData attrValue = new MagicData();
                attrValue.Data = config.AttrValueList[i];

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            //自带技能
            for (int i = 0; i < config.SkillList.Length; i++)
            {
                int skillId = config.SkillList[i];
                MagicData skillLevel = new MagicData();
                skillLevel.Data = config.SkillLevelList[i];

                pet.Skills.Add(new KeyValuePair<int, MagicData>(skillId, skillLevel));
            }

            //技能天赋
            for (int i = 0; i < config.TalentList.Length; i++)
            {
                pet.Talents.Add(config.TalentList[i]);
            }


            return pet;
        }

        private int GetFlairCount(int quality)
        {
            return Math.Max(1, quality - 3);
        }

        private int GetTalentCount(int quality)
        {
            return Math.Max(1, quality - 4);
        }

        private List<KeyValuePair<int, int>> BuildPetFlair(int role, int quality)
        {
            List<PetAtrConfig> configs = this.list.Where(m => (m.Role == 0 || m.Role == role) && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            int count = GetFlairCount(quality);

            for (int i = 1; i <= count; i++)
            {
                List<int> excludeList = GetExcludeList(configs, flairs);

                List<PetAtrConfig> temps = configs.Where(m => !excludeList.Contains(m.Id)).ToList();
                int index = RandomHelper.RandomNumber(0, temps.Count);

                PetAtrConfig config = temps[index];

                int attrValue = RandomHelper.RandomSerialNumber(config.MinValue, config.MaxValue);

                flairs.Add(new KeyValuePair<int, int>(config.Id, attrValue));
            }

            return flairs;
        }

        private List<int> GetExcludeList(List<PetAtrConfig> configs, List<KeyValuePair<int, int>> rsList)
        {
            List<int> excludeList = new List<int>();

            foreach (PetAtrConfig config in configs)
            {
                int count = rsList.Where(m => m.Key == config.Id).Count();

                if (count >= config.MaxCount)
                {
                    excludeList.Add(config.Id);

                    //Debug.Log("Exclued id :" + config.Id + " count:" + count);
                }
            }
            return excludeList;
        }

        private List<KeyValuePair<int, int>> BuildPetSkill(int role, int quality)
        {
            List<SkillConfig> configs = SkillConfigCategory.Instance.GetAllByRole(role);

            int count = GetFlairCount(quality);

            List<KeyValuePair<int, int>> skills = new List<KeyValuePair<int, int>>();
            List<int> ids = new List<int>();


            for (int i = 1; i <= count; i++)
            {
                List<SkillConfig> temps = configs.Where(m => !ids.Contains(m.SkillId)).ToList();

                int index = RandomHelper.RandomNumber(1, temps.Count + 1);

                SkillConfig config = temps[index - 1];

                int level = 11 - RandomHelper.RandomPowNumber(1, 10) + quality / 2;

                skills.Add(new KeyValuePair<int, int>(config.SkillId, level));
                ids.Add(config.SkillId);
            }

            return skills;
        }

        private List<int> BuildPetTalents(List<int> skills, int quality)
        {
            List<SkillTalentConfig> configs = SkillTalentConfigCategory.Instance.GetSkillAllConfigs(skills);

            int count = GetTalentCount(quality);

            List<int> talents = new List<int>();
            List<int> ids = new List<int>();

            for (int i = 1; i <= count; i++)
            {
                List<SkillTalentConfig> temps = configs.Where(m => !ids.Contains(m.Id) && skills.Contains(m.SkillId)).ToList();
                int index = RandomHelper.RandomNumber(1, temps.Count + 1);

                SkillTalentConfig config = temps[index - 1];

                talents.Add(config.Id);
                ids.Add(config.Id);
            }

            return talents;
        }


        public long GetPetFee(long level)
        {
            return 1000 + (level - 1) * 100;
        }

        public long GetFeeTotal(long level)
        {
            long total = 0;
            for (int i = 1; i < level; i++)
            {
                total += GetPetFee(i);
            }
            return total;
        }

        public int GetPetLayerFee(long layer)
        {
            return (int)Math.Min((4 + layer), 10);
        }

        public long GetPetTotalFee(long layer)
        {
            long total = 0;

            for (int i = 2; i <= layer; i++)
            {
                int fee = PetAtrConfigCategory.Instance.GetPetLayerFee(i - 1);
                total += fee;
            }

            return total;
        }

        public int GetPetLayerFeeTotal(long layer)
        {
            int total = 0;

            for (int i = 1; i < layer; i++)
            {
                total += GetPetLayerFee(i);
            }
            return total;
        }
    }
}
