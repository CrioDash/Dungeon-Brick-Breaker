
using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Scenes.LevelScene
{
    public class HoldAimScript : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform shootTransform;

        [SerializeField] private Transform leftBorder;
        [SerializeField] private Transform rightBorder;
        
        [SerializeField] private int bounceCount;
        [SerializeField] private Transform[] points;
        [SerializeField] private float speed;
        
        private BoxCollider2D _boxCollider;
        
        private LineRenderer _lineRenderer;
 
        private Vector3 _tempPos = Vector3.zero;
        private Vector2 _bulletDirection = Vector3.zero;
        
        private bool _isTouched;
        private bool _isCapable = true;


        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(false);
            }
            
        }

        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, BulletLand);
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletShoot, () => StartCoroutine(BulletShoot()));
        }
        
        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand, BulletLand);
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletShoot, () => StartCoroutine(BulletShoot()));
        }

        private void Update()
        {
            if (_isTouched)
            {
                _lineRenderer.material.mainTextureOffset = new Vector2(-Time.time*4, 0);
            }
        }

        #region MouseHandlers

        private void OnMouseDown()
        {
            if(!_isCapable)
                return;
            _isTouched = true;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(true);
            }
            ShootRay();
        }

        private void OnMouseDrag()
        {
            if(_isTouched)
                ShootRay();
        }

        private void OnMouseExit()
        {
            _isTouched = false;
            _lineRenderer.positionCount = 0;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(false);
            }
        }

        private void OnMouseUp()
        {
            
            _lineRenderer.positionCount = 0;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(false);
            }

            if (_isTouched)
            {
                LevelEventBus.Publish(LevelEventBus.LevelEventType.BulletShoot);
                shootTransform.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            _isTouched = false;
        }

        #endregion

        private IEnumerator BulletShoot()
        {
            _isCapable = false;
            
            for(int i =0; i< GameVariables.BulletCount; i++)
            {
                Bullet bul = BulletPool.Instance.Get();
                bul.transform.position = shootTransform.position;
                bul.SetSpeed(_bulletDirection.normalized * speed);

                yield return new WaitForSeconds(0.25f);
            }
        }

        private void BulletLand()
        {
            Vector3 pos = Bullet.MainBullet.transform.position;
            pos.y = playerTransform.position.y;
            pos.x = Mathf.Clamp(pos.x, leftBorder.position.x, rightBorder.position.x);
            playerTransform.position = pos;
            _isCapable = true;
        }
    

        private void ShootRay()
        {
            
            _lineRenderer.positionCount = bounceCount;
            _lineRenderer.SetPosition(0, (Vector2)shootTransform.position);

            Vector3 touchPos;
        
            touchPos = Application.platform == RuntimePlatform.WindowsEditor ? Camera.main.ScreenToWorldPoint(Input.mousePosition) 
                : Camera.main.ScreenToWorldPoint(Input.touches[0].position);

            if (_tempPos == touchPos)
                return;
            _tempPos = touchPos;

            Vector2 origin = shootTransform.position;
            Vector2 direction = (Vector2)touchPos - origin;

            float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            
            shootTransform.transform.rotation = Quaternion.Euler(new Vector3(0,0,angle-90f));
            
            _bulletDirection = direction;
        
            for(int i = 1; i<bounceCount; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, direction,
                    Mathf.Infinity, LayerMask.GetMask("Borders"));
                
                direction = Vector2.Reflect(direction.normalized, hit.normal);
                origin = hit.point + 0.01f * direction;
                
                
                _lineRenderer.SetPosition(i, origin);
                points[i-1].position = origin;
            }
        }
    
    
    }
}
