using System.Collections.Generic;
using UnityEngine.Events;

namespace Scenes.LevelScene
{
    public static class LevelEventBus
    {

        public enum LevelEventType
        {
            BulletShoot,
            BulletLand
        }

        public static Dictionary<LevelEventType, UnityEvent> Events = new Dictionary<LevelEventType, UnityEvent>();

        public static void Subscribe(LevelEventType type, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(type, thisEvent);
            }
        }
    

        public static void Unsubscribe(LevelEventType type, UnityAction listener)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(LevelEventType type)
        {
            UnityEvent thisEvent;
            if (Events.TryGetValue(type, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }

    }
}