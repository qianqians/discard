using UnityEngine;
using System.Collections;

namespace TinyFrameWork
{
    public class EventDispatcher
    {
        private static EventDispatcher instance;

        // common event manager.
        private EventManager eventCommonManager = new EventManager();
        // battle event manager.
        private EventManager eventBattleManager = new EventManager();
        // uiframework event manager.
        private EventManager eventUIFrameworkManager = new EventManager();
        //private EventManager eventSocketManager = new EventManager();

        public static EventDispatcher GetInstance()
        {
            if (instance == null)
                instance = new EventDispatcher();
            return instance;
        }

        public EventManager MainEventManager
        {
            get { return this.eventCommonManager; }
            private set { }
        }

        public EventManager BattleEventManager
        {
            get { return this.eventBattleManager; }
            private set { }
        }

        public EventManager UIFrameWorkEventManager
        {
            get { return this.eventUIFrameworkManager; }
            private set { }
        }

        //public EventManager EventSocketManager
        //{
        //    get { return this.eventSocketManager; }
        //    private set { }
        //}
    }
}