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

        private CameraManager _cameraManager;

        // Start is called before the first frame update
        void Start()
        {
            _cameraManager = new CameraManager(ref _cameraTransform, ref _playerTransform, ref _cameraFollowSpeedMult);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            _cameraManager.LateUpdate();
        }
    }
}
