using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDebug : MonoBehaviour
{
    Vector3 prev = Vector3.zero; 
    // Start is called before the first frame update
    void Start()
    {
        prev = transform.rotation.eulerAngles; 
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.rotation.eulerAngles - prev).sqrMagnitude>.1f)
        {
            prev = transform.rotation.eulerAngles;
            Debug.Log(gameObject.name + ": move: " + transform.rotation); 
        }
    }
}
