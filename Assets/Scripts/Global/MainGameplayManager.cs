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

        // Start is called before the first frame update
        void Start()
        {
            _skyboxScrollSpeed = 0.95f;
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
            _cameraManager = new CameraManager(ref _cameraTransform, ref _playerTransform, ref _cameraFollowSpeedMult);
        }

        // Update is called once per frame
        void Update()
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
