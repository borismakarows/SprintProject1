using UnityEngine;

public class Reward : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;

    [SerializeField] private bool bunny  = false;

    private GameStart gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (level != null)
        {
            gameManager = level.GetComponent<GameStart>();
        }
    }

    void Update()
    {
        if (bunny)
        {
            //TODO
            //while carrots remain -> deactivate
        }
    }

    ~Reward()
    {
        Debug.Log("Reward Destroyed");
        gameManager.gameWon();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player TRIGGERED");
        if (other.tag == "Player")
        {
            gameManager.gameWon();
        }
    }
}
