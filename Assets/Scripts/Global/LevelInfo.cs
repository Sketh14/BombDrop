using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BombDrop.Global
{
    [CreateAssetMenu(fileName = "Level_Info", menuName = "ScriptableObjects/Level Info")]
    public class LevelInfo : ScriptableObject
    {
        public string LevelHash;
    }
}
