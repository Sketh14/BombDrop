using System;

using UnityEngine;
using UnityEngine.UI;

using FrontLineDefense.Global;
using FrontLineDefense.Projectiles;

// using UnityEngine.InputSystem.
namespace FrontLineDefense.Player
{
    public class PlayerController : MonoBehaviour, IStatComponent
    {
        [SerializeField] private JoyStickController joyStick;
        [SerializeField] private float _speedMult = 5f;

        //New
        [SerializeField] private float _health = 100.0f;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Button _shootProjectile;
        // [SerializeField] private GameObject _projectilePrefab;           //test
        [SerializeField] private Transform _bombPoint;
        private Transform _planeMesh;
        /// <summary> 0 : Left | 1 : Right | 2 : In Process of turning </summary>
        private byte _planeMeshRotateMult;
        private const float _positionLerpVal = 0.35f;

        private void Start()
        {
            _shootProjectile.onClick.AddListener(ShootProjectile);
            _planeMesh = transform.GetChild(0);
            _planeMeshRotateMult = 1;
        }

        /*
        * Upward and Downward loop will turn the mesh of the Plane
        */
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
                ShootProjectile();
#endif

            Vector2 input = joyStick.GetInputDirection();

            Vector2 direction = new Vector2(input.x, input.y).normalized;

            // Move the airplance based on joystick input

            //

            if (direction.magnitude > 0.1f)
            {
                //calculate the angle in radians and convert to  degrees 
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Apply rotation to the airplace to point in the direction of movement
                // transform.rotation = Quaternion.Euler(0, 0, angle - 180);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 180), _rotateSpeed * Time.deltaTime);

            }

            //Apply rotation to the child mesh body when direction is changed
            // Switch between 180 | 0 
            if (transform.right.x < -0.3f && (_planeMeshRotateMult == 1 || _planeMeshRotateMult == 2))
            {
                _planeMeshRotateMult = 2;
                _planeMesh.localRotation = Quaternion.Lerp(_planeMesh.localRotation, Quaternion.Euler(180f, 0f, 0f), _rotateSpeed * Time.deltaTime);
                // Debug.Log($"Negative | transform right : {transform.right} | _planeMeshRotateMult : {_planeMeshRotateMult}"
                // + $" | Euler : {_planeMesh.localEulerAngles} | rotation : {_planeMesh.localRotation}");

                if (_planeMesh.localEulerAngles.y >= 179.99f && _planeMesh.localEulerAngles.x >= 358.0f)
                // if (_planeMesh.localRotation.x <= -0.9999f && _planeMesh.localRotation.w <= 0.09f)           //Gimbal Lock
                {
                    // Debug.Log($"Reached");
                    _planeMesh.localRotation = Quaternion.Euler(180f, 0f, 0f);
                    _planeMeshRotateMult = 0;
                }
            }
            else if (transform.right.x > 0.3f && (_planeMeshRotateMult == 0 || _planeMeshRotateMult == 2))
            {
                _planeMeshRotateMult = 2;
                _planeMesh.localRotation = Quaternion.Lerp(_planeMesh.localRotation, Quaternion.Euler(0, 0f, 0f), _rotateSpeed * Time.deltaTime);
                // Debug.Log($"Positive | transform right : {transform.right} | _planeMeshRotateMult : {_planeMeshRotateMult}"
                // + $" | Euler : {_planeMesh.localEulerAngles} | rotation : {_planeMesh.localRotation}");

                // if (_planeMesh.localRotation.x <= -0.009f && _planeMesh.localRotation.w >= 0.99f)        //Gimbal Lock
                if (_planeMesh.localEulerAngles.y <= 0.09f && _planeMesh.localEulerAngles.x >= 358.0f)
                {
                    // Debug.Log($"Reached");
                    _planeMesh.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    _planeMeshRotateMult = 1;
                }
            }

            transform.Translate(Vector2.left * _speedMult * Time.deltaTime);
        }

        /*private void FixedUpdate()
        {
            Vector3 speedVec = transform.right * -1f * _speedMult;
            transform.position = Vector3.Lerp(transform.position, transform.position + speedVec, _positionLerpVal);

            // transform.position = Vector3.Lerp(transform.position, (transform.position + Vector3.left) * _speedMult, _positionLerpVal);
            // transform.Translate(Vector2.left * speed);
        }*/

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Hit");
        }

        private void ShootProjectile()
        {
            // GameObject shotProjectile = Instantiate(_projectilePrefab, _bombPoint.position, transform.rotation);
            GameObject shotProjectile = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.BOMB].Get();
            shotProjectile.transform.position = _bombPoint.position;
            shotProjectile.transform.rotation = transform.rotation;
            shotProjectile.GetComponent<ProjectileBase>().SetStats(transform.right * -1.0f);
            // Debug.Log($"Shoot Clicked | transform.right : {transform.right} | Namer : {shotProjectile.name}");
        }

        public void TakeDamage(float damageTaken)
        {
            // Debug.Log($"Taking Damage : {damageTaken}");
            _health -= damageTaken;
        }
    }
}