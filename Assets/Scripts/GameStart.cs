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
    private Vector3 housePlayer;
    [SerializeField] private GameObject player;
	private PlayerMovement2D playerMovement;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (!isGamePlayed)
            {
                player = collision.gameObject;
                playerMovement = player.GetComponent<PlayerMovement2D>();
                if (level != null)
                {
                    
                    level.SetActive(true);
                    playerMovement.SwitchEra(Era.Child);
                    playerMovement.currentRoom = level_index;
                    housePlayer = collision.gameObject.transform.position;
                    collision.gameObject.transform.position = LevelSpawn.transform.position;
                    isGamePlayed = true;
                }   
            }
        }
    }

    
    public void GameWon()
    {
		player.transform.position = housePlayer;
        playerMovement.currentRoom = Room.TeenHouse;
        playerMovement.FreezeMovement();
        playerMovement.SwitchEra(Era.Teen);
        reward_UI.SetActive(true);
    }
}
