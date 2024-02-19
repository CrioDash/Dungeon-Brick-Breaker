using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "MobInfo", menuName = "Gameplay/New MobInfo")]
    public class MobInfo:ScriptableObject
    {
        [Header("Stats settings")] 
        [SerializeField] private int health;
        [SerializeField] private MobType type;
        [SerializeField] [Range(0.1f, 5)] private float normalResist;
        [SerializeField] [Range(0.1f, 5)] private float fireResist;
        [SerializeField] [Range(0.1f, 5)] private float thunderResist;
        [SerializeField] [Range(0.1f, 5)] private float coldResist;
        
        [Header("Sprite settings")]
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int skinCount;
        [SerializeField] private int framesPerSkin;
        [SerializeField] private float animationThreshold;

        public int Health => health;
        public MobType Type => type;
        public float NormalResist => normalResist;
        public float FireResist => fireResist;
        public float ThunderResist => thunderResist;
        public float ColdResist => coldResist;
        
        public Sprite[] Sprites => sprites;
        public int SkinCount => skinCount;
        public int FramesPerSkin => framesPerSkin;
        public float AnimationThreshold => animationThreshold;
        
        
        
    }
}