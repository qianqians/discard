using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DropObject : MonoBehaviour
{
    public GameObject draggedObject;
    protected bool isDragging = false;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (isDragging && draggedObject != null)
        {
            draggedObject.transform.position = GetWorldPosition(eventData.position);
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            Destroy(draggedObject);
            draggedObject = null;
        }
        isDragging = false;
    }

    protected Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        return Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
    }
}