using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public static class GameVariables
    {
        public static int CurrentLevel;
        
        public static int BulletCount = 3;
        public static int BulletDamage = 1;

        public static Dictionary<MobType, MobInfo> MobInfos;
        public static List<LevelInfo> LevelInfos;
    }
}