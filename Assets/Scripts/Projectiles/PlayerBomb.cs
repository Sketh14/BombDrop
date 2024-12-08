/*
* The Rigidbody component will not affect the bomb. It is just used to detect collisions with other projectiles/objects
*/

using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class PlayerBomb : ProjectileBase
    {
        [SerializeField] private float _scalePhysics;

        protected override void OnDisable()
        {
            base.OnDisable();
            // if (!_GradualSpeedIncrease)
            _CurrentSpeedMult = _SpeedMult;
        }

        protected override void OnEnable()
        {
            _CurrentSpeedMult = _SpeedMult;
        }

        protected override void Update()
        {
            base.Update();

            _SpeedVec = _SpeedVec + new Vector3(0f, Global.UniversalConstants._gravity * _scalePhysics, 0f) * Time.deltaTime;
            // _SpeedVec = (_SpeedVec + new Vector3(0f, Global.UniversalConstants._gravity * _scalePhysics, 0f) * Time.deltaTime).normalized;
        }
    }
}
