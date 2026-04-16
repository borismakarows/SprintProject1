using UnityEngine;

public class GameStart : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject LevelSpawn;
    [SerializeField] private GameObject reward;

    private bool isGamePlayed = false;
    private Transform housePlayer;
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        housePlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isGamePlayed && reward != null && reward.active == false)
        {
            player.transform.position = housePlayer.transform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name + "is now in room");
        if (collider.tag == "Player")
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
