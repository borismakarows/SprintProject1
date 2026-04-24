using UnityEngine;

public class Reward : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;

    [SerializeField] private bool bunny  = false;

    private GameStart gameManager;
    
    void Start()
    {
        if (level != null)
        {
            gameManager = level.GetComponent<GameStart>();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            gameManager.GameWon();
        }
    }
}
