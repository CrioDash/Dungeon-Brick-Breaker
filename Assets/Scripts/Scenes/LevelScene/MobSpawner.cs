using System;
using System.Collections;
using Data;
using Scenes.LevelScene.Mobs;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.LevelScene
{
    public class MobSpawner:MonoBehaviour
    {
        [SerializeField] private Transform mobGrid;
        
        private GameObject[] _mobPrefabs;
        
        
        private float _mobQuantity;
        private AnimationCurve _mobProbability;

        private int _waveNum = 1;

        private void Start()
        {
            _mobPrefabs = GameVariables.LevelInfos[GameVariables.CurrentLevel].MobPrefabs;
            _mobProbability = GameVariables.LevelInfos[GameVariables.CurrentLevel].MobProbability;
            _mobQuantity = GameVariables.LevelInfos[GameVariables.CurrentLevel].MobQuantity;
            
            SpawnMobs();
        }
        

        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, SpawnMobs);
        }

        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand, SpawnMobs);   
        }

        private void SpawnMobs()
        {
            Vector3 spawnPos = new Vector3(0, 7f, 0);
            for (int i = -3; i < 5; i++)
            {
                if(Random.Range(0f, 1f) > _mobQuantity + _waveNum*0.01875f) continue;
                
                int num = Mathf.FloorToInt(_mobProbability.Evaluate(Random.value) * _mobPrefabs.Length);
                
                spawnPos.x = i - 0.55f;
                DefaultMob mob =
                    MobPool.Instance.Get(_mobPrefabs[num], _mobPrefabs[num].GetComponent<DefaultMob>().Type);
                mob.transform.SetParent(mobGrid);
                mob.transform.localPosition = spawnPos;
            }
        }

        

    }
}