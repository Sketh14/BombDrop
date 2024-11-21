using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontLineDefense
{
    public class EnemyMissileCarrier : EnemyVehicleController_Base
    {
        private const float _maxZRotateAngle = -12f, _minRotateAngle = -168f;
        [SerializeField] private float _rotateSpeed;

        protected override void TargetPlayer()
        {
            Vector3 _playerDirection = _PlayerTransform.position - _Turret.position;
            // _partToRotate.rotation = Quaternion.LookRotation(_playerDirection);

            //calculate the angle in radians and convert to  degrees
            float zRotateAngle = (Mathf.Atan2(_playerDirection.y, _playerDirection.x) * Mathf.Rad2Deg) - 180f;
            // zRotateAngle = Mathf.Clamp(zRotateAngle, 100f, 150f);
            if (zRotateAngle >= _minRotateAngle && zRotateAngle <= _maxZRotateAngle)
                _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, 0f, zRotateAngle), _rotateSpeed * Time.deltaTime);
        }

        protected override void Shoot()
        {

        }
    }
}