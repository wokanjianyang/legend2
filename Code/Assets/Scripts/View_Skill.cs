using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class View_Skill : AViewPage
    {
        [Title("技能面板")]
        [LabelText("所有技能")]
        public ScrollRect sr_AllSkill;

        [LabelText("装载技能")]
        public Transform tran_EquipSkills;

        public Dialog_Rune_Info Dlg_Rune_Info;

        //public List<Button> PlanList;

        public Transform Tf_Plan;
        private List<Toggle> Toggle_Plan_List = new List<Toggle>();

        private int SelectRole = 1;
        public Transform Tf_Role;
        private List<Toggle> Toggle_Role_List = new List<Toggle>();

        private Item_Skill_Selected[] AllEquipSkills;

        private List<Item_Skill> learnSkills;
        private List<Item_Skill_Selected> equipSkills;
        private GameObject bookPrefab;

        void Awake()
        {
            Toggle_Plan_List = Tf_Plan.GetComponentsInChildren<Toggle>().ToList();
            Toggle_Role_List = Tf_Role.GetComponentsInChildren<Toggle>().ToList();
        }

        void OnEnable()
        {
            this.Refresh();
        }

        private void Refresh()
        {
            if (learnSkills != null)
            {
                foreach (var sp in learnSkills)
                {
                    sp.Show();
                }
            }
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.learnSkills = new List<Item_Skill>();
            this.equipSkills = new List<Item_Skill_Selected>();

            var user = GameProcessor.Inst.User;

            GameProcessor.Inst.EventCenter.AddListener<SkillShowEvent>(OnSkillShow);
            GameProcessor.Inst.EventCenter.AddListener<SkillUpEvent>(OnSkillUp);
            GameProcessor.Inst.EventCenter.AddListener<SkillDownEvent>(OnSkillDown);
            GameProcessor.Inst.EventCenter.AddListener<SkillChangePlanEvent>(OnSkillChangePlan);


            bookPrefab = Resources.Load<GameObject>("Prefab/Skill/Item_Skill");

            this.AllEquipSkills = this.tran_EquipSkills.GetComponentsInChildren<Item_Skill_Selected>();

            this.ShowSkillPanel();
            this.ShowSkillBattle();
            this.ChangeRole();

            this.InitPlanName();

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                int index = i;
                Toggle_Plan_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        ChangePlan(index);
                    }
                });
            }

            for (int i = 0; i < Toggle_Role_List.Count; i++)
            {
                int index = i + 1;
                Toggle_Role_List[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        SelectRole = index;
                        ChangeRole();
                    }
                });
            }
        }

        private void InitPlanName()
        {
            int SkillPanelIndex = GameProcessor.Inst.User.SkillPanelIndex;
            Toggle_Plan_List[SkillPanelIndex].isOn = true;

            User user = GameProcessor.Inst.User;

            for (int i = 0; i < Toggle_Plan_List.Count; i++)
            {
                user.PlanNameList.TryGetValue(i, out string name);
                if (name != null)
                {
                    Text tt = Toggle_Plan_List[i].GetComponentInChildren<Text>();
                    tt.text = name;
                }
            }
        }

        private void ChangePlan(int index)
        {
            GameProcessor.Inst.User.SkillPanelIndex = index;
            this.ShowSkillBattle();
        }

        private void ChangeRole()
        {
            for (int i = 0; i < learnSkills.Count; i++)
            {
                Item_Skill item = learnSkills[i];
                if (item.SkillPanel.Config.Role == SelectRole)
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        public void ShowRuneDesc(string desc)
        {
            Dlg_Rune_Info.Show(desc);
        }

        private void ShowSkillPanel()
        {
            User user = GameProcessor.Inst.User;
            List<SkillData> skills = user.SkillList;

            List<SkillPanel> list = User_Data.GetSkills();

            list = list.OrderBy(m => m.SkillId).ToList();

            foreach (var sp in list)
            {
                ShowSkillPanelItem(sp);
            }
        }

        private void ShowSkillPanelItem(SkillPanel skill)
        {
            if (skill == null)
            {
                return;
            }

            Item_Skill learn = this.learnSkills.Find(s => s.SkillPanel.SkillId == skill.SkillId);
            if (learn != null)
            {
                learn.SetItem(skill);
            }
            else
            {
                var emptyBook = GameObject.Instantiate(bookPrefab);
                var com = emptyBook.GetComponent<Item_Skill>();
                com.SetItem(skill);
                emptyBook.transform.SetParent(this.sr_AllSkill.content);
                emptyBook.transform.localScale = Vector3.one;

                this.learnSkills.Add(com);
            }
        }

        private void ShowSkillBattle()
        {
            var user = GameProcessor.Inst.User;

            if (user == null)
            {
                return;
            }

            for (int i = 0; i < AllEquipSkills.Length; i++)
            {
                AllEquipSkills[i].Clear();
                if (i < user.SkillNumber)
                {
                    AllEquipSkills[i].gameObject.SetActive(true);
                }
                else
                {
                    AllEquipSkills[i].gameObject.SetActive(false);
                }
            }

            List<int> skills = user.GetCurrentSkillList();


            for (int i = 0; i < AllEquipSkills.Length; i++)
            {
                if (skills.Count > i)
                {
                    AllEquipSkills[i].SetItem(skills[i]);
                }
            }
        }

        private void OnSkillShow(SkillShowEvent e)
        {
            this.ShowSkillPanel();
        }

        private void OnSkillUp(SkillUpEvent e)
        {
            this.ShowSkillBattle();
        }

        private void OnSkillDown(SkillDownEvent e)
        {
            this.ShowSkillBattle();
        }
        private void OnSkillChangePlan(SkillChangePlanEvent e)
        {
            this.ShowSkillBattle();
            this.ShowSkillPanel();
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            return page == ViewPageType.View_Skill;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            this.InitPlanName();
        }
    }
}
