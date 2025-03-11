using UnityEngine;

using FrontLineDefense.Player;
using System.Threading.Tasks;

namespace FrontLineDefense.Test
{
    public class PerformanceCheck : MonoBehaviour
    {
        [SerializeField] private JoyStickController _joystick;
        [SerializeField] private Transform _bodyToControl;
        private const float _rotateSpeed = 2.0f;

        private void Start()
        {
            // CheckSin();
        }

        private async void CheckSin()
        {
            System.Text.StringBuilder _debugStringBuilder = new System.Text.StringBuilder();
            float num = 0f;
            while (true)
            {
                _debugStringBuilder.Append(Mathf.Sin(num * Mathf.Deg2Rad));
                _debugStringBuilder.Append(',');
                if (num > 180f) break;
                num += 1f;
                await Task.Yield();
            }

            Debug.Log($"{_debugStringBuilder}");
        }

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
