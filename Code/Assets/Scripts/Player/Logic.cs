using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Logic : MonoBehaviour, IPlayer
    {

        public bool IsSurvice { get; private set; } = true;

        //private List<SDD.Events.Event> playerEvents = new List<SDD.Events.Event>();


        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(Dictionary<AttributeEnum, object> dict)
        {
            //设置名称
            SelfPlayer.EventCenter.Raise(new SetPlayerNameEvent
            {
            });

            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        public void ResetData()
        {
            //var dict = new Dictionary<AttributeEnum, object>();
            //foreach (var kvp in BaseAttributeMap)
            //{
            //    dict[kvp.Key] = kvp.Value;
            //}
            SetData(null);
            IsSurvice = true;

            //BattleAttributeMap.Clear();

            SelfPlayer.Reset();
            //this.SelfPlayer.SetPosition(GameProcessor.Inst.PlayerManager.RandomCell(this.SelfPlayer.Cell));
        }

        public void OnDamage(DamageResult dr)
        {
            if (!IsSurvice)
            {
                return;
            }

            if (SelfPlayer.Camp == PlayerType.Hero)
            {
                //Debug.Log($"{(this.SelfPlayer.Name)} 受到伤害:{(StringHelper.FormatNumber(dr.Damage))}");
            }

            double totalDamage = dr.Damage + dr.ExtendDamage;

            double currentSP = this.SelfPlayer.SP;
            if (currentSP > 0)
            {
                double spDamge = totalDamage;

                double spRate = 0; // this.SelfPlayer.AttributeBonus.CalPanelTotalAttr(AttributeEnum.SpRate);  //用默认属性不受buff和技能影响
                if (spRate > 0)
                {
                    double maxHp = this.SelfPlayer.AttributeBonus.CalPanelTotalAttr(AttributeEnum.HP); //用默认属性，不受buff和技能影响
                    double maxSpDamge = maxHp * (100 - spRate) / 100;

                    spDamge = Math.Min(spDamge, maxSpDamge);
                    //Debug.Log("maxHp:" + maxHp + " spDamage:" + spDamge);
                }

                currentSP -= spDamge;
                if (currentSP <= 0)
                {
                    currentSP = 0;
                }
                this.SelfPlayer.SetSP(currentSP);

                if ((this.SelfPlayer.Camp == PlayerType.Enemy && AppHelper.ShowMonsterDamage)
                 || (this.SelfPlayer.Camp != PlayerType.Enemy && AppHelper.ShowPlayerEffect))
                {
                    this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
                    {
                        Type = MsgType.SP,
                        Content = "-" + StringHelper.FormatNumber(spDamge)
                    });
                }

                this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });

                return;
            }

            double currentHP = this.SelfPlayer.HP;

            currentHP -= totalDamage;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            this.SelfPlayer.SetHP(currentHP);

            if ((this.SelfPlayer.Camp == PlayerType.Enemy && AppHelper.ShowMonsterDamage)
             || (this.SelfPlayer.Camp != PlayerType.Enemy && AppHelper.ShowPlayerEffect))
            {
                if (AppHelper.ShowMonsterDamage)
                {
                    string content = "-" + StringHelper.FormatNumber(dr.Damage);
                    if (dr.ExtendDamage > 0)
                    {
                        content += "+" + StringHelper.FormatNumber(dr.ExtendDamage);

                        //Debug.Log("DM:" + StringHelper.FormatNumber(dr.Damage) + "  EDM:" + StringHelper.FormatNumber(dr.ExtendDamage));
                    }
                    this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
                    {
                        Type = dr.Type,
                        Content = content
                    });
                }
            }

            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });

            if (currentHP <= 0)
            {
                var skillFuhuo = this.SelfPlayer.GetSkillByPriority(-1);
                if (skillFuhuo != null)
                {
                    skillFuhuo.Do();
                    return;
                }

                IsSurvice = false;
                this.SelfPlayer.EventCenter.Raise(new DeadRewarddEvent
                {
                    FromId = dr.FromId,
                    ToId = SelfPlayer.ID
                });

                if (SelfPlayer.Camp == PlayerType.Hero)
                {
                    if (SelfPlayer.RuleType != RuleType.MainStage && SelfPlayer.RuleType != RuleType.Babel)
                    {
                        //自动复活
                        StartCoroutine(this.AutoResurrection());
                    }
                }
                else if (SelfPlayer.Camp == PlayerType.Hero_Pet)
                {
                    //自动复活
                    StartCoroutine(this.AutoResurrection());
                }
                else
                {
                    StartCoroutine(this.ClearPlayer());
                }

            }
        }

        public void ToDie()
        {
            this.SelfPlayer.SetSP(0);
            this.SelfPlayer.SetHP(0);
            this.IsSurvice = false;
        }

        private IEnumerator ClearPlayer()
        {
            yield return new WaitForSeconds(ConfigHelper.DelayShowTime);
            GameProcessor.Inst.PlayerManager.RemoveDeadPlayers(this.SelfPlayer);
            yield return null;
        }

        private IEnumerator AutoResurrection()
        {
            int cd = ConfigHelper.AutoResurrectionTime;

            for (int i = 0; i < cd; i++)
            {
                SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
                {
                    Type = MsgType.Normal,
                    Content = $"{(cd - i)}秒后复活"
                });
                yield return new WaitForSeconds(1f);
            }

            SelfPlayer.Resurrection();
        }

        public void OnRestore(double hp)
        {
            double currentHP = this.SelfPlayer.HP;

            if (currentHP <= 0)
            {
                //?是否先判断死亡，再判断回复
                return;
            }

            double maxHp = this.SelfPlayer.AttributeBonus.CalBattleTotalAttr(AttributeEnum.HP);

            if (maxHp <= currentHP)
            {
                //满血不回复
                return;
            }

            currentHP += hp;
            if (maxHp <= currentHP)
            {
                currentHP = maxHp; //最多只能回复满血
            }

            if (SelfPlayer.Camp == PlayerType.Hero)
            {
                //Debug.Log($"{(this.SelfPlayer.Name)} 恢复生命:{(hp)} ,剩余血量:{(currentHP)}");
            }

            this.SelfPlayer.SetHP(currentHP);

            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent
            {
                Type = MsgType.Restore,
                Content = StringHelper.FormatNumber(hp)
            });
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }

        //public void RaiseEvents()
        //{
        //    foreach(var e in this.playerEvents)
        //    {
        //        this.SelfPlayer.EventCenter.Raise(e);
        //    }
        //    this.playerEvents.Clear();
        //}

        //public int GetMaxHP()
        //{
        //    var baseValue = 0f;
        //    if (BaseAttributeMap.TryGetValue(AttributeEnum.HP, out var value))
        //    {
        //        baseValue = (float)Convert.ToDouble(value);
        //    }
        //    return (int)baseValue;
        //}


        //public float GetAttributeFloat(AttributeEnum attr)
        //{
        //    var baseValue = 0f;
        //    if (BaseAttributeMap.TryGetValue(attr, out var value))
        //    {
        //        baseValue = (float)Convert.ToDouble(value);
        //    }

        //    var battleValue = 0f;
        //    if (BattleAttributeMap.TryGetValue(attr, out var value2))
        //    {
        //        battleValue = (float)Convert.ToDouble(value2);
        //    }

        //    return baseValue + battleValue;
        //}

        public void AddBattleAttribute(AttributeEnum attr, float value)
        {
            //BattleAttributeMap.TryGetValue(attr, out var value2);
            //BattleAttributeMap[attr] = (float)Convert.ToDouble(value2) + value;
        }

        /*        private void SetHP(string hp)
                {
                    SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent
                    {
                        HP = hp
                    });
                }*/

        public APlayer SelfPlayer { get; set; }
        public void SetParent(APlayer player)
        {
            SelfPlayer = player;
        }
    }
}
