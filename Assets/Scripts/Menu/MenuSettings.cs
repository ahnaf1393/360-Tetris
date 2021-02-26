using UnityEngine.EventSystems;
using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    public AudioSource buttonAudio;

    public EventSystem eventSystem;

    public GameObject sceneSpaceButton;
    public GameObject sceneRockButton;
    public GameObject musicDefaultButton;
    public GameObject musicRockButton;

    public int sceneChoice = 0;

    [HideInInspector]
    private static MenuSettings instance = null;


    public static MenuSettings getInstance()
    {

        if (instance == null)
        {
            Debug.Log("TetrisSettings.getInstance() called before instance was initialized!");
            instance = new MenuSettings();
            instance.Awake();
        }

        return instance;
    }

    void Awake()
    {
        MenuSettings.instance = this;
    }
}
