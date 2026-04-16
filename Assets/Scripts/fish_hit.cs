using UnityEngine;

public class fish_hit : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] public GameObject spawn;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && spawn != null)
        {
            collision.gameObject.transform.position = spawn.transform.position;
        }
    }
}
