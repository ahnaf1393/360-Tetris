using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private MenuSettings settings = null;

    // Start is called before the first frame update
    void Start()
    {
        settings = MenuSettings.getInstance();
        settings.sceneRockButton.GetComponent<Button>().interactable = true;
        settings.sceneSpaceButton.GetComponent<Button>().interactable = false;
        settings.musicRockButton.GetComponent<Button>().interactable = true;
        settings.musicDefaultButton.GetComponent<Button>().interactable = false;
    }

    public void SetScene(int index)
    {
        settings.sceneChoice = index;
    }

    public void Play()
    {
        MoveAudio();
        
        Destroy(GameObject.Find("BackgroundAudio"));
        if(settings.sceneChoice == 0)
            SceneManager.LoadScene("SpaceScene");
        else
            SceneManager.LoadScene("RockScene");
    }

    public void Options()
    {
        MoveAudio();
        if(settings.sceneRockButton.GetComponent<Button>().interactable)
            settings.eventSystem.SetSelectedGameObject(GameObject.Find("Scene Rock Button"));
        else
            settings.eventSystem.SetSelectedGameObject(GameObject.Find("Scene Space Button"));

    }

    public void Quit()
    {
        MoveAudio();
        Application.Quit();
    }

    public void FourWall()
    {
        MoveAudio();
        Destroy(GameObject.Find("BackgroundAudio"));
        SceneManager.LoadScene("SpaceScene");
    }

    public void OneWall()
    {
        MoveAudio();
    }

    public void Back()
    {
        MoveAudio();
        settings.eventSystem.SetSelectedGameObject(GameObject.Find("Options Button"));
    }

    public void SceneRockButton()
    {
        settings.sceneRockButton.GetComponent<Button>().interactable = false;
        settings.sceneSpaceButton.GetComponent<Button>().interactable = true;
        settings.eventSystem.SetSelectedGameObject(GameObject.Find("Scene Space Button"));
        SetScene(1);
    }

    public void SceneSpaceButton()
    {
        settings.sceneRockButton.GetComponent<Button>().interactable = true;
        settings.sceneSpaceButton.GetComponent<Button>().interactable = false;
        settings.eventSystem.SetSelectedGameObject(GameObject.Find("Scene Rock Button"));
        SetScene(0);
    }

    public void MusicRockButton()
    {
        settings.musicRockButton.GetComponent<Button>().interactable = false;
        settings.musicDefaultButton.GetComponent<Button>().interactable = true;
        settings.eventSystem.SetSelectedGameObject(GameObject.Find("Music Default Button"));
    }

    public void MusicDefaulteButton()
    {
        settings.musicRockButton.GetComponent<Button>().interactable = true;
        settings.musicDefaultButton.GetComponent<Button>().interactable = false;
        settings.eventSystem.SetSelectedGameObject(GameObject.Find("Music Rock Button"));
    }

    void MoveAudio()
    {
        settings.buttonAudio.mute = false;
        settings.buttonAudio.Play();
    }
    
}
