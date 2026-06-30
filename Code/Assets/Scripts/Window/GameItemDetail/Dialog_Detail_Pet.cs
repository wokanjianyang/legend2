using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Dialog_Detail_Pet : MonoBehaviour, IBattleLife
    {
        //[LabelText("容器")]
        //public RectTransform rect_Content;

        //[Title("背景图片")]
        //public Image img_Background;
        //public Sprite[] list_BackgroundImgs;

        [LabelText("名称")]
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Exp;
        public Text Txt_Layer;
        public Text Txt_Count;

        [LabelText("特性")]
        public Transform Tf_Trait;
        private List<Pet_Trait> TraitList;

        [LabelText("资质")]
        public Transform Tf_Flair;
        private List<Pet_Flair> FlairList;

        [LabelText("天赋")]
        public Transform Tf_Talent;
        private List<Pet_Talent> TalentList;

        [LabelText("技能")]
        public Transform Tf_Skill;
        private List<Pet_Skill> SkillList;

        [Title("导航")]
        public Button Btn_Equip;
        public Button Btn_UnEquip;
        public Button Btn_Recovery;

        public Button Btn_ToCard;
        public Button Btn_Lock;
        public Button Btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;

        //private RectTransform rectTransform;

        // Start is called before the first frame update
        void Awake()
        {
            this.Btn_Equip.onClick.AddListener(this.OnEquip);
            //this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


            this.Btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.Btn_ToCard.onClick.AddListener(this.OnClick_ToCard);

            this.Btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.Btn_Unlock.onClick.AddListener(this.OnClick_Unlock);

            this.Btn_Close.onClick.AddListener(this.OnClick_Close);

            TraitList = Tf_Trait.GetComponentsInChildren<Pet_Trait>().ToList();
            FlairList = Tf_Flair.GetComponentsInChildren<Pet_Flair>().ToList();
            TalentList = Tf_Talent.GetComponentsInChildren<Pet_Talent>().ToList();
            SkillList = Tf_Skill.GetComponentsInChildren<Pet_Skill>().ToList();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.AddListener<ShowDetailEvent>(this.OnShowEvent);
            //this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEvent(ShowDetailEvent e)
        {
            if (e.Show_Type != ShowType.Pet)
            {
                return;
            }

            this.gameObject.SetActive(true);

            Tf_Flair.gameObject.SetActive(false);
            Tf_Talent.gameObject.SetActive(false);
            Tf_Skill.gameObject.SetActive(false);

            this.Btn_Equip.gameObject.SetActive(false);
            this.Btn_UnEquip.gameObject.SetActive(false);
            this.Btn_Recovery.gameObject.SetActive(false);
            this.Btn_ToCard.gameObject.SetActive(false);
            this.Btn_Lock.gameObject.SetActive(false);
            this.Btn_Unlock.gameObject.SetActive(false);

            this.boxItem = e.Show_Item;

            User user = User_Data_Manager.Data;
            string titleColor = QualityConfigHelper.GetQualityColor(this.boxItem.Item.GetQuality());

            Pet pet = this.boxItem.Item as Pet;

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", titleColor, pet.GetName());
            this.Txt_Level.text = "宠物等级：" + pet.PetLevel.Data;

            long exp = PetAtrConfigCategory.Instance.GetPetFee(pet.PetLevel.Data);
            this.Txt_Exp.text = "Exp：" + pet.LevelExp.Data + "/" + exp;
            this.Txt_Count.text = "杀敌数：" + pet.GetTotalKillCount() + "点";

            for (int i = 0; i < TraitList.Count; i++)
            {
                if (i == 0)
                {
                    TraitList[i].gameObject.SetActive(true);
                    TraitList[0].SetContent(pet.Config.TraitId, pet.Config.TraitLevel, pet.TraitType);
                }
                else
                {
                    if (i <= pet.TraitList.Count)
                    {
                        TraitList[i].gameObject.SetActive(true);
                        TraitList[i].SetContent(pet.TraitList[i - 1].Id, pet.TraitList[i - 1].Level, pet.TraitList[i - 1].Type);
                    }
                    else
                    {
                        TraitList[i].gameObject.SetActive(false);
                    }
                }
            }

            var flairs = pet.Flairs;

            if (flairs != null && flairs.Count > 0)
            {
                Tf_Flair.gameObject.SetActive(true);

                for (int i = 0; i < FlairList.Count; i++)
                {
                    Pet_Flair child = FlairList[i];

                    if (i < flairs.Count())
                    {
                        child.SetContent(flairs[i].Key, flairs[i].Value.Data, pet.GetTotalKillCount());
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            var talents = pet.Talents;
            if (talents != null && talents.Count > 0)
            {
                Tf_Talent.gameObject.SetActive(true);

                for (int i = 0; i < TalentList.Count; i++)
                {
                    Pet_Talent child = TalentList[i];

                    if (i < talents.Count())
                    {
                        child.SetContent(talents[i]);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            var skills = pet.Skills;
            if (skills != null && skills.Count > 0)
            {
                Tf_Skill.gameObject.SetActive(true);

                for (int i = 0; i < SkillList.Count; i++)
                {
                    Pet_Skill child = SkillList[i];

                    if (i < skills.Count())
                    {
                        child.SetContent(skills[i].Key, skills[i].Value.Data);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }

            this.Btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);

            if (e.Box_Type == ComBoxType.Bag)
            {
                //包裹中
                this.Btn_Equip.gameObject.SetActive(true);
                this.Btn_Lock.gameObject.SetActive(!this.boxItem.Item.IsLock);
                this.Btn_Unlock.gameObject.SetActive(this.boxItem.Item.IsLock);

                if (pet.Level > 1) //升阶过的只能重生
                {

                }
                else  //没升阶的只能回收
                {
                    this.Btn_Recovery.gameObject.SetActive(!this.boxItem.Item.IsLock);
                }

                if (pet.GetQuality() == pet.Config.CardQuality && !user.IsCardMax(pet.Config.CardId))
                {
                    this.Btn_ToCard.gameObject.SetActive(!this.boxItem.Item.IsLock);
                }

            }
            else if (e.Box_Type == ComBoxType.OnEquip)
            {
                //装备栏中
                this.Btn_UnEquip.gameObject.SetActive(true);
            }
        }

        private void OnEquip()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new PetBattleUpEvent()
            {
                BoxItem = this.boxItem,
            });
        }

        private void OnClick_ToCard()
        {
            this.gameObject.SetActive(false);

            Pet pet = this.boxItem.Item as Pet;

            GameProcessor.Inst.EventCenter.Raise(new EquipToCardEvent()
            {
                BoxItem = this.boxItem,
                CardId = pet.Config.CardId,
            });
        }

        private void OnRecovery()
        {
            this.gameObject.SetActive(false);

            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能回收", ToastType = ToastTypeEnum.Failure });
                return;
            }

            if (this.boxItem.Item.GetQuality() >= 5)
            {
                GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认回收？", true,
                () =>
                {
                    GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                    {
                        BoxItem = this.boxItem,
                    });
                }, () =>
                {

                });
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new RecoveryEvent()
                {
                    BoxItem = this.boxItem,
                });
            }

        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        public void OnClick_Lock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = true
            });
        }

        private void OnClick_Unlock()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = false
            });
        }
    }
}
