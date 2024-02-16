using System;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Floor
{
    public class FloorSpawner:MonoBehaviour
    {

        private SpriteRenderer[] _spriteRenderers;

        private void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            foreach (SpriteRenderer sprite in _spriteRenderers)
            {
                int num = Mathf.FloorToInt(GameVariables.LevelInfos[GameVariables.CurrentLevel].FloorProbability.Evaluate(Random.value)
                                           * GameVariables.LevelInfos[GameVariables.CurrentLevel].FloorPrefabs.Length);
                sprite.sprite = GameVariables.LevelInfos[GameVariables.CurrentLevel].FloorPrefabs[num].GetComponent<SpriteRenderer>().sprite;

                int rotation = Random.Range(0, 4);
                
                sprite.transform.rotation = Quaternion.Euler(new Vector3(0,0,rotation*90));
            }
        }
    }
}