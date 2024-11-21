using UnityEngine;

namespace FrontLineDefense
{
    public abstract class EnemyVehicleController_Base : MonoBehaviour
    {
        // Some turrets might rotate a full 180 on z-axis | some will rotate to some 
        // constraints on z-axis and then rotate on y-axis to face the player 
        [SerializeField] protected Transform _PlayerTransform, _Turret;
        [SerializeField] protected PoolManager.PoolType _PoolToUse;
        [SerializeField] protected float _ShootCooldown, _DetectionRange;
        private float _health, _shootTime;

        void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            TargetPlayer();

            //Shoot in some intervals
        }

        protected abstract void TargetPlayer();
        protected abstract void Shoot();

        protected virtual void TakeDamage(in float damageTaken)
        {
            _health -= damageTaken;
            if (_health <= 0f)
                PoolManager.Instance.ObjectPool[(int)_PoolToUse].Release(gameObject);
        }
    }
}