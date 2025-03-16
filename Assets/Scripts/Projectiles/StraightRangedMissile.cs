/*
* These will be similar to PlayerBomb in regards of Rigibody mechanics
* StraightRangedMissiles : These could be slower than the player and shoot upto a distance
*/

using BombDrop.Global;
using UnityEngine;

namespace BombDrop.Projectiles
{
    public class StraightRangedMissile : ProjectileBase
    {
        private Vector2 _playerTargetedPos;
        private const float _maxTargetDiff = 2f;

        protected override void OnDisable()
        {
            base.OnDisable();
            // if (!_GradualSpeedIncrease)
            _CurrentSpeedMult = _SpeedMult;
        }

        public override void SetStats(in Vector3 initialSpeedVec, in bool leftAligned, in float damageDealt = 0f, in float spedMult = 1f)
        {
            _CurrentSpeedMult = _SpeedMult;
            _CurrentTurnSpeed = 1f;
            _playerTargetedPos = GameManager.Instance.PlayerTransform.position;
            // _SpeedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized;
            _SpeedVec = initialSpeedVec;

            Quaternion lookRotation = Quaternion.LookRotation(_SpeedVec, Vector3.up);
            transform.rotation = lookRotation;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (Vector2.SqrMagnitude(new Vector2(transform.position.x, transform.position.y) - new Vector2(_playerTargetedPos.x, _playerTargetedPos.y))
                <= _maxTargetDiff * _maxTargetDiff)
                PoolManager.Instance.ObjectPool[(int)_PoolToUse].Release(gameObject);
        }
    }
}
