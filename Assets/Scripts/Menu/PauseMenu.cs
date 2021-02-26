using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public VRUIInput vRUIInput; 


    // Update is called once per frame
    void Update()
    {
        if(TetrisSettings.getInstance().userInput.isPause() || Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Escape key has been pressed...");
            if (GameIsPaused)
            {
                if(TetrisSettings.getInstance().inputMode == TetrisSettings.InputMode.Mouse)
                    Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        vRUIInput.enabled = false;

        Debug.Log("Resuming the game...");

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause ()
    {
        vRUIInput.enabled = true; 
        pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu ()
    {
        Debug.Log("Loading the Menu");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting the Game...");
        Application.Quit();
    }

}
