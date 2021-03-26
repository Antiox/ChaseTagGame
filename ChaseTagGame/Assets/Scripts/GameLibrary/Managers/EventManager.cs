using GameLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<Type, Action<IGameEvent>> events = new Dictionary<Type, Action<IGameEvent>>();
        private readonly Dictionary<Delegate, Action<IGameEvent>> callbacks = new Dictionary<Delegate, Action<IGameEvent>>();

        public void AddListener<T>(Action<T> e) where T : IGameEvent
        {
            if (!callbacks.ContainsKey(e))
            {
                void action(IGameEvent a) => e((T)a);
                callbacks[e] = action;

                if (events.TryGetValue(typeof(T), out Action<IGameEvent> internalAction))
                    events[typeof(T)] = internalAction += action;
                else
                    events[typeof(T)] = action;
            }
        }

        public void RemoveListener<T>(Action<T> e) where T : IGameEvent
        {
            if (callbacks.TryGetValue(e, out var action))
            {
                if (events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        events.Remove(typeof(T));
                    else
                        events[typeof(T)] = tempAction;
                }

                callbacks.Remove(e);
            }
        }

        public void Broadcast(IGameEvent e)
        {
            if (events.TryGetValue(e.GetType(), out var action))
                action.Invoke(e);
        }

        public void Clear()
        {
            events.Clear();
            callbacks.Clear();
        }
    }
}
