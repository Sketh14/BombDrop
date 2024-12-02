/*
* These will be similar to PlayerBomb in regards of Rigibody mechanics
* FollowingMissiles : These will be slower than the player and follow the player
*/

using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class FollowingMissile : ProjectileBase
    {
        [SerializeField] private Transform _playerTransform;

        protected override void Update()
        {
            base.Update();

            _SpeedVec = (_playerTransform.position - transform.position).normalized * _SpeedMult;
        }
    }
}
