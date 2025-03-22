#define TESTING

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using BombDrop.Global;
using BombDrop.Projectiles;
using BombDrop.Utils;
using System.Threading;


#if TESTING
using UnityEngine.InputSystem;
#endif

// using UnityEngine.InputSystem.
namespace BombDrop.Player
{
    public class PlayerController : MonoBehaviour, IStatComponent
    {
        [SerializeField] private JoyStickController joyStick;
        [SerializeField] private float _speedMult = 5f;

        //New
        [SerializeField] private float _health = 100.0f;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Button _dropBomb;
        [SerializeField] private EventTrigger _shootBullet;
        // [SerializeField] private GameObject _projectilePrefab;           //test
        [SerializeField] private Transform _bombPoint, _shootPoint, _propeller;
        private Transform _planeMesh;
        private GameObject _instancedBullet;

        /// <summary> 0 : Left | 1 : Right | 2 : In Process of turning </summary>
        private byte _planeMeshRotateMult;
        /// <summary> 0 : Available | 1 : Shot </summary>
        private byte _bombStatus;
        /// <summary> 0 : Available | 1 : Shooting </summary>
        private byte _shootStatus;
        private float _ogHealth;
        private float _shootTime;
        private const float _shootInterval = 0.25f;
        // private const float _positionLerpVal = 0.35f;
        private const float _bombCooldownTime = 2f;
        private const float _propellerSpeedMult = 150f;
        private const float _checkBoundaryWaitTime = 1f;

        private CancellationTokenSource _cts;
        private CustomTimer _customTimer;

        private void OnDestroy()
        {
            _cts.Cancel();
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            _dropBomb.onClick.AddListener(DropBomb);
            // _shootBullet.OnSelect((data) => (ShootBullets));
            EventTrigger.Entry downEvent = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
            EventTrigger.Entry upEvent = new EventTrigger.Entry() { eventID = EventTriggerType.PointerUp };
            upEvent.callback.AddListener((eventData) =>
            {
                _shootStatus = 0;
                _shootTime = 0f;
            });
            downEvent.callback.AddListener((eventData) =>
            {
                _shootStatus = 1;
            });
            _shootBullet.triggers.Add(downEvent);
            _shootBullet.triggers.Add(upEvent);

            _ogHealth = _health;

            _planeMesh = transform.GetChild(0);
            _planeMeshRotateMult = (int)PlaneRotateStatus.RIGHT;
            _customTimer = new CustomTimer();

            CheckBoundary();
        }

        /*
        * Upward and Downward loop will turn the mesh of the Plane
        */
        private void Update()
        {
#if UNITY_EDITOR && TESTING
            // if (Input.GetKeyDown(KeyCode.Space))
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                DropBomb();
            // if (Input.GetKey(KeyCode.LeftControl))
            if (Keyboard.current.leftCtrlKey.isPressed)
                _shootStatus = 1;
            else
                _shootStatus = 0;
#endif
            if ((_shootStatus & (1 << 0)) != 0)
                ShootBullets();
            else _shootTime = 0f;

            Vector2 direction = joyStick.GetInputDirection().normalized;

            // Move the airplance based on joystick input
            if (direction.sqrMagnitude > 0.01f)
            {
                //calculate the angle in radians and convert to  degrees 
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // Apply rotation to the airplace to point in the direction of movement
                // transform.rotation = Quaternion.Euler(0, 0, angle - 180);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle - 180), _rotateSpeed * Time.deltaTime);
            }

            //Apply rotation to the child mesh body when direction is changed
            // Switch between 180 | 0 
            if (transform.right.x < -0.3f && (_planeMeshRotateMult == (int)PlaneRotateStatus.RIGHT
                    || _planeMeshRotateMult == (int)PlaneRotateStatus.IN_PROCESS_OF_TURNING))
            {
                _planeMeshRotateMult = (int)PlaneRotateStatus.IN_PROCESS_OF_TURNING;
                _planeMesh.localRotation = Quaternion.Lerp(_planeMesh.localRotation, Quaternion.Euler(180f, 0f, 0f), _rotateSpeed * Time.deltaTime);
                // Debug.Log($"Negative | transform right : {transform.right} | _planeMeshRotateMult : {_planeMeshRotateMult}"
                // + $" | Euler : {_planeMesh.localEulerAngles} | rotation : {_planeMesh.localRotation}");

                if (_planeMesh.localEulerAngles.y >= 179.99f && _planeMesh.localEulerAngles.x >= 358.0f)
                // if (_planeMesh.localRotation.x <= -0.9999f && _planeMesh.localRotation.w <= 0.09f)           //Gimbal Lock
                {
                    // Debug.Log($"Reached");
                    _planeMesh.localRotation = Quaternion.Euler(180f, 0f, 0f);
                    _planeMeshRotateMult = (int)PlaneRotateStatus.LEFT;
                }
            }
            else if (transform.right.x > 0.3f && (_planeMeshRotateMult == (int)PlaneRotateStatus.LEFT
                    || _planeMeshRotateMult == (int)PlaneRotateStatus.IN_PROCESS_OF_TURNING))
            {
                _planeMeshRotateMult = (int)PlaneRotateStatus.IN_PROCESS_OF_TURNING;
                _planeMesh.localRotation = Quaternion.Lerp(_planeMesh.localRotation, Quaternion.Euler(0, 0f, 0f), _rotateSpeed * Time.deltaTime);
                // Debug.Log($"Positive | transform right : {transform.right} | _planeMeshRotateMult : {_planeMeshRotateMult}"
                // + $" | Euler : {_planeMesh.localEulerAngles} | rotation : {_planeMesh.localRotation}");

                // if (_planeMesh.localRotation.x <= -0.009f && _planeMesh.localRotation.w >= 0.99f)        //Gimbal Lock
                if (_planeMesh.localEulerAngles.y <= 0.09f && _planeMesh.localEulerAngles.x >= 358.0f)
                {
                    // Debug.Log($"Reached");
                    _planeMesh.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    _planeMeshRotateMult = (int)PlaneRotateStatus.RIGHT;
                }
            }

            transform.Translate(Vector2.left * _speedMult * Time.deltaTime);

            _propeller.Rotate(new Vector3(10f, 0f, 0f) * _propellerSpeedMult * Time.deltaTime);
        }

