using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Data
{
    public class MobInfoLoader:MonoBehaviour
    {
        private void Awake()
        {
            if(GameVariables.MobInfos!=null)
                return;
            GameVariables.MobInfos = new Dictionary<MobType, MobInfo>();
            MobInfo[] info = Resources.LoadAll<MobInfo>("Mobs/");
            for(int i=0;i<info.Length;i++)
            {
                GameVariables.MobInfos.Add(info[i].Type, info[i]);
            }
        }
    }
}