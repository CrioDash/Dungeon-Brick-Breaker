using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class BulletPool: MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        
        public static BulletPool Instance;

        private List<Bullet> _activeBullets = new List<Bullet>();
        
        private Stack<Bullet> _bulletsStack = new Stack<Bullet>();

        public int ActiveBullets => _activeBullets.Count;

        private void Awake()
        {
            Instance = this;
        }

        public Bullet Get()
        {
            
            if (_bulletsStack.Count == 0)
            {
                Bullet bul = Instantiate(bulletPrefab.gameObject, transform).gameObject.GetComponent<Bullet>();
                _activeBullets.Add(bul);
                return bul;
            }
            else
            {
               Bullet bul = _bulletsStack.Pop();
               bul.gameObject.SetActive(true);
               _activeBullets.Add(bul);
               return bul;
            }
        }

        public void Put(Bullet bullet)
        {
            _activeBullets.Remove(bullet);
            _bulletsStack.Push(bullet);
            bullet.gameObject.SetActive(false);
        }

        public void ClearActiveBullets()
        {
            int num = _activeBullets.Count;
            for (int i = 0; i<num; i++)
            {
                Put(_activeBullets[0]);
            }
        }

    }
}