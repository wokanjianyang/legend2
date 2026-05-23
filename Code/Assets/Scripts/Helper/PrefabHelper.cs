using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class PrefabHelper
    {
        public GameObject ComBoxEmpty = null;
        public GameObject ComBoxDefault = null;
        public GameObject ComBoxEquip = null;

        private List<Sprite> ComBoxList = new List<Sprite>();

        //private List<GameObject> ComBoxList = new List<GameObject>();
        private GameObject Pet_Forge_Box_Prefab = null;
        private GameObject BoxDropPrefab = null;

        private GameObject Message_Prefab = null;
        private GameObject DropMessage_Prefab = null;

        private List<GameObject> PlayerList = new List<GameObject>();

        private List<Sprite> BoxImageList = new List<Sprite>();
        private Dictionary<int, Sprite> FashionList = new Dictionary<int, Sprite>();
        private List<Sprite> ValetList = new List<Sprite>();
        private Dictionary<int, Sprite> MonsterList = new Dictionary<int, Sprite>();
        private List<Sprite> MonsterWorldList = new List<Sprite>();

        private Dictionary<int, Sprite> EquipBgList = new Dictionary<int, Sprite>();

        private Dictionary<int, Sprite> SkillLogoList = new Dictionary<int, Sprite>();

        private Dictionary<int, Sprite> EquipLogoList = new Dictionary<int, Sprite>();

        private Dictionary<int, Sprite> LegacyLogoList = new Dictionary<int, Sprite>();

        private Dictionary<int, Sprite> PetBgList = new Dictionary<int, Sprite>();

        private Sprite MonsterDefend = null;

        private static PrefabHelper instance = null;

        public static PrefabHelper Instance()
        {
            if (instance == null)
            {
                instance = new PrefabHelper();
            }

            return instance;
        }

        public PrefabHelper()
        {
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Hero"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Pet"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_1"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_2"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_3"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_4"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_5"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_6"));

            ComBoxEmpty = Resources.Load<GameObject>("Prefab/Window/Bag/Box_Empty");
            ComBoxDefault = Resources.Load<GameObject>("Prefab/Window/Bag/Box_Default");
            ComBoxEquip = Resources.Load<GameObject>("Prefab/Window/Bag/Box_Equip");

            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box1"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box2"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box3"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box4"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box5"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box6"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box7"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box8"));
            ComBoxList.Add(Resources.Load<Sprite>("UI/Bag/Box9"));

            for (int i = 1; i <= 10; i++)
            {
                EquipBgList.Add(i, Resources.Load<Sprite>("UI/Equip/Equip" + i));
            }

            for (int i = 1001; i <= 1004; i++)
            {
                EquipBgList.Add(i, Resources.Load<Sprite>("UI/Equip/Equip" + i));
            }

            for (int i = 15; i <= 20; i++)
            {
                EquipBgList.Add(i, Resources.Load<Sprite>("UI/Equip/Equip1"));
            }

            Pet_Forge_Box_Prefab = Resources.Load<GameObject>("Prefab/Window/Pet/Pet_Forge_Box");

            BoxDropPrefab = Resources.Load<GameObject>("Prefab/Window/GameItem/Box_Drop");

            Message_Prefab = Resources.Load<GameObject>("Prefab/Dialog/Msg");
            DropMessage_Prefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box1"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box2"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box3"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box4"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box5"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box6"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box7"));
            BoxImageList.Add(Resources.Load<Sprite>("UI/Bag/Box8"));

            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet1"));
            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet2"));
            ValetList.Add(Resources.Load<Sprite>("UI/Player/Player_Valet3"));

            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World1"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World2"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World3"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World4"));
            MonsterWorldList.Add(Resources.Load<Sprite>("UI/Player/Player_World5"));

            MonsterDefend = Resources.Load<Sprite>("UI/Player/Player_Defend");
        }

        public GameObject GetPlayer(PlayerType cam, int quality)
        {
            if (cam == PlayerType.Hero)
            {
                return PlayerList[0];
            }
            else if (cam == PlayerType.Hero_Pet)
            {
                return PlayerList[1];
            }
            else
            {
                return PlayerList[quality + 1];
            }

        }

        public Sprite GetBoxImage(int quanlity)
        {
            return BoxImageList[quanlity - 1];
        }

        public Com_Box CreateComBox(BoxItem item)
        {
            GameObject box;
            if (item.Item.GetItemType() == ItemType.Equip)
            {
                box = GameObject.Instantiate(ComBoxEquip);
            }
            else
            {
                box = GameObject.Instantiate(ComBoxDefault);
            }

            Com_Box comItem = box.GetComponent<Com_Box>();

            comItem.SetItem(item);

            return comItem;
        }

        public Pet_Forge_Box CreateBoxSelect(Transform parent)
        {
            var go = GameObject.Instantiate(Pet_Forge_Box_Prefab);
            Pet_Forge_Box comItem = go.GetComponent<Pet_Forge_Box>();
            //comItem.SetItem(item, type, cycle);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public Box_Drop CreateBoxDrop(Transform parent, Item item)
        {
            var go = GameObject.Instantiate(BoxDropPrefab);
            Box_Drop comItem = go.GetComponent<Box_Drop>();

            comItem.SetItem(item);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public Box_Drop CreateBoxDrop(Transform parent, string name, int quality, int count)
        {
            var go = GameObject.Instantiate(BoxDropPrefab);
            Box_Drop comItem = go.GetComponent<Box_Drop>();

            comItem.SetItem(name, quality, count);

            comItem.transform.SetParent(parent);
            comItem.transform.localPosition = Vector3.zero;
            comItem.transform.localScale = Vector3.one;

            return comItem;
        }

        public GameObject MessagePrefab()
        {
            return this.Message_Prefab;
        }

        public GameObject DropMessagePrefab()
        {
            return this.DropMessage_Prefab;
        }

        public Sprite GetFashion(int id)
        {
            if (!FashionList.ContainsKey(id))
            {
                FashionList[id] = Resources.Load<Sprite>("UI/Player/Fashion/Fashion" + id);
            }

            return FashionList[id];
        }

        public Sprite GetValet(int id)
        {
            return ValetList[id - 1];
        }


        public Sprite GetMonster(int id)
        {
            if (!MonsterList.ContainsKey(id))
            {
                MonsterList[id] = Resources.Load<Sprite>("UI/Player/Monster/Monster" + id);
            }

            return MonsterList[id];
        }

        public Sprite GetSkillLog(int skillId)
        {
            if (!SkillLogoList.ContainsKey(skillId))
            {
                SkillLogoList[skillId] = Resources.Load<Sprite>("UI/Skill/Logo/" + skillId);
            }

            return SkillLogoList[skillId];
        }

        public Sprite GetEquipLog(int role, int part)
        {
            int key = role * 100 + part;
            if (!EquipLogoList.ContainsKey(key))
            {
                EquipLogoList[key] = Resources.Load<Sprite>("UI/Bag/Equip/" + "Box_Equip_" + role + "_" + part);
            }

            return EquipLogoList[key];
        }

        public Sprite GetLegacyLogo(int role, int part)
        {
            int[] pl = { 1, 2, 3, 4, 5, 7, 9, 10 };
            int p = pl[part - 1];

            if (!LegacyLogoList.ContainsKey(part))
            {
                LegacyLogoList[part] = Resources.Load<Sprite>("UI/Bag/Equip/" + "Box_Equip_" + role + "_" + p);
            }

            return LegacyLogoList[part];
        }

        public Sprite GetMonsterWorld(int id)
        {
            return MonsterWorldList[id - 1];
        }



        public Sprite GetDefend()
        {
            return MonsterDefend;
        }

        public Sprite GetEquipBg(int part)
        {
            if (EquipBgList.ContainsKey(part))
            {
                return EquipBgList[part];
            }
            else
            {
                return EquipBgList[1];
            }
        }
    }
}