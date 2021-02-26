using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{

    [HideInInspector]
    private static BackgroundSound instance = null;

    public static BackgroundSound getInstance()
    {

        if (instance == null)
        {
            Debug.Log("TetrisSettings.getInstance() called before instance was initialized!");
            instance = new BackgroundSound();
            instance.Awake();
        }

        return instance;
    }

    void Awake()
    {
        BackgroundSound.instance = this;

        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
