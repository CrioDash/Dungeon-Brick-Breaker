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
        
        [Header("Sprite settings")]
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int skinCount;
        [SerializeField] private int framesPerSkin;
        [SerializeField] private float animationThreshold;

        public int Health => health;
        public MobType Type => type;
        
        public Sprite[] Sprites => sprites;
        public int SkinCount => skinCount;
        public int FramesPerSkin => framesPerSkin;
        public float AnimationThreshold => animationThreshold;
        
        
        
    }
}