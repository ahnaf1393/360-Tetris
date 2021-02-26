using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelection : MonoBehaviour
{
    public static int musicIndex;

    [HideInInspector]
    private static MusicSelection instance = null;

    public static MusicSelection getInstance()
    {
        return instance;
    }

    public int GetIndex()
    {
        return musicIndex;
    }

    void Awake()
    {
        MusicSelection.instance = this;
    }

    public void SetMusic(int index)
    {
        musicIndex = index;
    }

    public int GetMusic()
    {
        return musicIndex;
    }
}