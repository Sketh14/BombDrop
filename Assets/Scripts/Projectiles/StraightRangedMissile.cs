/*
* These will be similar to PlayerBomb in regards of Rigibody mechanics
* StraightRangedMissiles : These could be slower than the player and shoot upto a distance
*/

using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class StraightRangedMissile : ProjectileBase
    {
        [SerializeField] private Transform _playerTransform;

        protected override void Update()
        {
            base.Update();

            _SpeedVec = (_playerTransform.position - transform.position).normalized * _SpeedMult;
        }
    }
}
