using System;
using UnityEngine;

namespace BombDrop.Global
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
        public bool PlayerDead;
        public int PlayerCoins;
        // public AudioManager AudioManagerRef;
        public LevelInfo LevelInfo;


        #region Actions
        public Action<Vector3[]> OnMapGenerated;

        /// <summary> 0 : Bomb Drop | 1 : Player Hit | 2 : Player Dead | 3 : Coin Collected </summary>
        public Action<float, int> OnPlayerAction;
        public Action<int> OnButtonClicked, OnBoundariesEntered;
        public Action<Vector3, PoolManager.PoolType> OnProjectileHit;
        #endregion Actions
    }
}
