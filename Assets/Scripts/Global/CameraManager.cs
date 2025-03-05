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
            // targetPos.y = _followTransform.position.y + _yOffset;

            //Look Towards Player
            //Should be able to multiply the rotation with a matrix to offset the rotation somehow
            // _cameraTransform.LookAt(_followTransform.position + (_followTransform.right * -2));
            // Quaternion currRotation = _cameraTransform.rotation;
            // currRotation.x += 0.15f;
            // currRotation.z = currRotation.y = 0f;
            // currRotation.y = -0.127f; currRotation.z = 0.0271f;      //Does not works as intended
            // currRotation.w -= (currRotation.w < 0) ? 0.1f : -0.1f;       //Do not want to modify w
            // Quaternion currRotation = Quaternion.LookRotation(_followTransform.position - _cameraTransform.position) * Quaternion.Euler(24f, -15f, 0f);
            // Quaternion currRotation = Quaternion.LookRotation(_followTransform.position - _cameraTransform.position);

            //https://discussions.unity.com/t/lookat-to-only-rotate-on-y-axis-how/10895/3
            Vector3 lookDirection = _followTransform.position - _cameraTransform.position;
            //To Fix y-rotation | As the values of Y/Z-axis will  only change, then the camera will tilt up and down only, 
            //so fixing the x-axis will point the vector in a slightly forward direction, with Y/Z axis free to move
            lookDirection.x = -10f;                 //1[Vec] = 1.5..[Euler]
            lookDirection.y -= 10f;                  //To get offset in up/down direction
            Quaternion currRotation = Quaternion.LookRotation(lookDirection);
            _cameraTransform.rotation = currRotation;

            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPos, _followSpeedMult * Time.deltaTime);
        }
    }
}
