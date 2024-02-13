using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "Gameplay/New LevelInfo")]
    public class LevelInfo:ScriptableObject
    {
        [SerializeField] private GameObject[] mobPrefabs;
        
        [SerializeField] private float mobQuantity;
        [SerializeField] [Range(0.1f, 0.9f)] private float mobDifficulty;

        public GameObject[] MobPrefabs => mobPrefabs;
        public float MobQuantity => mobQuantity;
        public float MobDifficulty => mobDifficulty;
    }
}