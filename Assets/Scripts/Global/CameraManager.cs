using UnityEngine;

namespace FrontLineDefense.Global
{
    public class CameraManager
    {
        private Transform _followTransform, _cameraTransform;
        private float _followSpeedMult;
        private float _yOffset;

        public CameraManager(ref Transform cameraTransform, ref Transform followTransform, ref float followSpeedMult)
        {
            _cameraTransform = cameraTransform;
            _followTransform = followTransform;
            _followSpeedMult = followSpeedMult;
            _yOffset = cameraTransform.position.y - followTransform.position.y;
        }

        ~CameraManager() { }

        public void LateUpdate()
        {
            Vector3 targetPos = _cameraTransform.position;
            targetPos.x = _followTransform.position.x;
            targetPos.y = _followTransform.position.y + _yOffset;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPos, _followSpeedMult * Time.deltaTime);
        }
    }
}
