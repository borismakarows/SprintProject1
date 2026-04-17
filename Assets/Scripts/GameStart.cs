using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;

    [SerializeField] private Room level_index;
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
                PlayerMovement2D playerMovement = player.GetComponent<PlayerMovement2D>();
                if (level != null)
                {
                    level.SetActive(true);
                    playerMovement.currentEra = Era.Child;
                    playerMovement.currentRoom = level_index;
                    housePlayer = collider.transform;
                    collider.gameObject.transform.position = LevelSpawn.transform.position;
                    isGamePlayed = true;
                }   
            }
        }
        
    }
    public void gameWon()
    {
        isGameFinished = true;
        player.transform.position = housePlayer.transform.position;
        //UI_Reward.SetActive(true);
    }
}