        /*private void FixedUpdate()
        {
            Vector3 speedVec = transform.right * -1f * _speedMult;
            transform.position = Vector3.Lerp(transform.position, transform.position + speedVec, _positionLerpVal);

            // transform.position = Vector3.Lerp(transform.position, (transform.position + Vector3.left) * _speedMult, _positionLerpVal);
            // transform.Translate(Vector2.left * speed);
        }*/

        //TODO: Make a destruction logic/effect
        private void OnCollisionEnter(Collision other)
        {
            // Debug.Log("Hit");
            TakeDamage(-1000f);
            // gameObject.SetActive(false);
            // GameManager.Instance.OnPlayerAction?.Invoke(0f, (int)PlayerAction.PLAYER_DEAD);
        }

        private void DropBomb()
        {
            if (_bombStatus == (int)BombStatus.SHOT) return;
            _bombStatus = (int)BombStatus.SHOT;
            CoolDownBomb();
            // GameObject shotProjectile = Instantiate(_projectilePrefab, _bombPoint.position, transform.rotation);
            GameObject shotProjectile = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.BOMB].Get();
            shotProjectile.transform.position = _bombPoint.position;
            shotProjectile.transform.rotation = _bombPoint.rotation;

            bool leftAligned = false;
            if (_bombPoint.eulerAngles.y > 150)
                leftAligned = true;
            shotProjectile.GetComponent<ProjectileBase>().SetStats(Vector3.forward, leftAligned);

            shotProjectile.SetActive(true);
            // Debug.Log($"Shoot Clicked | transform.right : {transform.right} | Namer : {shotProjectile.name}");
        }

        private void ShootBullets()
        {
            if (_shootTime >= _shootInterval)
            {
                _shootTime = 0f;
                //Shoot Bullet
                _instancedBullet = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.PLAYER_BULLET].Get();
                _instancedBullet.transform.position = _shootPoint.position;
                _instancedBullet.transform.rotation = _shootPoint.rotation;
                _instancedBullet.SetActive(true);
                AudioManager.Instance.PlaySFXClip(AudioTypes.PLAYER_SHOOT, 0.5f);
            }
            _shootTime += Time.deltaTime;
        }

        private async void CoolDownBomb()
        {
            GameManager.Instance.OnPlayerAction?.Invoke(_bombCooldownTime, (int)PlayerAction.BOMB_DROP);
            await _customTimer.WaitForSeconds(_bombCooldownTime);
            if (_cts.IsCancellationRequested) return;
            _bombStatus = (int)BombStatus.AVAILABLE;
        }

        //TODO: Plane destruction effect
        public void TakeDamage(float damageTaken)
        {
            // Debug.Log($"Taking Damage : {damageTaken}");
            _health -= damageTaken;

            if (_health <= 0 || damageTaken < -999f)
            {
                // Debug.Log($"Player Dead : {GameManager.Instance.PlayerDead}");
                GameManager.Instance.PlayerDead = true;
                gameObject.SetActive(false);
                GameManager.Instance.OnPlayerAction?.Invoke(0f, (int)PlayerAction.PLAYER_DEAD);
                AudioManager.Instance.EngineAudioSource.gameObject.SetActive(false);
                AudioManager.Instance.PlaySFXClip(AudioTypes.BOMB_EXPLOSION, 1f);

                GameManager.Instance.OnProjectileHit?.Invoke(transform.position, PoolManager.PoolType.BOMB, BombStatus.HIT_STAT);
            }
            else
                GameManager.Instance.OnPlayerAction?.Invoke(_health / _ogHealth, (int)PlayerAction.PLAYER_HIT);
        }

        private async void CheckBoundary()
        {
            // bool outsideBoundary = false;
            int playerBoundaryStatus = (int)PlayerAction.INSIDE_BOUNDARY;
            while (true)
            {
                if (playerBoundaryStatus == (int)PlayerAction.INSIDE_BOUNDARY
                     && (transform.position.x < -700f || transform.position.x > 350f))
                {
                    // outsideBoundary = true;
                    playerBoundaryStatus = (int)PlayerAction.OUTSIDE_BOUNDARY;
                    GameManager.Instance.OnBoundariesEntered?.Invoke(playerBoundaryStatus);
                }
                else if (playerBoundaryStatus == (int)PlayerAction.OUTSIDE_BOUNDARY &&
                    transform.position.x > -700f && transform.position.x < 350f)
                {
                    // outsideBoundary = false;
                    playerBoundaryStatus = (int)PlayerAction.INSIDE_BOUNDARY;
                    GameManager.Instance.OnBoundariesEntered?.Invoke(playerBoundaryStatus);
                }
                await _customTimer.WaitForSeconds(_checkBoundaryWaitTime);

                if (_cts.IsCancellationRequested) return;
            }
        }
    }
}