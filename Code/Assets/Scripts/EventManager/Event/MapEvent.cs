using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChangeMainMapEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
    }

    public class ShowMainMapInfoEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
        public int Count { get; set; }
        public int Time { get; set; }
    }
}
