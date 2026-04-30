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
        private List<GameObject> ComBoxList = new List<GameObject>();
        private GameObject BoxSelectPrefab = null;
        private GameObject BoxDropPrefab = null;

        private GameObject Message_Prefab = null;
        private GameObject DropMessage_Prefab = null;

        private List<GameObject> PlayerList = new List<GameObject>();

        private List<Sprite> BoxImageList = new List<Sprite>();
        private List<Sprite> FashionList = new List<Sprite>();
        private List<Sprite> ValetList = new List<Sprite>();
        private Dictionary<int, Sprite> MonsterList = new Dictionary<int, Sprite>();
        private List<Sprite> MonsterWorldList = new List<Sprite>();

        private Dictionary<int, Sprite> EquipBgList = new Dictionary<int, Sprite>();

        private Dictionary<int, Sprite> SkillLogoList = new Dictionary<int, Sprite>();

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
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_1"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_2"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_3"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_4"));
            PlayerList.Add(Resources.Load<GameObject>("Prefab/Player/Monster_5"));

            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box_Empty"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box1"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box2"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box3"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box4"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box5"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box6"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box7"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box8"));
            ComBoxList.Add(Resources.Load<GameObject>("Prefab/Window/Bag/Box9"));

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

            BoxSelectPrefab = Resources.Load<GameObject>("Prefab/Window/GameItem/BoxSelect");

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

            for (int i = 1; i <= 24; i++)
            {
                FashionList.Add(Resources.Load<Sprite>("UI/Player/Fashion" + i));
            }

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

        public GameObject GetPlayer(int type)
        {
            return PlayerList[type];
        }

        public GameObject GetBoxPrefab(int quanlity)
        {
            return ComBoxList[quanlity];
        }

        public Sprite GetBoxImage(int quanlity)
        {
            return BoxImageList[quanlity - 1];
        }


        public Com_Box CreateComBox(BoxItem item)
        {
            var prefab = GetBoxPrefab(item.Item.GetQuality());
            var box = GameObject.Instantiate(prefab);
            Com_Box comItem = box.GetComponent<Com_Box>();

            comItem.SetItem(item);

            return comItem;
        }

        public Box_Select CreateBoxSelect(Transform parent, BoxItem item, ComBoxType type, int cycle)
        {
            var go = GameObject.Instantiate(BoxSelectPrefab);
            Box_Select comItem = go.GetComponent<Box_Select>();
            comItem.SetItem(item, type, cycle);

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
            return FashionList[id - 1];
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