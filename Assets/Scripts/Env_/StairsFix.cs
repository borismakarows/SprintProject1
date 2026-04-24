using UnityEngine;

public class StairsFix : MonoBehaviour
{
    [Header("Stairs")] 
    [SerializeField] private GameObject stairs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && stairs != null)
        {
            stairs.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && stairs != null)
        {
            stairs.SetActive(true);
        }
    }
}
