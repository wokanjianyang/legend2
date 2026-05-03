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
        public Text TxtName;
        public Text TxtLevel;
        public Text TxtLayer;
        public Text Txt_Count;

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
        public Button btn_Equip;
        public Button btn_UnEquip;
        public Button btn_Recovery;
        public Button btn_Restore;

        public Button btn_Lock;
        public Button btn_Unlock;

        public Button Btn_Close;

        private BoxItem boxItem;

        //private RectTransform rectTransform;

        // Start is called before the first frame update
        void Awake()
        {
            this.btn_Equip.onClick.AddListener(this.OnEquip);
            //this.btn_UnEquip.onClick.AddListener(this.OnUnEquip);


            this.btn_Recovery.onClick.AddListener(this.OnRecovery);
            this.btn_Restore.onClick.AddListener(this.OnClick_Restore);

            this.btn_Lock.onClick.AddListener(this.OnClick_Lock);
            this.btn_Unlock.onClick.AddListener(this.OnClick_Unlock);

            this.Btn_Close.onClick.AddListener(this.OnClick_Close);

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

            GameProcessor.Inst.EventCenter.AddListener<ShowPetDetailEvent>(this.OnShowEvent);
            //this.rectTransform = this.transform.GetComponent<RectTransform>();
        }

        private void OnShowEvent(ShowPetDetailEvent e)
        {
            this.gameObject.SetActive(true);

            Tf_Flair.gameObject.SetActive(false);
            Tf_Talent.gameObject.SetActive(false);
            Tf_Skill.gameObject.SetActive(false);

            this.btn_Equip.gameObject.SetActive(false);
            this.btn_UnEquip.gameObject.SetActive(false);
            this.btn_Recovery.gameObject.SetActive(false);
            this.btn_Restore.gameObject.SetActive(false);
            this.btn_Lock.gameObject.SetActive(false);
            this.btn_Unlock.gameObject.SetActive(false);

            this.boxItem = e.boxItem;

            string titleColor = QualityConfigHelper.GetColor(this.boxItem.Item);

            Pet pet = this.boxItem.Item as Pet;

            this.TxtName.text = string.Format("<color=#{0}>{1}</color>", titleColor, pet.GetName());
            this.TxtLevel.text = pet.PetLevel.Data + "";
            this.TxtLayer.text = pet.PetLayer.Data + "";
            this.Txt_Count.text = "杀敌数：" + pet.GetTotalKillCount() + "点";

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

            this.btn_Equip.gameObject.SetActive(this.boxItem.BoxId != -1);

            if (!this.boxItem.Item.IsLock)
            {
                if (pet.PetLevel.Data > 1)
                {
                    this.btn_Restore.gameObject.SetActive(this.boxItem.BoxId != -1);
                }
                else
                {
                    this.btn_Recovery.gameObject.SetActive(this.boxItem.BoxId != -1);
                }
            }

            if (this.boxItem.Item.IsLock)
            {
                this.btn_Unlock.gameObject.SetActive(true);
            }
            else
            {
                this.btn_Lock.gameObject.SetActive(true);
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

        private void OnClick_Restore()
        {
            if (this.boxItem.Item.IsLock)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "锁定的不能重生", ToastType = ToastTypeEnum.Failure });
                return;
            }

            GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("重生消耗5000兆金币，其他材料全部返回。是否确认？", true,
                () =>
                {
                    this.gameObject.SetActive(false);
                    GameProcessor.Inst.EventCenter.Raise(new RestoreEvent()
                    {
                        BoxItem = this.boxItem,
                    });
                }, () =>
                {

                });
        }

        private void OnRecovery()
        {
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
                    this.gameObject.SetActive(false);

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
                this.gameObject.SetActive(false);

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
            this.btn_Lock.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = true
            });

            this.btn_Unlock.gameObject.SetActive(true);
        }

        private void OnClick_Unlock()
        {
            this.btn_Unlock.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new EquipLockEvent()
            {
                BoxItem = this.boxItem,
                IsLock = false
            });

            this.btn_Lock.gameObject.SetActive(true);
        }
    }
}
