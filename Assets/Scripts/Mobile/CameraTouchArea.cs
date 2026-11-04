using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchArea : MonoBehaviour, IDragHandler
{
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private float dragScale = 1f;

    public void OnDrag(PointerEventData eventData)
    {
        if (targetCamera != null)
            targetCamera.AddLookInput(eventData.delta * dragScale);
    }
}
