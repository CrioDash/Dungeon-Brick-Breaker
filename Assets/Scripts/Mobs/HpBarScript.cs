using System;
using System.Collections;
using Scenes.LevelScene.Mobs;
using UnityEngine;
using UnityEngine.TextCore;

namespace Mobs
{
    public class HpBarScript:MonoBehaviour
    {
        [SerializeField] private GameObject hpBar;
        
        private DefaultMob _parent;

        private Coroutine _coroutine;

        private void Awake()
        {
            _parent = transform.parent.GetComponent<DefaultMob>();
        }

        private void OnEnable()
        {
            hpBar.transform.localScale = new Vector3(0.95f, 0.6f, 0);
        }

        public void TakeDamage(int dmg)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(TakeDamageRoutine(dmg));
        }

        private IEnumerator TakeDamageRoutine(int dmg)
        {
            float t = 0;
            
            Vector3 startScale = new Vector3(0.95f * ((float)(_parent.Health + dmg) / _parent.MaxHealth), 0.6f, 0);
            Vector3 endScale = new Vector3(0.95f * ((float)_parent.Health / _parent.MaxHealth), 0.6f, 0);

            if (_parent.Health <= 0)
                endScale.x = 0;
            
            Vector3 scale = hpBar.transform.localScale;

            while (t < 1)
            {
                scale = Vector3.Lerp(startScale, endScale, t);
                hpBar.transform.localScale = scale;
                t += Time.deltaTime * 4;
                yield return null;
            }
            

        }
    }
}