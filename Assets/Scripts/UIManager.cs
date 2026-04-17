using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] float videoDelay = 0.3f;
    [SerializeField] VideoPlayer startCutsceneVideo;
    [SerializeField] GameObject cutSceneUI;
    [SerializeField] Image cutsceneFinalImage;

    void Start()
    {
        StartCoroutine(CutsceneFinish());
    }
    private IEnumerator CutsceneFinish()
    {
        yield return new WaitForSeconds(videoDelay);
        startCutsceneVideo.gameObject.SetActive(false);
        cutsceneFinalImage.gameObject.SetActive(true);
    }

    public void CloseTheLetterUI()
    {
        cutSceneUI.SetActive(false);
    }
    
}
