using UnityEngine;

public class PointCounter : MonoBehaviour
{
    [Header("BunnyReward")] 
    [SerializeField] private GameObject bunny;
    private int collectedCarrotNum;

    void Awake() => collectedCarrotNum = 0;

    void OnTriggerEnter2D(Collider2D otherCol)
    {
        if(otherCol.CompareTag("Carrot")) 
        {
            Debug.Log("Carrot Triggered"); 
            collectedCarrotNum++; 
            Debug.Log("Skor:" + collectedCarrotNum);
            Destroy(otherCol.gameObject);
             

            if (collectedCarrotNum == 6)
            {
                bunny.SetActive(true);
            }
        }
    }
}
