using UnityEngine;

using FrontLineDefense.Global;

namespace FrontLineDefense.Enemy
{
    public class EnemyMissileCarrier : EnemyVehicleController_Base
    {
        [SerializeField] private float _rotateSpeed;
        private const float _maxZRotateAngle = -12f, _minRotateAngle = -178f;

        // float zRotateAngle;         //Debugging
        protected override void TargetPlayer()
        {
            if (_ShotProjectileStatus != (byte)ShootStatus.AVAILABLE_TO_SHOOT
                && _ShotProjectileStatus != (byte)ShootStatus.RECHARGING) return;

            Vector3 _playerDirection = _PlayerTransform.position - _Turret.position;
            // _partToRotate.rotation = Quaternion.LookRotation(_playerDirection);

            //calculate the angle in radians and convert to  degrees
            // zRotateAngle = (Mathf.Atan2(_playerDirection.y, _playerDirection.x) * Mathf.Rad2Deg) - 180f;
            float zRotateAngle = (Mathf.Atan2(_playerDirection.y, _playerDirection.x) * Mathf.Rad2Deg) - 180f;
            // zRotateAngle = Mathf.Clamp(zRotateAngle, 100f, 150f);
            if (zRotateAngle >= _minRotateAngle && zRotateAngle <= _maxZRotateAngle)
                _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, 0f, zRotateAngle), _rotateSpeed * Time.deltaTime);
        }

        protected override async void Shoot()
        {
            _ShotProjectileStatus = (byte)ShootStatus.RECHARGING;
            await _CtTimer.WaitForSeconds(1f);
            _ShotProjectileStatus = (byte)ShootStatus.SEARCHING_PLAYER;
            await _CtTimer.WaitForSeconds(1f);
            _ShotProjectileStatus = (byte)ShootStatus.RECHARGE_DONE;
        }
    }
}