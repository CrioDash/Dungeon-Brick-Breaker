using System;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class Player:MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private GameObject heroSprite;
        
        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletShoot, HideHero);
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, ShowHero);
        }

        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletShoot, HideHero);
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand, ShowHero);
        }

        private void HideHero()
        {
            heroSprite.gameObject.SetActive(false);
            particleSystem.Play();
        }

        private void ShowHero()
        {
            heroSprite.gameObject.SetActive(true);
            particleSystem.Play();
        }
        
    }
}