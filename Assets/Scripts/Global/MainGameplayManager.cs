using UnityEngine;

namespace FrontLineDefense.Global
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
        // private GameObject _explosionEffect;

        private void OnDisable()
        {
            GameManager.Instance.OnProjectileHit -= MakeExplosionAt;
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnProjectileHit += MakeExplosionAt;

            _skyboxScrollSpeed = 0.95f;
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
            _cameraManager = new CameraManager(ref _cameraTransform, ref _playerTransform, ref _cameraFollowSpeedMult);
            _explosionEffectManager = new ExplosionEffectManager();
        }

        private async void MakeExplosionAt(Vector3 explosionPosition, PoolManager.PoolType explosionPoolType)
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
            PoolManager.Instance.ObjectPool[prefabIndex].Release(explosionEffect);
            // _explosionEffect.SetActive(false);
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
