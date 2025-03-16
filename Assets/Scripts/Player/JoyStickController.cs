using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  
/// </summary>

namespace BombDrop.Player
{
    public class JoyStickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {

        private RectTransform joystickBackground;
        private RectTransform joystickHandle;

        private Vector2 inputDirection;

        private void Start()
        {
            joystickBackground = transform.GetChild(0).GetComponent<RectTransform>();
            joystickHandle = transform.GetChild(1).GetComponent<RectTransform>();
        }


        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position);
            position = Vector2.ClampMagnitude(position, joystickBackground.sizeDelta.x / 2);
            inputDirection = new Vector2(position.x / (joystickBackground.sizeDelta.x / 2), position.y / (joystickBackground.sizeDelta.y / 2));
            joystickHandle.anchoredPosition = position;


        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputDirection = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;

        }

        public Vector2 GetInputDirection()
        {
            return inputDirection;
        }
    }
}