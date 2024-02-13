using System;
using System.Collections;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.LevelScene.Mobs
{
    public class DefaultMob:MonoBehaviour
    {
        [SerializeField] private MobType type;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private SpriteRenderer spriteMask;
        
        
        protected IOnBulletHit BulletHit
        {
            get;
            set;
        }

        protected IOnDeath Death
        {
            get;
            set;
        }

        protected IOnWaveEnd WaveEnd
        {
            get;
            set;
        }

        protected int Health
        {
            get;
            set;
        }

        public MobType Type => type;

        private SpriteMask _mask;
        
        private BoxCollider2D _boxCollider;

        private int _mobNum;
        
        private Vector3 _startScale;
        private Color _clr;

        private Coroutine _hitRoutine;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _mask = sprite.GetComponent<SpriteMask>();
        }

        private void Start()
        {
            Health = GameVariables.MobInfos[type].Health;
            _clr = sprite.color;
            _startScale = transform.localScale;
            _mobNum = Random.Range(0, GameVariables.MobInfos[type].SkinCount);
            sprite.sprite = GameVariables.MobInfos[type].Sprites[_mobNum*GameVariables.MobInfos[type].FramesPerSkin];
            StartCoroutine(MobIdleRoutine());
            StartCoroutine(WaveEndRoutine());
            StartCoroutine(MobFadeout());
            SetStats();
        }

        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, () => StartCoroutine(WaveEndRoutine()));
        }

        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand, () => StartCoroutine(WaveEndRoutine()));
        }

        protected virtual void SetStats()
        {
            BulletHit ??= gameObject.AddComponent<DefaultBulletHit>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Bullet"))
            {
                if (_hitRoutine != null)
                {
                    StopCoroutine(_hitRoutine);
                }
                _hitRoutine = StartCoroutine(MobHitRoutine());
                BulletHit?.TakeDamage(GameVariables.BulletDamage);
            }
        }

        public void ReduceHealth(int dmg)
        {
            Health -= dmg;
            if (Health > 0) return;
            Death?.DeathEvent();
            StartCoroutine(MobDeathRoutine());
        }
        
        private IEnumerator MobFadeout()
        {
            Color startClr = new Color(1, 1, 1, 0);
            float t = 0;
            while (t < 1)
            {
                sprite.color = Color.Lerp(startClr, Color.white, t);
                t += Time.deltaTime * 2;
                yield return null;
            }

            sprite.color = Color.white;
        }
        
        private IEnumerator WaveEndRoutine()
        {
            float t = 0;
            float startY = transform.localPosition.y;
            
            AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

            Vector3 pos = transform.localPosition;
            
            while (t<1)
            {
                pos.y = Mathf.Lerp(startY, startY - 1, curve.Evaluate(t));
                transform.localPosition = pos;
                t += Time.deltaTime * 2;
                yield return null;
            }

            pos.y = startY - 1;
            transform.localPosition = pos;
            WaveEnd?.WaveEnd();

        }

        private IEnumerator MobDeathRoutine()
        {
            _boxCollider.enabled = false;
            _mask.enabled = true;
            sprite.sprite = GameVariables.MobInfos[type].Sprites[_mobNum*GameVariables.MobInfos[type].FramesPerSkin];

            Color maskClr = Color.white;
            Color spriteClr = Color.white;

            Color endClr = new Color(1, 1, 1, 0);
            
            maskClr.a = 0.3f;

            Vector3 startPos = transform.localPosition;
            Vector3 movePos = startPos;
            movePos.y += 1;
            
            float t = 0;
            while (t<1)
            {
                spriteMask.color = Color.Lerp(maskClr, endClr, t);
                sprite.color = Color.Lerp(spriteClr, endClr, t);

                transform.localPosition = Vector3.Lerp(startPos, movePos, t);
                
                t += Time.deltaTime * 2;
                yield return null;
            }

            Destroy(gameObject);

        }

        private IEnumerator MobIdleRoutine()
        {
            WaitForSeconds threshold = new WaitForSeconds(Mathf.Clamp(Random.Range(GameVariables.MobInfos[type].AnimationThreshold-1, GameVariables.MobInfos[type].AnimationThreshold+1), 0, int.MaxValue));
            float time = Random.Range(0.05f, 0.25f);
            WaitForSeconds timeWait = new WaitForSeconds(time);
            while (true)
            {
                yield return threshold;
                for (int i = 0; i < GameVariables.MobInfos[type].FramesPerSkin; i++)
                {
                    yield return new WaitUntil(() => _hitRoutine == null);
                    sprite.sprite = GameVariables.MobInfos[type].Sprites[_mobNum * GameVariables.MobInfos[type].FramesPerSkin + i];
                    yield return timeWait;
                }

                yield return null;
            }
        }

        private IEnumerator MobHitRoutine()
        {
            sprite.sprite = GameVariables.MobInfos[type].Sprites[GameVariables.MobInfos[type].SkinCount * GameVariables.MobInfos[type].FramesPerSkin + _mobNum];
            
            float t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(_startScale, _startScale-Vector3.one*0.1f, t);
                t += Time.deltaTime * 8;
                yield return null;
            }

            sprite.color = new Color(1, 0.5f, 0.7f);
            
            transform.localScale = _startScale-Vector3.one*0.2f;
            
            t = 0;
            while (t < 1)
            {
                transform.localScale = Vector3.Lerp(_startScale-Vector3.one*0.1f,_startScale,  t);
                t += Time.deltaTime * 8;
                yield return null;
            }
            transform.localScale = _startScale;
            sprite.color = _clr;
            sprite.sprite = GameVariables.MobInfos[type].Sprites[_mobNum*GameVariables.MobInfos[type].FramesPerSkin];
            _hitRoutine = null;
        }
    }
}