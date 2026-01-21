using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class ShowMainMapInfoEvent : SDD.Events.Event
    {
        public int Count { get; set; }
    }

    public class ChangeMainMapEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
    }
}
