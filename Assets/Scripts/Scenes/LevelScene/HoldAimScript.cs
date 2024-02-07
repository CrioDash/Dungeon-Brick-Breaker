using UnityEngine;

namespace Scenes.LevelScene
{
    public class HoldAimScript : MonoBehaviour
    {
        [SerializeField] private Transform shootTransform;
        [SerializeField] private int bounceCount;
        [SerializeField] private Transform[] points;

        private BoxCollider2D _boxCollider;
        private LineRenderer _lineRenderer;
 
        private Vector3 _tempPos = Vector3.zero;
        private bool _isTouched;


        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _boxCollider = GetComponent<BoxCollider2D>();
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_isTouched)
            {
                _lineRenderer.material.mainTextureOffset = new Vector2(-Time.time*4, 0);
            }
        }

        #region MouseHandlers

        private void OnMouseEnter()
        {
            _isTouched = true;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(true);
            }
        }

        private void OnMouseDown()
        {
            _isTouched = true;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(true);
            }
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
            _isTouched = false;
            _lineRenderer.positionCount = 0;
            foreach (Transform tr in points)
            {
                tr.gameObject.SetActive(false);
            }
        }

        #endregion
    

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
        
            for(int i = 1; i<bounceCount; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(origin, direction,
                    Mathf.Infinity, LayerMask.GetMask("TapSection"));
                
                direction = Vector2.Reflect(direction.normalized, hit.normal);
                origin = hit.point + 0.01f * direction;
            
                _lineRenderer.SetPosition(i, origin);
                points[i-1].position = origin;
            }

        }
    
    
    }
}
