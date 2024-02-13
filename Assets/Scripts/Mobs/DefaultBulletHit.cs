using UnityEngine;

namespace Scenes.LevelScene.Mobs
{
    public class DefaultBulletHit:MonoBehaviour, IOnBulletHit
    {
        private DefaultMob _mob;
        
        public void TakeDamage(int dmg)
        {
            if (_mob == null)
                _mob = GetComponent<DefaultMob>();
            _mob.ReduceHealth(dmg);
        }
    }
}