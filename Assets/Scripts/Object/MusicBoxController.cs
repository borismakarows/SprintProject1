using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicBoxController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] AudioSource musicBoxSource;
    [SerializeField] AudioClip musicClip;
    [SerializeField] Transform startPos;
    PlayerMovement2D playerRef;
   
    
    [Header("Parameters")]
    [SerializeField] float baseMusicWindowTimer = 0.3f;
    [SerializeField] float musicTimerRandomizer = 0.1f;
    [SerializeField] float baseMoveWindowTimer = 0.5f;
    [SerializeField] float moveTimerRandomizer = 0.1f;
    [SerializeField] float musicStartWarningDuration = 1.0f;
    [SerializeField] float musicStopWarningDuration = 0.25f;

    bool isPaused;


    void Awake() => musicBoxSource = GetComponent<AudioSource>();

    void OnValidate() => musicBoxSource = GetComponent<AudioSource>();
  

    void Start()
    {
        DebuggingComponents();
        PauseMusic();
    }

    void Update()
    {
        if (playerRef == null) return;
        CheckMovementDuringMusic();
    }

     private void DebuggingComponents()
    {
        if (musicClip == null) {Debug.Log("Music Clip is null");}
        if (startPos == null) {Debug.Log("Assign start position");}
    }

    
    public void ResetMiniGame()
    {
        if (playerRef == null) return;
        StopAllCoroutines();
        PauseMusic();
        playerRef.transform.position = startPos.position;
    }

    private void CheckMovementDuringMusic()
    {
        if (!isPaused && playerRef.isAnyInputFired)
        {
            Debug.Log("Pressed Buttons, FAILED!");
            ResetMiniGame();
        }
    }

    private void UnpauseMusic()
    {
        isPaused = false;
        musicBoxSource.UnPause();
    }

    private void PauseMusic()
    {
        isPaused = true;
        musicBoxSource.Pause();
    }

    public void StartMusicBoxGame()
    {
        StartCoroutine(StartMusicWindow());
    }


    private IEnumerator StartMusicWindow()
    {
        MusicStartIndicator();
        yield return new WaitForSeconds(musicStartWarningDuration);
        StartCoroutine(MusicWindow());
    }


    private IEnumerator MusicWindow()
    {
        UnpauseMusic();
        float musicWindow = baseMusicWindowTimer - Random.Range(-musicTimerRandomizer, musicTimerRandomizer);
        yield return new WaitForSeconds(musicWindow);   
        StartCoroutine(StartMoveWindow());
    }    

    private IEnumerator StartMoveWindow()
    {
        MusicStopIndicator();
        yield return new WaitForSeconds(musicStopWarningDuration);
        StartCoroutine(MoveWindow());
    }

    private IEnumerator MoveWindow()
    {
        PauseMusic();
        float moveWindow = baseMoveWindowTimer - Random.Range(-moveTimerRandomizer, moveTimerRandomizer);
        yield return new WaitForSeconds(moveWindow);
        StartCoroutine(StartMusicWindow());
    }

    private void MusicStartIndicator()
    {
        Debug.Log("Do not Move or You are dead");
    }

    private void MusicStopIndicator()
    {
        Debug.Log("Music is about to stop");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRef = collision.GetComponent<PlayerMovement2D>();
            StartMusicBoxGame(); 
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision: " + collision.gameObject.name);
    }

}
