using System;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class Bullet:MonoBehaviour
    {
        public static Bullet MainBullet;

        private Rigidbody2D _body;

        private void Awake()
        { 
            MainBullet = this;
            _body = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("BottomBorder"))
            {
                SetSpeed(Vector2.zero);
                BulletPool.Instance.Put(this);
            }
            
        }

        private void OnDisable()
        {
            if(BulletPool.Instance.ActiveBullets==0)
                LevelEventBus.Publish(LevelEventBus.LevelEventType.BulletLand);
        }

        public void SetSpeed(Vector2 speed)
        {
            _body.velocity = speed;
        }
        
    }
}