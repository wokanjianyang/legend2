using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class SlotBox : MonoBehaviour
    {
        public Image Img_Bg;

        private Com_Box equip;
        public int Part = 0;
        public string Name = "";

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(GameObject prefab)
        {
            //var box = GameObject.Instantiate(prefab, this.transform);
            //baseInfo = box.GetComponent<Com_Box>();
        }

        public void Init(int part)
        {
            this.Part = part;
            this.Img_Bg.sprite = PrefabHelper.Instance().GetEquipBg(part);
        }

        public void SetPart(int part, string name)
        {
            this.Part = part;
            this.Name = name;
        }


        public void Equip(Com_Box equip)
        {
            this.equip = equip;

            Img_Bg.gameObject.SetActive(false);
        }
        public void UnEquip()
        {
            if (this.equip != null)
            {
                Com_Box comItem = this.equip;
                this.equip = null;
                GameObject.Destroy(comItem.gameObject);
            }

            Img_Bg.gameObject.SetActive(true);
        }
        public Com_Box GetEquip()
        {
            return this.equip;
        }
    }
}
