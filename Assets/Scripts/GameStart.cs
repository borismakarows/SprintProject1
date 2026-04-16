using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject LevelSpawn;
    [SerializeField] private GameObject reward;

    private bool isGamePlayed = false;
    private bool isGameFinished = false;
    private Transform housePlayer;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        housePlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isGamePlayed && !isGameFinished && reward == null)
        {
            player.transform.position = housePlayer.transform.position;
            isGameFinished = true;
            
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name + "is now in room");
        if (collider.tag == "Player")
        {
            if (!isGameFinished)
            {
                Debug.Log("yes its player");
                player = collider.gameObject;
                if (level != null)
                {
                    housePlayer = collider.transform;
                    collider.gameObject.transform.position = LevelSpawn.transform.position;
                    isGamePlayed = true;
                }   
            }
        }
        
    }
}
