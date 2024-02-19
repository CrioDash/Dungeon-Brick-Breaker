using Data;
using UnityEngine;

namespace Scenes.LevelScene.Mobs
{
    public class DefaultBulletHit:MonoBehaviour, IOnBulletHit
    {
        private DefaultMob _mob;
        
        public void TakeDamage(float dmg, DamageType type, Vector3 pos)
        {
            if (_mob == null)
                _mob = GetComponent<DefaultMob>();
            _mob.ReduceHealth(dmg, type);
        }
    }
}