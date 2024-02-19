using UnityEngine;

namespace Scenes.LevelScene.Mobs
{
    public interface IOnBulletHit
    {
        public void TakeDamage(float dmg, DamageType type, Vector3 pos);
    }
}