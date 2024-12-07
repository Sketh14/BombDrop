// #define TARGETING_TEST

using UnityEngine;

using FrontLineDefense.Global;
using FrontLineDefense.Projectiles;

namespace FrontLineDefense.Enemy
{
    public class EnemyMissileCarrier : EnemyVehicleController_Base
    {
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private PoolManager.PoolType _missilePool;
        [SerializeField] private bool rotateY;
        private const float _maxZRotateAngle = -12f, _minRotateAngle = -178f;
        private const float _searchWaitTime = 2f;

        // public float zRotateAngle;         //Debugging
        // public float yRotateAngle;         //Debugging
        // public Vector3 playerDirection;        //Debugging
        protected override void TargetPlayer()
        {
            // if (_ShotProjectileStatus != (byte)ShootStatus.AVAILABLE_TO_SHOOT
            //     && _ShotProjectileStatus != (byte)ShootStatus.RECHARGING) return;
#if !TARGETING_TEST
            if (_ShotProjectileStatus != (byte)ShootStatus.SEARCHING_PLAYER) return;
#endif

            Vector3 playerDirection = GameManager.Instance.PlayerTransform.position - _Turret.position;
            // _partToRotate.rotation = Quaternion.LookRotation(_playerDirection);

            if (rotateY)
            {
                //calculate the angle in radians and convert to  degrees
                float yRotateAngle = (Mathf.Atan2(playerDirection.z, playerDirection.x) * Mathf.Rad2Deg) - 180f;
                _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, yRotateAngle, -30f), _rotateSpeed * Time.deltaTime);
            }
            else
            {
                //calculate the angle in radians and convert to  degrees
                // zRotateAngle = (Mathf.Atan2(_playerDirection.y, _playerDirection.x) * Mathf.Rad2Deg) - 180f;
                float zRotateAngle = (Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg) - 180f;
                // zRotateAngle = Mathf.Clamp(zRotateAngle, 100f, 150f);
                if (zRotateAngle >= _minRotateAngle && zRotateAngle <= _maxZRotateAngle)
                    _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, 0f, zRotateAngle), _rotateSpeed * Time.deltaTime);
            }
        }

        protected override async void Shoot()
        {
            GameObject shotProjectile = PoolManager.Instance.ObjectPool[(int)_missilePool].Get();
            shotProjectile.transform.position = _ShootPoint.position;
            shotProjectile.transform.rotation = _ShootPoint.rotation;
            shotProjectile.GetComponent<ProjectileBase>().SetStats(transform.right * -1.0f);
            shotProjectile.SetActive(true);

            _ShotProjectileStatus = (byte)ShootStatus.RECHARGING;
            await _CtTimer.WaitForSeconds(_ShootCooldown);
            _ShotProjectileStatus = (byte)ShootStatus.SEARCHING_PLAYER;
            await _CtTimer.WaitForSeconds(_searchWaitTime);
            _ShotProjectileStatus = (byte)ShootStatus.AVAILABLE_TO_SHOOT;
        }
    }
}