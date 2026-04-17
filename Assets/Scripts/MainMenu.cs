using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
    }

    public void ChangeVolume()
    {
        float volumeValue = volumeSlider.value;
        AudioListener.volume = volumeValue;
        PlayerPrefs.SetFloat("musicVolume", volumeValue);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit triggered!");
        Application.Quit();
    }


}
