// #define TEST_SHOOT_OFF

using System.Threading;
using UnityEngine;

using BombDrop.Utils;
using BombDrop.Global;

namespace BombDrop.Enemy
{
    public abstract class EnemyVehicleController_Base : MonoBehaviour, IStatComponent
    {
        // Some turrets might rotate a full 180 on z-axis | some will rotate to some 
        // constraints on z-axis and then rotate on y-axis to face the player 
        // [SerializeField] protected Transform _PlayerTransform, _Turret;
        [SerializeField] protected Transform _Turret;
        [SerializeField] protected PoolManager.PoolType _VehiclePoolType;
        [SerializeField] protected float _ShootCooldown, _DetectionRange, _Health, _SearchWaitTime;
        [SerializeField] protected Transform _ShootPoint;
        [SerializeField] protected RectTransform _healthBarRect;
        [SerializeField] protected int _CoinAmount;
        /// <summary> 0: Available to Shoot | 1 : Shot | 2 : Recharging | 3 : Recharging Complete </summary>
        protected int _ShotProjectileStatus;
        protected bool _ReleasedToPool;
        protected float _OgHealth;
        // private float _searchTime;

        protected CustomTimer _CtTimer;
        protected CancellationTokenSource _Cts;

        protected virtual void OnDisable()
        {
            // _Health = _OgHealth;                //To reset stats if needed
            _Cts.Cancel();
            GameManager.Instance.OnProjectileHit -= CheckForHit;
        }

        protected virtual void OnEnable()
        {
            _ShotProjectileStatus |= 1 << (int)ShootStatus.SEARCHING_PLAYER;
            _ShotProjectileStatus |= 1 << (int)ShootStatus.AVAILABLE_TO_SHOOT;
            // Debug.Log($"Enabling ShootStatus : {_ShotProjectileStatus}");
            _Cts = new CancellationTokenSource();


            GameManager.Instance.OnProjectileHit += CheckForHit;
        }

        protected virtual void Start()
        {
            _OgHealth = _Health;
            _CtTimer = new CustomTimer();
        }

        protected virtual void FixedUpdate()
        {
            if (Mathf.Abs(GameManager.Instance.PlayerTransform.position.x - transform.position.x) <= _DetectionRange)
            {
                TargetPlayer();

                // if (!GameManager.Instance.PlayerDead && (_ShotProjectileStatus == (byte)ShootStatus.AVAILABLE_TO_SHOOT)
                if (!GameManager.Instance.PlayerDead
                    // && (_ShotProjectileStatus & (1 << ((int)ShootStatus.AVAILABLE_TO_SHOOT | (int)ShootStatus.FOUND_PLAYER))) != 0)
                    && (_ShotProjectileStatus & (1 << (int)ShootStatus.AVAILABLE_TO_SHOOT)) != 0
                    && (_ShotProjectileStatus & (1 << (int)ShootStatus.FOUND_PLAYER)) != 0)
                {
                    // Debug.Log($"Player Dead : {GameManager.Instance.PlayerDead}");
                    Shoot();
                }
            }
            else
            {
                _ShotProjectileStatus &= ~(1 << (int)ShootStatus.FOUND_PLAYER);
            }

            //Shoot in some intervals
            // We are not looking for perfect time-interval, so this will do to reduce performance cost
            /*if (_searchTime > _ShootCooldown && _ShotProjectileStatus == (byte)ShootStatus.FOUND_PLAYER)
            {
                _ShotProjectileStatus = (byte)ShootStatus.AVAILABLE_TO_SHOOT;
                _searchTime = 0f;
            }
            else
                _searchTime += Time.fixedDeltaTime;
            */
        }

        protected abstract void TargetPlayer();
        protected abstract void Shoot();

        private void CheckForHit(Vector3 explosionPosition, PoolManager.PoolType explosionPoolType, BombStatus bombStatus)
        {
            if (bombStatus != BombStatus.HIT_OTHER) return;

            float damageDealt = 1f;
            switch (explosionPoolType)
            {
                case PoolManager.PoolType.FOLLOWING_MISSILES:
                    damageDealt = UniversalConstants.FollowingMissileDamage;
                    break;

                case PoolManager.PoolType.STRAIGHT_RANGED_MISSILES:
                    damageDealt = UniversalConstants.StraightMissileDamage;
                    break;

                case PoolManager.PoolType.BOMB:
                    damageDealt = UniversalConstants.PlayerBombDamage;
                    break;
            }

            float distanceSquared = Vector3.SqrMagnitude(explosionPosition - transform.position);
            if (distanceSquared < (UniversalConstants.MissileDamageRange * UniversalConstants.MissileDamageRange))
                TakeDamage(damageDealt * (1f / distanceSquared * 14f));                      //Min Distance Squared Possibly : 14.44118
            // Debug.Log($"Name : {gameObject.name} | distanceSquared : {distanceSquared}"
            // + $" | Damage Dealt * 11 : {damageDealt * (1f / distanceSquared * 14f)} |"
            // + $"Damage Dealt: {damageDealt * (1f / distanceSquared)} ");
        }

        public virtual void TakeDamage(float damageTaken)
        {
            if (_ReleasedToPool) return;

            _Health -= damageTaken;
            _healthBarRect.anchorMax = new Vector2(_Health / _OgHealth, 1f);
            if (_Health <= 0f)
            {
                /*GameObject coinInstantiated;
                // Release coins to give to player
                for (int i = 0; i < _coinAmount; i++)
                {
                    coinInstantiated = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.COIN].Get();
                    coinInstantiated.transform.position = _ShootPoint.position;
                    coinInstantiated.SetActive(true);
                }*/

                // GameManager.Instance.EnemiesLeft--;
                if (--GameManager.Instance.EnemiesLeft <= 0)
                    GameManager.Instance.OnPlayerAction?.Invoke(1f, (int)PlayerAction.LEVEL_CLEARED);

                _ReleasedToPool = true;
                GameManager.Instance.PlayerCoins += _CoinAmount;
                PoolManager.Instance.ObjectPool[(int)_VehiclePoolType].Release(gameObject);
                GameManager.Instance.OnPlayerAction?.Invoke(_CoinAmount, (int)PlayerAction.COIN_COLLECTED);
            }
        }
    }

    public abstract class EnemyMissileCarrier : EnemyVehicleController_Base
    {
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private PoolManager.PoolType _missilePool;
        // protected const float _searchWaitTime = 2f;

        protected virtual async void RechargeShoot()
        {
            _ShotProjectileStatus = (byte)ShootStatus.RECHARGING;
            await _CtTimer.WaitForSeconds(_ShootCooldown);
            _ShotProjectileStatus = (byte)ShootStatus.SEARCHING_PLAYER;
            await _CtTimer.WaitForSeconds(_SearchWaitTime);
            _ShotProjectileStatus = (byte)ShootStatus.AVAILABLE_TO_SHOOT;
        }
    }
}