using Scenes.LevelScene.Mobs;
using UnityEngine;

namespace Mobs
{
    public class Shielder:DefaultMob
    {
        [SerializeField] private GameObject sparksPrefab;
        protected override void SetStats()
        {
            BulletHit ??= gameObject.AddComponent<ShielderBulletHit>();
        }
        public void CreateSparks()
        {
            GameObject gm = Instantiate(sparksPrefab,transform);
            gm.transform.position = transform.position;
            Destroy(gm, 0.5f);
        }
    }
}