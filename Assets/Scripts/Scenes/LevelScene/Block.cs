using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class Block:MonoBehaviour
    {
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
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, t);
                t += Time.deltaTime * 8;
                yield return null;
            }

            transform.localScale = Vector3.one * 0.8f;
            
            t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(Vector3.one * 0.8f,Vector3.one,  t);
                t += Time.deltaTime * 8;
                yield return null;
            }
            transform.localScale = Vector3.one;
            
        }
    }
}