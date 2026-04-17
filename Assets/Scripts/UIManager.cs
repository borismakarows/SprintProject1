using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] float videoDelay = 0.3f;
    [Header("Components")]
    [SerializeField] VideoPlayer IntroCutsceneVideo;
    [SerializeField] GameObject cutSceneUI;
    [SerializeField] Image cutsceneFinalImage;
    bool isPlayingCutscene;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] PlayerMovement2D playerRef;


    void Start()
    {
        if (playerRef == null) {Debug.Log("Fuck, no player ref."); return;}
        StartCoroutine(PlayCutscene());
        DeactivatePauseMenu();
        playerRef.SwitchToUIInput();
    }
    private IEnumerator PlayCutscene()
    {
        playerRef.SwitchToUIInput();
        isPlayingCutscene = true;
        yield return new WaitForSeconds(videoDelay);
        IntroCutsceneVideo.gameObject.SetActive(false);
        cutsceneFinalImage.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void CloseTheLetterUI()
    {
        if (playerRef == null) {Debug.Log("No Player ref"); return;}
        playerRef.SwitchToGameInput();
        cutSceneUI.SetActive(false);
        isPlayingCutscene = false;
    }

    public void TogglePause()
    {
        if (playerRef != null)
        {
            if (pauseMenu.activeSelf)
            {
                DeactivatePauseMenu();
                if (!isPlayingCutscene) {playerRef.SwitchToGameInput();}
            }
            else
            {
                ActivatePauseMenu();
                playerRef.SwitchToUIInput();
            }          
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        DeactivatePauseMenu();
        if (!isPlayingCutscene) {playerRef.SwitchToGameInput();}
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ActivatePauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        if (IntroCutsceneVideo.isActiveAndEnabled) {IntroCutsceneVideo.Pause();}
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        if (IntroCutsceneVideo.isActiveAndEnabled) {IntroCutsceneVideo.Play();}
    }
    
}
