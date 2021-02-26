
using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(RectTransform))]
public class ButtonCollider : MonoBehaviour
{
    private BoxCollider boxCollider;
    private RectTransform rectTransform;
  

    private void OnEnable()
    {
        ValidateCollider();
    }

    private void OnValidate()
    {
        ValidateCollider();
    }

    private void ValidateCollider()
    {
        rectTransform = GetComponent<RectTransform>();

        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        boxCollider.size = rectTransform.sizeDelta;
        Vector3 size = boxCollider.size;

        size.z = 10.0f;
        boxCollider.size = size;

    }
}