using UnityEngine;

using FrontLineDefense.Player;

namespace FrontLineDefense.Test
{
    public class PerformanceCheck : MonoBehaviour
    {
        [SerializeField] private JoyStickController _joystick;
        [SerializeField] private Transform _bodyToControl;
        private const float _rotateSpeed = 2.0f;

        // Update is called once per frame
        void Update()
        {
            Vector2 joyStickDirection = _joystick.GetInputDirection().normalized;
            // if (joyStickDirection.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(joyStickDirection.y, joyStickDirection.x) * Mathf.Rad2Deg;
                _bodyToControl.rotation = Quaternion.Lerp(_bodyToControl.rotation, Quaternion.Euler(0, 0, angle - 180), _rotateSpeed * Time.deltaTime);
            }
        }
    }
}
