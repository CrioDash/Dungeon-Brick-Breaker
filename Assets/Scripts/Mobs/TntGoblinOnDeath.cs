using System.Collections.Generic;
using System.Linq;
using Scenes.LevelScene;
using Scenes.LevelScene.Mobs;
using UnityEngine;

namespace Mobs
{
    public class TntGoblinOnDeath:MonoBehaviour, IOnDeath
    {
        private TntGoblin _mob;
        
        public void DeathEvent()
        {
            if (_mob == null)
                _mob = GetComponent<TntGoblin>();
            
            Vector3 pos = transform.position;
            pos.y += 0.5f;
            List<Collider2D> mobs = Physics2D.OverlapCircleAll(pos, 1).ToList();
            foreach (Collider2D col in mobs)
            {
                if (col.CompareTag("Mob") && col.gameObject != gameObject)
                    col.GetComponent<DefaultMob>().BulletHit.TakeDamage(5, DamageType.Fire, transform.position);
            }
            _mob.CreateSmoke();
        }
    }
}