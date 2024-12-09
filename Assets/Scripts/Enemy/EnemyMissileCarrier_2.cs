// #define TARGETING_TEST

using UnityEngine;

using FrontLineDefense.Global;
using FrontLineDefense.Projectiles;

namespace FrontLineDefense.Enemy
{
    public class EnemyMissileCarrier_2 : EnemyVehicleController_Base
    {
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private PoolManager.PoolType _missilePool;
        [SerializeField] private Transform[] _shootPoints;
        private byte _currentShootPointIndex;
        private const float _alignThreshold = 2f;
        // private const float _searchWaitTime = 2f;

        protected override void OnDisable()
        {
            base.OnDisable();
            _currentShootPointIndex = 0;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            //Fill the shoot points with missiles

        }

        public float yRotateAngle;         //Debugging
        // public Vector3 playerDirection;        //Debugging
        protected override void TargetPlayer()
        {
            // if (_ShotProjectileStatus != (byte)ShootStatus.AVAILABLE_TO_SHOOT
            //     && _ShotProjectileStatus != (byte)ShootStatus.RECHARGING) return;
#if !TARGETING_TEST
            // if (_ShotProjectileStatus != (byte)ShootStatus.SEARCHING_PLAYER) return;
            if ((_ShotProjectileStatus & (1 << (int)ShootStatus.SEARCHING_PLAYER)) == 0) return;
#endif

            Vector3 playerDirection = GameManager.Instance.PlayerTransform.position - _Turret.position;
            // _partToRotate.rotation = Quaternion.LookRotation(_playerDirection);

            //calculate the angle in radians and convert to  degrees
            // float 
            yRotateAngle = (Mathf.Atan2(playerDirection.z, playerDirection.x) * Mathf.Rad2Deg) - 180f;
            // _Turret.localRotation = Quaternion.Euler(0f, yRotateAngle, -30f);
            _Turret.localRotation = Quaternion.Lerp(_Turret.localRotation, Quaternion.Euler(0f, yRotateAngle, -30f), _rotateSpeed * Time.fixedDeltaTime);
            // Debug.Log($"Turret Euler : {_Turret.eulerAngles} | yRotateAngle : {yRotateAngle} | Diff : {_Turret.eulerAngles.y - yRotateAngle}");

            if ((_ShotProjectileStatus & (1 << (int)ShootStatus.FOUND_PLAYER)) == 0
                && ((yRotateAngle * -1f) - _Turret.eulerAngles.y) <= _alignThreshold)
            // && Vector3.Angle(_Turret.right * -1f, playerDirection) <= _alignThreshold)
            {
                // Debug.Log($"Turret Aligned : {_ShotProjectileStatus}");
                _ShotProjectileStatus |= 1 << (int)ShootStatus.FOUND_PLAYER;
            }
        }

        protected override async void Shoot()
        {
            GameObject shotProjectile = PoolManager.Instance.ObjectPool[(int)_missilePool].Get();
            shotProjectile.transform.position = _ShootPoint.position;

            // Debug.Log($"Shoot Point | Local Euler Angle : {_ShootPoint.localEulerAngles} | Euler Angle : {_ShootPoint.eulerAngles}");
            if (_ShootPoint.eulerAngles.y > 150)
                shotProjectile.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -150f));
            else
                shotProjectile.transform.rotation = _ShootPoint.rotation;

            shotProjectile.GetComponent<ProjectileBase>().SetStats(_ShootPoint.right * -1.0f);
            shotProjectile.SetActive(true);

            _ShotProjectileStatus = (byte)ShootStatus.RECHARGING;
            await _CtTimer.WaitForSeconds(_ShootCooldown);
            _ShotProjectileStatus |= 1 << (int)ShootStatus.SEARCHING_PLAYER;
            await _CtTimer.WaitForSeconds(_SearchWaitTime);
            _ShotProjectileStatus |= 1 << (int)ShootStatus.AVAILABLE_TO_SHOOT;
        }
    }
}