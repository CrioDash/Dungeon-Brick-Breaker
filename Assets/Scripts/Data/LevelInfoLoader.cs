using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LevelInfoLoader:MonoBehaviour
    {
        private void Awake()
        {
            if(GameVariables.LevelInfos!=null)
                return;
            GameVariables.LevelInfos = new List<LevelInfo>();
            LevelInfo[] info = Resources.LoadAll<LevelInfo>("Levels/");
            for(int i=0;i<info.Length;i++)
            {
                GameVariables.LevelInfos.Add(info[i]);
            }
        }
    }
}