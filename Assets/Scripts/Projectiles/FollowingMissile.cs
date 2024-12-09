// #define OFFSET_ANGLE_TEST

/*
* These will be similar to PlayerBomb in regards of Rigibody mechanics
* FollowingMissiles : These will be slower than the player and follow the player
*/

using FrontLineDefense.Global;
using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class FollowingMissile : ProjectileBase
    {
        // private Vector3 _prevSpeedVec;
        private float _xyOffsetAngle = 5;

        protected override void OnDisable()
        {
            base.OnDisable();
            _CurrentSpeedMult = 0f;
        }

        protected override void Update()
        {
            // Vector3 projectedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;
            // _SpeedVec = Vector3.Lerp(_SpeedVec, projectedVec, _turnSpeed);
            _SpeedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;

            // Doesnt work as intended | Missile sometimes slows down and takes a sharp turn
#if OFFSET_ANGLE_TEST
            //Offsetting by certain angle
            /*
            *   |   cosA    -sinA   |
            *   |   sinA    cosA    |
            *   x' = x*cosA - y*sinA; y' = x*sinA + y*cosA;
            */
            _SpeedVec.x -= (_SpeedVec.x * Mathf.Cos(_xyOffsetAngle) - _SpeedVec.y * Mathf.Sin(_xyOffsetAngle));
            _SpeedVec.y -= (_SpeedVec.x * Mathf.Sin(_xyOffsetAngle) + _SpeedVec.y * Mathf.Cos(_xyOffsetAngle));
            // _SpeedVec.y *= _turnSpeed;
#endif

            if (_CurrentSpeedMult < _SpeedMult)
                _CurrentSpeedMult += Time.deltaTime * 7f;
            // _currentSpeedMult += Time.deltaTime * 7f;
            // _currentSpeedMult = Mathf.Clamp(_currentSpeedMult, 1, _SpeedMult);

            base.Update();
        }
    }
}
