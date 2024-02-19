using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Mobs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.LevelScene.Mobs
{
    public class DefaultMob:MonoBehaviour
    {
        [SerializeField] private MobType type;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private SpriteRenderer spriteMask;
        
        
        public IOnBulletHit BulletHit
        {
            get;
            set;
        }

        public IOnDeath Death
        {
            get;
            set;
        }

        public IOnWaveEnd WaveEnd
        {
            get;
            set;
        }

        public float Health
        {
            get;
            private set;
        }

        public int MaxHealth
        {
            get;
            private set;
        }

        public Dictionary<DamageType, float> Resists
        {
            get;
            private set;
        }

        private HpBarScript _hpBar;
        
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
            _hpBar = GetComponentInChildren<HpBarScript>();
        }

        private void Start()
        {
            if(Camera.main.aspect != 9f/16)
                transform.localScale = Vector3.one * Camera.main.aspect / (9f / 16);
            
            Resists = new Dictionary<DamageType, float>();
            Resists.Add(DamageType.Normal, GameVariables.MobInfos[type].NormalResist);
            Resists.Add(DamageType.Fire, GameVariables.MobInfos[type].FireResist);
            Resists.Add(DamageType.Thunder, GameVariables.MobInfos[type].ThunderResist);
            Resists.Add(DamageType.Cold, GameVariables.MobInfos[type].ColdResist);
            Resists.Add(DamageType.Pure, 1);
            Health = GameVariables.MobInfos[type].Health;
            MaxHealth = GameVariables.MobInfos[type].Health;
            _clr = sprite.color;
            
            _startScale = transform.localScale;
            _mobNum = Random.Range(0, GameVariables.MobInfos[type].SkinCount);
            spriteMask.enabled = false;
            sprite.color = Color.clear;
            sprite.sprite = GameVariables.MobInfos[type].Sprites[_mobNum*GameVariables.MobInfos[type].FramesPerSkin];
            StartCoroutine(MobIdleRoutine());
            StartCoroutine(MobSpawnRoutine());
            SetStats();
        }

        private void OnEnable()
        {
            LevelEventBus.Subscribe(LevelEventBus.LevelEventType.BulletLand, DoWaveEnd);
        }

        private void OnDisable()
        {
            LevelEventBus.Unsubscribe(LevelEventBus.LevelEventType.BulletLand, DoWaveEnd);
        }

        private void DoWaveEnd()
        {
            StartCoroutine(WaveEndRoutine());
        }

        protected virtual void SetStats()
        {
            BulletHit ??= gameObject.AddComponent<DefaultBulletHit>();
        }

        public void ResetStats()
        {
            Health = MaxHealth;
            _boxCollider.enabled = true;
            _mask.enabled = false;
            spriteMask.enabled = true;
            StartCoroutine(MobSpawnRoutine());
            StartCoroutine(MobIdleRoutine());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Bullet"))
            {
                BulletHit?.TakeDamage(GameVariables.BulletDamage, GameVariables.ActiveBulletType, other.transform.position);
            }
        }

        public void ReduceHealth(float dmg, DamageType type)
        {
            if(Health==0 && dmg >0) return;
            
            if (_hitRoutine != null)
            {
                StopCoroutine(_hitRoutine);
            }

            _hitRoutine = StartCoroutine(MobHitRoutine());
            
            dmg *= Resists[type];
            
            Health -= dmg;
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            
            if (Health > 0)
            {
                _hpBar.TakeDamage(dmg);
                return;
            }
            
            _hpBar.SetZeroHp();
            StartCoroutine(MobDeathRoutine());
            
        }
        
        private IEnumerator MobSpawnRoutine()
        {
            yield return null;
            float startY = transform.localPosition.y;
            AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            Vector3 pos = transform.localPosition;

            Color startClr = _clr;
            startClr.a = 0;
            
            float t = 0;
            while (t < 1)
            {
                pos.y = Mathf.Lerp(startY, startY - 1, curve.Evaluate(t));
                transform.localPosition = pos;
                sprite.color = Color.Lerp(startClr, _clr, t);
                t += Time.deltaTime * 4;
                yield return null;
            }
            
            pos.y = startY - 1;
            transform.localPosition = pos;
            sprite.color = _clr;
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
                t += Time.deltaTime * 4;
                yield return null;
            }

            pos.y = startY - 1;
            transform.localPosition = pos;
            WaveEnd?.WaveEnd();

        }

        private IEnumerator MobDeathRoutine()
        {
            _boxCollider.enabled = false;
            
            Death?.DeathEvent();

            spriteMask.enabled = true;
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
            
            MobPool.Instance.Put(this, Type);

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