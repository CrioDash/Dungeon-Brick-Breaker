using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class Block:MonoBehaviour
    {
        private SpriteRenderer _sprite;

        private Vector3 _startScale;
        private Color _clr;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _clr = _sprite.color;
            _startScale = transform.localScale;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Bullet"))
                StartCoroutine(BlockHitRoutine());
        }

        private IEnumerator BlockHitRoutine()
        {
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(_startScale, Vector3.one*1.3f, t);
                t += Time.deltaTime * 8;
                yield return null;
            }

            _sprite.color = new Color(1, 0.5f, 0.7f);
            transform.localScale = Vector3.one*1.3f;
            
            t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(Vector3.one*1.3f,_startScale,  t);
                t += Time.deltaTime * 8;
                yield return null;
            }
            transform.localScale = _startScale;
            _sprite.color = Color.white;

        }
    }
}