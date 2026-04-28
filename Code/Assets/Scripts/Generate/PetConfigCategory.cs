using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class PetConfigCategory
    {
        public Pet BuildPet(int role, int quality)
        {
            Pet pet = new Pet(role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;
            pet.Quality = quality;

            List<KeyValuePair<int, int>> flairs = BuildPetFlair(role, quality);

            //杀敌资质
            foreach (var flair in flairs)
            {
                int attrId = flair.Key;
                MagicData attrValue = new MagicData();
                attrValue.Data = flair.Value;

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            //自带技能红色以下，单技能，红色2技能，金色3技能
            List<KeyValuePair<int, int>> skills = BuildPetSkill(role, pet.Quality);
            foreach (var skill in skills)
            {
                int skillId = skill.Key;
                MagicData skillLevel = new MagicData();
                skillLevel.Data = skill.Value;

                pet.Skills.Add(new KeyValuePair<int, MagicData>(skillId, skillLevel));
            }

            //技能天赋，紫色橙色1条天赋，红色2条天赋，金色3条天赋（天赋必属于自带技能当中）
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

            GiftPackPet packPet = GiftPackPetCategory.Instance.Get(configId);

            Pet pet = new Pet(packPet.Role);

            pet.PetLevel.Data = 1;
            pet.PetLayer.Data = 1;
            pet.Quality = packPet.AttrIdList.Length;


            //杀敌资质
            for (int i = 0; i < packPet.AttrIdList.Length; i++)
            {
                int attrId = packPet.AttrIdList[i];
                MagicData attrValue = new MagicData();
                attrValue.Data = packPet.AttrValueList[i];

                pet.Flairs.Add(new KeyValuePair<int, MagicData>(attrId, attrValue));
            }

            //技能天赋


            //自带技能


            return pet;
        }

        private int GetFlairCount(int quality)
        {
            return Math.Max(1, quality - 4);
        }

        private List<KeyValuePair<int, int>> BuildPetFlair(int role, int quality)
        {
            List<PetConfig> configs = this.list.Where(m => (m.Role == 0 || m.Role == role) && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            List<KeyValuePair<int, int>> flairs = new List<KeyValuePair<int, int>>();

            int count = GetFlairCount(quality);

            for (int i = 1; i <= count; i++)
            {
                List<int> excludeList = GetExcludeList(configs, flairs);

                List<PetConfig> temps = configs.Where(m => !excludeList.Contains(m.Id)).ToList();
                int index = RandomHelper.RandomNumber(0, temps.Count);

                PetConfig config = temps[index];

                int attrValue = RandomHelper.RandomSerialNumber(config.MinValue, config.MaxValue);

                flairs.Add(new KeyValuePair<int, int>(config.Id, attrValue));
            }

            return flairs;
        }

        private List<int> GetExcludeList(List<PetConfig> configs, List<KeyValuePair<int, int>> rsList)
        {
            List<int> excludeList = new List<int>();

            foreach (PetConfig config in configs)
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

            int count = GetFlairCount(quality);

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
                int fee = PetConfigCategory.Instance.GetPetLayerFee(i - 1);
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

    public partial class PetConfig
    {
        public long GetAttr(long layer)
        {
            return 0;
        }

        public long GetFee(long layer)
        {
            return 0;
        }


    }

}
