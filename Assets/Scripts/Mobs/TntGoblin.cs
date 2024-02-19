using Scenes.LevelScene.Mobs;
using UnityEngine;

namespace Mobs
{
    public class TntGoblin:DefaultMob
    {
        [SerializeField] private GameObject smokePrefab;
        
        protected override void SetStats()
        {
            BulletHit ??= gameObject.AddComponent<DefaultBulletHit>();
            Death ??= gameObject.AddComponent<TntGoblinOnDeath>();
        }

        public void CreateSmoke()
        {
            Vector3 pos = transform.position;
            pos.y += 0.5f;
            Instantiate(smokePrefab, pos, Quaternion.identity);
        }
    }
}