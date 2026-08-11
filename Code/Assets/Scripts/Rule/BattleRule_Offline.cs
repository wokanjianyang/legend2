using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BattleRule_Offline : ABattleRule
    {
        private double MapTime = 0;
        protected override RuleType ruleType => RuleType.Offline;

        private int MapId = 0;
        private int Total = 0;
        private int Model = 1;
        private bool Complete = false;

        public BattleRule_Offline(Dictionary<string, object> param)
        {
            param.TryGetValue("MapId", out object mapId);
            param.TryGetValue("MapId", out object model);

            this.MapId = (int)mapId;
            this.Model = (int)model;
            this.MapTime = 0;
        }


        public override void DoMapLogic(int roundNum, double currentRoundTime)
        {
            if (Complete)
            {
                return;
            }

            MapTime += currentRoundTime;

            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

            string msg = (int)MapTime + "S击杀" + Math.Max(0, Total - enemys.Count) + "个";
            GameProcessor.Inst.EventCenter.Raise(new ShowMainMapInfoEvent() { Message = msg });

            if (enemys.Count >= 20)
            {
                return;
            }

            if (MapTime >= ConfigHelper.OfflineTime)
            {
                this.Complete = true;
                //挑战结束

                this.GameOver();
                return;
            }

            MapConfig mapConfig = MapConfigCategory.Instance.Get(AppHelper.CurrentMapId);

            int quality = 1;

            var enemy = new Monster(mapConfig.Id, quality, RuleType.Normal, this.Model);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

            Total++;
        }

        private void GameOver()
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.CloseBattle(RuleType.Offline, 0);

            User user = User_Data_Manager.Data;

            user.OfflineLog[1] = this.MapId;
            user.OfflineLog[2] = this.Total;
            user.OfflineLog[3] = this.Model;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Normal, Message = "记录离线效率成功！" });
        }
    }



}
