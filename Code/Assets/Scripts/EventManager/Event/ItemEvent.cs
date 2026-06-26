using System.Collections.Generic;
using UnityEngine;

namespace Game
{


    public class EquipToCardEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }

        public int CardId { get; set; }
    }

    public class EquipOneEvent : SDD.Events.Event
    {
        public bool IsWear { get; set; } = true;

        public int Part { get; set; }
        public BoxItem BoxItem { get; set; }
    }

    public class RestoreEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }

    public class LoseEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }

    public class EquipLockEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
        public bool IsLock { get; set; }
    }
}
