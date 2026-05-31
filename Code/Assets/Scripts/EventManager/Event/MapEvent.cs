using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChangeMainMapEvent : SDD.Events.Event
    {
        public RuleType Type { get; set; }

        public int MapId { get; set; }
    }

    public class ShowMainMapInfoEvent : SDD.Events.Event
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }


    //Ö÷Ïß¹Ø¿¨

    public class StartStageEvent : SDD.Events.Event
    {

    }

    public class ShowStageInfoEvent : SDD.Events.Event
    {
        public int Mc1 { get; set; }
        public int Mc2 { get; set; }
        public int Mc3 { get; set; }
        public int Mc4 { get; set; }
        public int Mc5 { get; set; }
    }
}
