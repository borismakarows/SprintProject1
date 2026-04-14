using UnityEngine;
using UnityEngine.UIElements;

public class MusicBoxController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform effectStartPos;
    [SerializeField] BoxCollider2D musicEffectArea;
    [SerializeField] AudioSource musicSource;
    [SerializeField] CircleCollider2D PickupCol;


    void Awake()
    {
        musicEffectArea = GetComponentInChildren<BoxCollider2D>();
        PickupCol = GetComponentInChildren<CircleCollider2D>();
        musicSource = GetComponent<AudioSource>();
    }



}
