using System;
using UnityEngine;

namespace FrontLineDefense.Global
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;
        public static GameManager Instance { get => _instance; }

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);
        }
        #endregion Singleton

        public Transform PlayerTransform;

        #region Actions
        public Action<Vector3[]> OnMapGenerated;

        /// <summary> 0 : Bomb Drop | 1 : Player Hit </summary>
        public Action<float, int> OnPlayerAction;
        #endregion Actions
    }
}
