using UnityEngine;

using FrontLineDefense.Global;
using FrontLineDefense.Projectiles;

namespace FrontLineDefense.Enemy
{
    public class EnemyAA : EnemyVehicleController_Base
    {
        [SerializeField] private float _rotateSpeed;
        private const float _maxZRotateAngle = -12f, _minRotateAngle = -178f;
        private float _shootTime;

        // float zRotateAngle;         //Debugging
        protected override void TargetPlayer()
        {
            Vector3 _playerDirection = GameManager.Instance.PlayerTransform.position - _Turret.position;

            //calculate the angle in radians and convert to  degrees
            float zRotateAngle = (Mathf.Atan2(_playerDirection.y, _playerDirection.x) * Mathf.Rad2Deg) - 180f;
            if (zRotateAngle >= _minRotateAngle && zRotateAngle <= _maxZRotateAngle)
                _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, 0f, zRotateAngle), _rotateSpeed * Time.deltaTime);
        }

        protected override void Shoot()
        {
            // Debug.Log($"SHooting");
            _ShotProjectileStatus = (byte)ShootStatus.SHOT_PROJECTILE;
            _shootTime = 0f;
            GameObject shotProjectile = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.AA_BULLET].Get();
            shotProjectile.transform.position = _ShootPoint.position;
            shotProjectile.transform.rotation = _ShootPoint.rotation;
            // shotProjectile.GetComponent<ProjectileBase>().SetStats(_Turret.right * -1.0f);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            //Shoot in some intervals
            // We are not looking for perfect time-interval, so this will do to reduce performance cost
            if (_shootTime > _ShootCooldown)
            {
                // Debug.Log($"Shoot Available");
                _ShotProjectileStatus = (byte)ShootStatus.AVAILABLE_TO_SHOOT;
            }
            else
                _shootTime += Time.fixedDeltaTime;
        }
    }
}