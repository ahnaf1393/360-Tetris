using UnityEngine;

public class OffscreenArrowIndicator : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        transform.up = target.position - transform.position;
        
        Vector3 screenPoint = cam.WorldToViewportPoint(target.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            transform.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
