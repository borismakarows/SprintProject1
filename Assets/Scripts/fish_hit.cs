using UnityEngine;

public class fish_hit : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] public GameObject spawn;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.tag == "Player" && spawn != null)
        {
            Debug.Log("Hut Player");
            collision.gameObject.transform.position = spawn.transform.position;
        }
    }
}
