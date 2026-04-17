using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;

    [SerializeField] private Room level_index;
    [SerializeField] private GameObject LevelSpawn;
    [SerializeField] private GameObject reward;
	[SerializeField] private GameObject reward_UI;

    private bool isGamePlayed = false;
    private bool isGameFinished = false;
    private Vector3 housePlayer;
    private GameObject player;
	private PlayerMovement2D playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        housePlayer = GameObject.FindGameObjectWithTag("Player").transform.position;
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
                playerMovement = player.GetComponent<PlayerMovement2D>();
                if (level != null)
                {
                    level.SetActive(true);
                    playerMovement.currentEra = Era.Child;
                    playerMovement.currentRoom = level_index;
                    housePlayer = collider.gameObject.transform.position;
                    collider.gameObject.transform.position = LevelSpawn.transform.position;
                    isGamePlayed = true;
                }   
            }
        }
        
    }
    public void gameWon()
    {
		Debug.Log(housePlayer);
        isGameFinished = true;
        player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = housePlayer;
        playerMovement.currentEra = Era.Teen;
        playerMovement.currentRoom = 0;
        reward_UI.SetActive(true);
    }
}
