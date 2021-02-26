using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour, IAnimation
{

    public Vector3 maxScale = Vector3.one * 1.25f;
    public Vector3 minScale = Vector3.one * 0.25f;
    public float speed = 2.5f;

    public bool grow = false; 

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale; 
        
        if(grow)
        {
            scale += maxScale.normalized* speed * Time.deltaTime; 

            if(scale.sqrMagnitude >= maxScale.sqrMagnitude)
            {
                grow = false; 
            }
        }
        else
        {
            scale -= maxScale.normalized * speed * Time.deltaTime;

            if (scale.sqrMagnitude <= minScale.sqrMagnitude)
            {
                grow = true;
            }
        }
        transform.localScale = scale; 

    }

}
