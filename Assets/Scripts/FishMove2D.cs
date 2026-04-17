using UnityEngine;

public class FishMove2D : MonoBehaviour
{
    [Header("Swim Stats")]
    [SerializeField] float speed = 5f;
    [SerializeField] float frequency = 2.0f;
    [SerializeField] float magnitude = 0.5f;

    [Header("Route & Vision")]
    private bool isChasing = false;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startRoute = 0f;
    private float endRoute = 0f;

    [SerializeField] private GameObject player;
    [SerializeField] GameObject route;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float viewAngle = 45f; // Total cone is 90 degrees (45 each way)
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    private SpriteRenderer sprite;
    
    
    
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else
        {
            if (isChasing)
            {
                ReturnToRoute();
            }
        }
        Swim();
    }
    void init()
    {
        player =  GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        startRotation = transform.rotation;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        initRoute();
    }
    void initRoute()
    {
        if (route != null)
        {
            route.GetComponent<Renderer>().enabled = false;
            startRoute = route.transform.position.x-(route.transform.localScale.x/2);
            endRoute = route.transform.position.x+(route.transform.localScale.x/2);
        }
        else
        {
            startRoute = transform.position.x;
            endRoute = transform.position.x;
        }
    }
    void Reset()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
    
    void ReturnToRoute() {
        transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * 2f);

        Vector3 targetPos = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, startRotation) < 1f) {
            isChasing = false; 
        }
    }
    
    void Swim()
    {
        if (!isChasing)
        {
            //change directions
            if (transform.position.x > endRoute)
            {
                sprite.flipY = true;
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (transform.position.x < startRoute)
            {
                sprite.flipY = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            Vector3 playerDir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    bool CanSeePlayer()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance, targetMask);
        
        foreach (var target in targetsInViewRadius) {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
    
            if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2) {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                Debug.DrawRay(transform.position, dirToTarget * distanceToTarget, Color.red);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstructionMask);

                if (hit.collider != null) {
                    if (hit.collider.CompareTag("Player")) {
                        Debug.Log("I see the player directly!");
                        return true;
                    } else {
                        //Debug.Log("Something (a wall) is in the way.");
                    }
                }
            }
        }
        return false;
    }
}
