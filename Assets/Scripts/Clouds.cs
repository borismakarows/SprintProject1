using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Clouds : MonoBehaviour
{
    [Header("Clouds")] 
    [SerializeField] int min_cd = 2;
    [SerializeField] int max_cd = 20;
    [SerializeField] GameObject[] cloudPrefab;
    [SerializeField] int max_clouds = 6;
    private int current_cd = 0;
    private int cd = 0;
    private int active_clouds = 0;
    
    
    void Update()
    {
        if (current_cd >= cd)
        {
            active_clouds = GameObject.FindGameObjectsWithTag("cloud").Length;
            if(active_clouds < max_clouds)
            spawnCloud();
            cd = Random.Range(min_cd, max_cd);
            current_cd = 0;
        } else current_cd++;
    }
    void spawnCloud()
    {
            Instantiate(cloudPrefab[Random.Range(0, cloudPrefab.Length-1)], new Vector3(Random.Range(-40, 40), Random.Range(10, 20),0), Quaternion.identity);
    }
}
