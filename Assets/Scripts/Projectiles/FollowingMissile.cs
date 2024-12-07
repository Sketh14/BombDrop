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

        protected override void OnDisable()
        {
            base.OnDisable();
            _CurrentSpeedMult = 0f;
        }

        protected override void Update()
        {
            base.Update();

            // Vector3 projectedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;
            // _SpeedVec = Vector3.Lerp(_SpeedVec, projectedVec, 0.5f);
            _SpeedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;

            if (_CurrentSpeedMult < _SpeedMult)
                _CurrentSpeedMult += Time.deltaTime * 7f;
            // _currentSpeedMult += Time.deltaTime * 7f;
            // _currentSpeedMult = Mathf.Clamp(_currentSpeedMult, 1, _SpeedMult);
        }
    }
}
