using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.LevelScene.UI
{
    public class StopButtonScript: MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletShoot,
                () => gameObject.SetActive(true));
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, 
                () => gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletShoot,
                () => gameObject.SetActive(true));
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand,
                () => gameObject.SetActive(false));
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}