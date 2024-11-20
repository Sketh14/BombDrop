// The Projectiles have custom physics
// Wont be able to achieve through normal implementation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontLineDefense
{
    // [RequireComponent(typeof(Rigidbody))]
    public class ProjectileBase : MonoBehaviour
    {
        // private Rigidbody _projectileRb;

        // Only x matters overall, as y will be affected by gravity
        // Reference : Projectile has its own speed irrespective of plane's speed
        [SerializeField] private float _speedX;
        [SerializeField] private float _scalePhysics;
        private int _directionMult;
        private Vector3 _speedVec;
        private float _inititalRot;
        private const float _gravity = -9.8f;

        // Start is called before the first frame update
        void Start()
        {
            // _projectileRb = GetComponent<Rigidbody>();
            // _projectileRb.AddForce(Vector3.right * 10f, ForceMode.Impulse);      //Test

            _directionMult = 1;
        }

        /*
            The projectile goes forward and changes direction simlutaneously. As it reaches peak, 
            it starts to curve down and proceed downwards.

            |       /        |               |               |
            |      /         |     /         |               |
            |     /          |    /          |   /           |   
            |    ----->      |   ----->      |   ----->      |   ----->
            |                |    \          |   \           |   \ 
            |                |               |    \          |    \        
            |                |               |               |     \
            |___________     |___________    |___________    |___________
            Need to Calculate Curve.
            (-) Move in small Increments
        */
        private void Update()
        {
            //Apply Movement
            transform.position = transform.position + (_speedVec * _directionMult * Time.deltaTime);
            _speedVec = _speedVec + (new Vector3(0f, _gravity, 0f) * Time.deltaTime * _scalePhysics);
        }

        private void FixedUpdate()
        {
            //Apply Rotation
            //calculate the angle in radians and convert to  degrees 
            float angle = Mathf.Atan2(_speedVec.normalized.y, _speedVec.normalized.x) * Mathf.Rad2Deg;

            // Apply rotation to the airplace to point in the direction of movement
            transform.rotation = Quaternion.Euler(0, 0, angle - 180);

            // _projectileRb.AddForce(Vector3.right * 15f, ForceMode.Acceleration);
        }

        public void SetStats(in Vector2 initialSpeed)
        {
            _inititalRot = transform.eulerAngles.z;
            _speedVec = new Vector3(initialSpeed.x * _speedX, initialSpeed.y * _speedX, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Hit | Collider : {other.name} | Tag : {other.tag}");
            gameObject.SetActive(false);
            if (other.CompareTag(UniversalConstants.Player))
            {
                other.GetComponent<PlayerController>().TakeDamage(10f);
            }
        }
    }
}