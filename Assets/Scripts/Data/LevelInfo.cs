using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelInfo", menuName = "Gameplay/New LevelInfo")]
    public class LevelInfo:ScriptableObject
    {
        [SerializeField] private GameObject[] mobPrefabs;
        [SerializeField] private GameObject[] floorPrefabs;
        
        [SerializeField] [Range(0.1f, 1)] private float mobQuantity;
        [SerializeField] private AnimationCurve mobProbabilityCurve;
        [SerializeField] private AnimationCurve floorProbabilityCurve;

        public GameObject[] MobPrefabs => mobPrefabs;
        public GameObject[] FloorPrefabs => floorPrefabs;
        public float MobQuantity => mobQuantity;
        public AnimationCurve MobProbability => mobProbabilityCurve;
        public AnimationCurve FloorProbability => floorProbabilityCurve;
    }
}