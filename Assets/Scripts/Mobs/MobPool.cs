using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Scenes.LevelScene.Mobs
{
    public class MobPool:MonoBehaviour
    {
        public static MobPool Instance;

        private Dictionary<MobType, List<DefaultMob>> _activeMobs = new Dictionary<MobType, List<DefaultMob>>();

        private Dictionary<MobType, Stack<DefaultMob>> _mobStack = new Dictionary<MobType, Stack<DefaultMob>>();
        public int ActiveMobs => _activeMobs.Count;

        private void Awake()
        {
            Instance = this;
        }

        public DefaultMob Get(GameObject prefab, MobType type)
        {
            if (_mobStack.Count == 0)
            {
                DefaultMob mob = Instantiate(prefab.gameObject, transform).gameObject.GetComponent<DefaultMob>();
                if(!_activeMobs.ContainsKey(type))
                    _activeMobs.Add(type, new List<DefaultMob>());
                _activeMobs[type].Add(mob);
                return mob;
            }
            else
            {
                DefaultMob mob = _mobStack[type].Pop();
                mob.gameObject.SetActive(true);
                if(!_activeMobs.ContainsKey(type))
                    _activeMobs.Add(type, new List<DefaultMob>());
                _activeMobs[type].Add(mob);
                return mob;
            }
        }

        public void Put(DefaultMob mob, MobType type)
        {
            _activeMobs[type].Remove(mob);
            if (!_mobStack.ContainsKey(type))
                _mobStack.Add(type, new Stack<DefaultMob>());
            _mobStack[type].Push(mob);
            mob.gameObject.SetActive(false);
        }

        public void ClearActiveBullets()
        {
            for (int i = 0; i<_activeMobs.Keys.Count; i++)
            {
                for (int j = 0; j < _activeMobs[(MobType)i].Count; i++)
                {
                    Put(_activeMobs[(MobType)i][0].GetComponent<DefaultMob>(), (MobType)i);
                }
            }
        }
    }
}