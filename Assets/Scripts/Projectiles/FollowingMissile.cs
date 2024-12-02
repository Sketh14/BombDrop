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
        protected override void Update()
        {
            base.Update();

            _SpeedVec = (GameManager.Instance.PlayerTransform.position - transform.position).normalized * _SpeedMult;
        }
    }
}
