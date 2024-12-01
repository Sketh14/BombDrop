/*
* These will be similar to Player_Bomb in regards of Rigibody mechanics
* These will be slower than the player and follow the player
*/

using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class EnemyMissile : ProjectileBase
    {
        [SerializeField] private Transform _playerTransform;

        /*
        * There will be a delay in the change of direciton of the missile?
        */
        protected override void Update()
        {
            base.Update();

            _SpeedVec = (_playerTransform.position - transform.position).normalized * _SpeedMult;
        }
    }
}
