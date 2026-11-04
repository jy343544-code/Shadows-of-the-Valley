using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickArea;
    [SerializeField] private RectTransform handle;
    [SerializeField] private PlayerController player;
    [SerializeField] private float handleRange = 60f;

    private void Awake()
    {
        if (joystickArea == null) joystickArea = transform as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickArea == null || handle == null || player == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea, eventData.position, eventData.pressEventCamera, out Vector2 point);

        Vector2 half = joystickArea.rect.size * 0.5f;
        Vector2 input = new Vector2(
            half.x > 0 ? point.x / half.x : 0f,
            half.y > 0 ? point.y / half.y : 0f);

        input = Vector2.ClampMagnitude(input, 1f);
        handle.anchoredPosition = input * handleRange;
        player.SetMoveInput(input);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handle != null) handle.anchoredPosition = Vector2.zero;
        if (player != null) player.SetMoveInput(Vector2.zero);
    }
}
