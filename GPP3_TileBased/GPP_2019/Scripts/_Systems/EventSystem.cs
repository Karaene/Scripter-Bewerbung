using System;
using System.Collections.Generic;
using System.Text;

namespace GPP_2019
{
    class EventSystem
    {
        //Singleton
        private EventSystem() { }
        private static EventSystem instance = null;
        public static EventSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventSystem();
                }
                return instance;
            }
        }

        public class EventListener
        {
            public delegate void Callback(params object[] parameters);
            public bool IsSingleShot;
            public Callback Method;

            public EventListener(bool IsSingleShot)
            {
                this.IsSingleShot = IsSingleShot;
            }
        }

        private Dictionary<string, IList<EventListener>> eventTable;
        private Dictionary<string, IList<EventListener>> EventTable
        {
            get
            {
                if (eventTable == null)
                {
                    eventTable = new Dictionary<string, IList<EventListener>>();
                }
                return eventTable;
            }
        }

        private Queue<PointsEventData> eventQueue;
        private Queue<PointsEventData> EventQueue
        {
            get
            {
                if (eventQueue == null)
                    eventQueue = new Queue<PointsEventData>();

                return eventQueue;
            }
        }

        public void AddListener(string name, EventListener listener)
        {
            if (!EventTable.ContainsKey(name))
                EventTable.Add(name, new List<EventListener>());
            

            if (EventTable[name].Contains(listener))
                return;

            EventTable[name].Add(listener);
        }

        public void RaiseEvent(string name, params object[] parameters)
        {
            if (!EventTable.ContainsKey(name))
                return;

            for (int i = 0; i < EventTable[name].Count; i++)
            {
                EventListener listener = EventTable[name][i];
                listener.Method(parameters);
                if (listener.IsSingleShot)
                    EventTable[name].Remove(listener);
            }
        }

        /* Bei Raise Event erstmal das Event in eine Queue legen
         * Später dann die Queue durchgehen und die Events nach Type sortiert ausführen
         */

        /*
        public void AddEvent(Category category, string name, params object[] parameters)
        {
            if (!EventTable.ContainsKey(name))
                return;
            
            EventQueue.Enqueue(new PointsEventData(category, name, parameters));

            Console.WriteLine("new Event added!");
            
            foreach (var element in EventQueue)
            {
                Console.WriteLine("Queue Elements: " + element.Name);
            }
        }
        */
    }
}
