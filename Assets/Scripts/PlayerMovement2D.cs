using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float acc = 50f;
    [SerializeField] float deAcc = 50f;
    [SerializeField] float velPow = 0.9f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 moveInput;

    void Awake() => rb = GetComponent<Rigidbody2D>();
    void OnValidate() => rb = GetComponent<Rigidbody2D>();

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    void Update()
    {
        float targetSpeed = moveInput.x * maxSpeed;
        
        float speedDif = targetSpeed - rb.linearVelocityX;
       
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acc : deAcc;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPow) * Mathf.Sign(speedDif) * Time.deltaTime;

        rb.AddForce(movement * Vector2.right);
    }
  
}

