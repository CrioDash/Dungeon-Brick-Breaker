using Scenes.LevelScene;
using Scenes.LevelScene.Mobs;
using UnityEngine;

namespace Mobs
{
    public class ShielderBulletHit:MonoBehaviour, IOnBulletHit
    {
        private Shielder _mob;
        public void TakeDamage(float dmg, DamageType type, Vector3 pos)
        {
            if (_mob == null)
                _mob = GetComponent<Shielder>();
            if (pos.y <= transform.position.y)
            {
                _mob.CreateSparks();
                dmg /= 5;
            }
            _mob.ReduceHealth(dmg, type);
        }
    }
}