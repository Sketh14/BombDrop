using UnityEngine;

using BombDrop.Global;
using System.Threading;

namespace BombDrop.Gameplay
{
    public class MainGameplayManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        #region CameraControls
        [Header("Camera Controls")]
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraFollowSpeedMult;
        #endregion CameraControls

        [SerializeField] private Material _skyBoxMat;
        private float _skyboxScrollSpeed;
        private CameraManager _cameraManager;
        private ExplosionEffectManager _explosionEffectManager;

        private CancellationTokenSource _cts;
        // private GameObject _explosionEffect;

        // [SerializeField] private Vector3[] _playerShootDownPoints;

        private void OnDisable()
        {
            GameManager.Instance.OnProjectileHit -= MakeExplosionAt;
            GameManager.Instance.OnBoundariesEntered -= ShootDownPlayer;
            _cts.Cancel();
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnProjectileHit += MakeExplosionAt;
            GameManager.Instance.OnBoundariesEntered += ShootDownPlayer;

            _cts = new CancellationTokenSource();
            _skyboxScrollSpeed = 0.95f;
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
            _cameraManager = new CameraManager(ref _cameraTransform, ref _playerTransform, ref _cameraFollowSpeedMult);
            _explosionEffectManager = new ExplosionEffectManager();
        }

        private async void MakeExplosionAt(Vector3 explosionPosition, PoolManager.PoolType explosionPoolType, BombStatus bombStatus)
        {
            // Debug.Log($"Make Explosion Called | explosionPoolType : {explosionPoolType}");
            float explosionIntensity = 10f;
            int prefabIndex = 1;
            switch (explosionPoolType)
            {
                case PoolManager.PoolType.BOMB:
                    explosionIntensity = 15f;
                    prefabIndex = (int)PoolManager.PoolType.EXPLOSION_BIG;
                    break;

                case PoolManager.PoolType.FOLLOWING_MISSILES:
                case PoolManager.PoolType.STRAIGHT_RANGED_MISSILES:
                    explosionIntensity = 10f;
                    prefabIndex = (int)PoolManager.PoolType.EXPLOSION_SMALL;
                    break;
            }

            GameObject explosionEffect = PoolManager.Instance.ObjectPool[prefabIndex].Get();
            explosionEffect.transform.position = explosionPosition;
            explosionEffect.SetActive(true);

            await _explosionEffectManager.FlashLights(explosionEffect.transform, explosionIntensity);
            if (_cts.IsCancellationRequested) return;
            PoolManager.Instance.ObjectPool[prefabIndex].Release(explosionEffect);
            // _explosionEffect.SetActive(false);
        }

        private void ShootDownPlayer(int playerStatus)
        {
            if (playerStatus != (int)PlayerAction.OUTSIDE_BOUNDARY_SAFEZONE) return;

            GameObject followingMissileForPlayer = PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.FOLLOWING_MISSILES].Get();

            if (_playerTransform.position.x > 0)
            {
                // followingMissileForPlayer.transform.position = _playerShootDownPoints[0];
                followingMissileForPlayer.transform.position = new Vector3(600f, _playerTransform.position.y, 0f);

                //Calculate Player Angle and set
                // Vector3 playerDir = (_playerTransform.position - transform.position).normalized;
                // float rotateAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                // followingMissileForPlayer.transform.rotation = Quaternion.Euler(new Vector3(rotateAngle * -1f, -90f, 0f));
                followingMissileForPlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
            }
            else
            {
                followingMissileForPlayer.transform.position = new Vector3(-1010f, _playerTransform.position.y, 0f);

                //Calculate Player Angle and set
                // Vector3 playerDir = (_playerTransform.position - transform.position).normalized;
                // float rotateAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                // followingMissileForPlayer.transform.rotation = Quaternion.Euler(new Vector3(rotateAngle * -1f, -90f, 0f));
                followingMissileForPlayer.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            }

            followingMissileForPlayer.transform.localScale = new Vector3(2f, 2f, 2f);
            followingMissileForPlayer.GetComponent<Projectiles.ProjectileBase>().SetStats(Vector3.right * -1.0f, true, -1000f, 80f);
            followingMissileForPlayer.SetActive(true);
        }

        private void Update()
        {
            // _skyBoxMat.SetFloat("_Rotation", Mathf.Sin(_skyboxScrollSpeed) * 360f);
            _skyBoxMat.SetFloat("_Rotation", _skyboxScrollSpeed * 360f);
            _skyboxScrollSpeed += (Time.deltaTime * 0.001f);

            if (_skyboxScrollSpeed >= 1f)
                _skyboxScrollSpeed = 0f;
        }

        private void LateUpdate()
        {
            _cameraManager.LateUpdate();
        }
    }
}
