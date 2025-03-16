/*
* The Rigidbody component will not affect the bomb. It is just used to detect collisions with other projectiles/objects
*/

using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class PlayerBomb : ProjectileBase
    {
        // [SerializeField] private float _scalePhysics;
        [SerializeField] private float _turnSpeed = 1f;

        protected override void OnDisable()
        {
            base.OnDisable();
            // if (!_GradualSpeedIncrease)
            _CurrentSpeedMult = _SpeedMult;
        }

        //This should be called after SetStats
        protected override void OnEnable()
        {
            _CurrentSpeedMult = _SpeedMult;
            _CurrentTurnSpeed = _turnSpeed;
            _SpeedVec += Vector3.down;

            base.OnEnable();
        }

        /*protected override void Update()
        {
            // _SpeedVec = _SpeedVec + new Vector3(0f, Global.UniversalConstants.Gravity * _scalePhysics, 0f).normalized * Time.deltaTime;

            // Apply Movement
            // transform.position = transform.position + (_SpeedVec * Time.deltaTime * _CurrentSpeedMult);
            // _SpeedVec = (_SpeedVec + new Vector3(0f, Global.UniversalConstants._gravity * _scalePhysics, 0f) * Time.deltaTime).normalized;
            base.Update();
        }*/
    }
}
