using System.Collections;
using System.Collections.Generic;
using UnityEngine;




class ActivateAnimation : MonoBehaviour, IAnimation
{

    public GameObject[] animationObjects; 


    void Start()
    {
        OnDisable(); 
    }

    void OnDisable()
    {
        foreach (var o in animationObjects)
        {
            o.SetActive(false);
        }
    }

    void OnEnable()
    {
        foreach (var o in animationObjects)
        {
            o.SetActive(true);
        }
    }



}

