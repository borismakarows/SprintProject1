using UnityEngine;

public class CloudBehav : MonoBehaviour
{
    [Header("Cloud Prefab")]
    [SerializeField] float speed = 5.0f;
    private bool movingleft = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = Vector3.one * Random.Range(0.8f, 2f);
        if(transform.position.x < 0) movingleft = false;
        else movingleft = true;
    }

    // Update is called once per frame
    void Update()
    {
        OutOfBounds();
        if (movingleft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    void OutOfBounds()
    {
        if (transform.position.x > 45 || transform.position.x < -45)
        {
            Destroy(gameObject);
        }
    }
}
