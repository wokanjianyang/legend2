using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using SDD.Events;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    abstract public class APlayer
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public long Level { get; set; }

        public double SP { get; set; }

        public double MaxSP { get; set; }
        public double HP { get; set; }
        public int Quality { get; set; }

        public bool IsHide { get; set; } = false;

        public long BirthDay { get; set; } = 0;

        private float MoveInterval = 1;

        public PlayerType Camp { get; set; }

        public MondelType ModelType { get; set; } = MondelType.Nomal;

        public RuleType RuleType = RuleType.Normal;

        public int FashionId { get; set; } = 0;

        public int RingType { get; set; } = 0;

        public bool IsFestive { get; set; } = false;

        public Vector3Int Cell { get; set; }

        public AttributeBonus AttributeBonus { get; set; }

        public Transform Transform { get; private set; }

        public Logic Logic { get; private set; }

        public int RoundCounter { get; set; }

        public EventManager EventCenter { get; private set; }

        public bool IsSurvice
        {
            get
            {
                return this.Logic.IsSurvice && this.HP > 0;
            }
        }

        public List<SkillState> SelectSkillList { get; set; }

        public Effect_Manager EffectManager;

        //public List<Effect_State> EffectStateList { get; set; }

        //protected Dictionary<int, List<Effect>> EffectMap = new Dictionary<int, List<Effect>>();


        public Player_Info Info = null;
        //private Dictionary<int, int> SkillUseRoundCache = new Dictionary<int, int>();

        public void ChangeMaxHp(int fromId, double total)
        {
            //double PreMaxHp = this.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            ////Debug.Log("PreMaxHp:" + PreMaxHp);
            //double rate = this.HP * 1f / PreMaxHp;
            ////Debug.Log("rate:" + rate);
            ////Debug.Log("effect maxHp Rate:" + total);

            //this.AttributeBonus.SetAttr(AttributeEnum.PanelHp, fromId, total);

            //double CurrentMaxHp = this.AttributeBonus.GetTotalAttrDouble(AttributeEnum.HP);
            ////Debug.Log("CurrentMaxHp:" + CurrentMaxHp);
            //double currentHp = CurrentMaxHp * rate;
            ////Debug.Log("effect MaxHp:" + StringHelper.FormatNumber(currentHp));
            //this.HP = currentHp;

            //this.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        [JsonIgnore]
        public int GroupId { get; set; }

        protected APlayer _enemy;
        public APlayer Enemy
        {
            get
            {
                //if (_enemy != null && _enemy.IsSurvice)
                //{
                //    return _enemy;
                //}

                //_enemy = null;

                return _enemy;
            }
        }

        virtual public APlayer CalcEnemy()
        {
            if (_enemy != null && (_enemy.IsHide || !_enemy.IsSurvice))
            {
                _enemy = null;
            }

            return _enemy;
        }

        public void ClearEnemy()
        {
            this._enemy = null;
        }




        public APlayer()
        {
            //this.UUID = System.Guid.NewGuid().ToString("N");
            this.EventCenter = new EventManager();
            this.AttributeBonus = new AttributeBonus();
            this.SelectSkillList = new List<SkillState>();

            this.EffectManager = new Effect_Manager(this);

            //this.Load();
        }

        [JsonIgnore]
        public int UseSkillPosition { get; set; } = 0;

        virtual public void Load()
        {
            //Debug.Log("this.Qulaity" + this.Quality);

            GameObject prefab = PrefabHelper.Instance().GetPlayer(Camp, this.Quality);
            this.Transform = GameObject.Instantiate(prefab).transform;

            this.Transform.SetParent(GameProcessor.Inst.PlayerRoot);
            var rect = this.Transform.GetComponent<RectTransform>();
            //rect.sizeDelta = GameProcessor.Inst.MapData.CellSize;
            rect.localScale = UnityEngine.Vector3.one;
            this.Transform.SetAsFirstSibling();

            this.Logic = this.Transform.GetComponent<Logic>();
            var coms = this.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(this);
                }
            }

            //加载技能
            //LoadSkill();
        }

        virtual public void Reset()
        {
            double maxHP = AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);
            SetHP(maxHP);
        }

        public void SetSpeed(int atkSpeed, int moveSpeed)
        {
            this.MoveInterval = Mathf.Max(0.2f, 100f / (100 + moveSpeed));
        }

        public float CalAtkInterval(int skillPercent)
        {
            int AtkSpeed = (int)AttributeBonus.CalBattleTotalAttr(AttributeEnum.Speed);

            return Mathf.Max(0.2f, 100f / (100 + AtkSpeed + skillPercent));
        }

        public float CalMoveInterval()
        {
            return this.MoveInterval;
        }

        virtual public SkillState GetSkill(int priority)
        {
            List<SkillState> list = SelectSkillList.Where(m => m.SkillPanel.Config.Priority >= priority && m.SkillPanel.SkillId != 9001)
                .OrderBy(m => m.UserCount * 1000 + m.Priority).ToList();

            //long now = TimeHelper.ClientNowSeconds();

            foreach (SkillState state in list)
            {
                if (state.IsCanUse())
                {
                    return state;
                }
            }

            if (priority == 0)
            {
                SkillState normal = SelectSkillList.FirstOrDefault(m => m.SkillPanel.SkillId == 9001);
                if (normal != null && normal.IsCanUse())
                {
                    return normal;
                }
            }

            return null;
        }


        //增加普攻技能
        protected void AddSkillNormal()
        {
            SkillData sd = new SkillData(9001, (int)SkillPosition.Default);
            SkillPanel sp = new SkillPanel(sd, null, null, null, false);
            SkillState skill = new SkillState(this, sp, sd.Position, 0);
            SelectSkillList.Add(skill);
        }

        public virtual void SetSkillAfter()
        {

        }

        public void SkillAfter()
        {
        }


        public SkillState GetSkillByPriority(int priority)
        {
            SkillState state = SelectSkillList.Where(m => m.SkillPanel.Config.Priority == priority).FirstOrDefault();


            if (state != null && state.IsCanUse())
            {
                return state;
            }

            return null;
        }


        public bool GetIsPause()
        {
            return EffectManager.isPause();
        }

        public void DoCD(float time)
        {
            if (!this.IsSurvice) return;

            foreach (SkillState ss in this.SelectSkillList)
            {
                ss.RunCD(time);
            }

            this.EffectManager.RunCD(time);
        }

        public virtual float DoEvent()
        {
            this.RoundCounter++;

            if (!this.IsSurvice) return 1f;

            //1.控制前计算高优级技能
            float attackIgnorePause = AttackIgnorePause();
            if (attackIgnorePause > 0)
            {
                return attackIgnorePause;
            }

            //2.判断控制
            if (GetIsPause())
            {
                return Math.Min(CalAtkInterval(0), CalMoveInterval());
            }

            //3.普通技能
            return AttackLogic();
        }

        public virtual float AttackIgnorePause()
        {
            SkillState skill = this.GetSkill(200);
            if (skill != null)
            {
                //Debug.Log("Player Use Prioriry Skill:" + skill.SkillPanel.SkillData.SkillConfig.Name);
                skill.Do();

                //行动结算

                return CalAtkInterval(skill.SkillPanel.Speed);
            }

            return 0f;
        }

        public virtual float AttackLogic()
        {
            //1. 优先攻击首要目标
            this.CalcEnemy();
            SkillState skill;
            if (_enemy != null)
            {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();

                    return CalAtkInterval(skill.SkillPanel.Speed);
                }
            }

            //2. 攻击最近目标
            _enemy = this.FindNearestEnemy();
            if (_enemy != null)
            {
                skill = this.GetSkill(0);
                if (skill != null)
                {
                    skill.Do();

                    return CalAtkInterval(skill.SkillPanel.Speed);
                }
            }
            else
            {
                return CalAtkInterval(0);
            }

            //3. 移动到首要目标
            return MoveToEnemy();
        }

        private float MoveToEnemy()
        {
            var enemys = FindNearestEnemys();

            for (int i = 0; i < enemys.Count; i++)
            {
                var endPos = GameProcessor.Inst.MapData.GetPath(this.Cell, enemys[i].Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                    return MoveInterval;
                }
            }
            return 1f;
        }



        public void Move(Vector3Int cell)
        {
            this.SetPosition(cell);
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
            this.Transform.DOKill(true);
            this.Transform.DOLocalMove(targetPos, MoveInterval);
        }

        public void MoveFast(Vector3Int cell)
        {
            this.SetPosition(cell);
            var targetPos = GameProcessor.Inst.MapData.GetWorldPosition(cell);
            this.Transform.DOKill(true);
            this.Transform.DOLocalMove(targetPos, 0.1f);
        }

        public void SetPosition(Vector3 pos, bool isGraphic = false)
        {
            this.Cell = new Vector3Int((int)pos.x, (int)pos.y, 0);
            if (isGraphic)
            {
                this.Transform.localPosition = GameProcessor.Inst.MapData.GetWorldPosition(this.Cell);
            }
        }

        public APlayer FindNearestEnemy()
        {

            APlayer ret = null;

            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.IsSurvice && p.GroupId != this.GroupId && !p.IsHide);

            if (enemys.Count > 0)
            {
                enemys.Sort((a, b) =>
                {
                    var distance = a.Cell - this.Cell;
                    var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    distance = b.Cell - this.Cell;
                    var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    if (l0 < l1)
                    {
                        return -1;
                    }
                    else if (l0 > l1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                });

                ret = enemys[0];
            }

            return ret;
        }

        public List<APlayer> FindNearestEnemys()
        {
            //查找和自己不同类的,并且不是自己的主人/仆人
            var enemys = GameProcessor.Inst.PlayerManager.GetAllPlayers().FindAll(p => p.IsSurvice && p.GroupId != this.GroupId && !p.IsHide);

            if (enemys.Count > 0)
            {
                enemys.Sort((a, b) =>
                {
                    var distance = a.Cell - this.Cell;
                    var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    distance = b.Cell - this.Cell;
                    var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

                    if (l0 < l1)
                    {
                        return -1;
                    }
                    else if (l0 > l1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                });
            }

            return enemys.GetRange(0, Math.Min(enemys.Count, 3));
        }

        public virtual void OnHit(DamageResult dr)
        {
            this.Logic.OnDamage(dr);
        }

        public void OnRestore(int fromId, double hp)
        {
            double decre = this.AttributeBonus.CalBattleTotalAttr(AttributeEnum.DecreRestore);
            decre = Math.Min(100, decre);

            hp = hp * (100 - decre) / 100.0;

            this.Logic.OnRestore(hp);
        }

        public void SetHP(double hp)
        {
            this.HP = hp;
        }

        public void AddSP(double sp)
        {
            this.MaxSP = sp;
            this.SP = sp;
        }
        public void SetSP(double sp)
        {
            this.SP = sp;
        }



        public void ShowMiss()
        {
            if ((this.Camp == PlayerType.Enemy && AppHelper.ShowMonsterDamage)
              || (this.Camp != PlayerType.Enemy && AppHelper.ShowPlayerEffect))
            {
                this.EventCenter.Raise(new ShowMsgEvent()
                {
                    Type = MsgType.Miss,
                    Content = "闪"
                });
            }
        }

        public void ShowMiss2()
        {
            if ((this.Camp == PlayerType.Enemy && AppHelper.ShowMonsterDamage)
             || (this.Camp != PlayerType.Enemy && AppHelper.ShowPlayerEffect))
            {
                this.EventCenter.Raise(new ShowMsgEvent()
                {
                    Type = MsgType.Miss,
                    Content = "躲"
                });
            }
        }

        /// <summary>
        /// 复活
        /// </summary>
        public void Resurrection()
        {
            this.Logic.ResetData();
            this._enemy = null;
        }

        public T GetComponent<T>()
        {
            return this.Transform.GetComponent<T>();
        }

        public void OnDestroy()
        {
            //foreach (var skill in this.SelectSkillList)
            //{
            //    skill.Destory();
            //}
            //SelectSkillList.Clear();

            this.EventCenter.RemoveAllListeners();
            if (this.Transform != null)
            {
                GameObject.Destroy(this.Transform.gameObject);
            }
        }
    }
}
