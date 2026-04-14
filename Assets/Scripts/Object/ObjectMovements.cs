using System;
using Unity.VisualScripting;
using UnityEngine;

public enum MovementTypes
{
    Sine,
    Circular,
    Noise
}


[RequireComponent(typeof(Rigidbody2D))]
public class ObjectMovements : MonoBehaviour
{
    [Header("Select Movement Type")]
    [SerializeField] MovementTypes MovementType = MovementTypes.Sine;

    [Header("Movement Parameters")]
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float frequency = 1.0f;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    Vector2 startPos;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        startPos = rb.position;
    } 
    void OnValidate() { if(rb == null) rb = GetComponent<Rigidbody2D>(); } 

    void FixedUpdate()
    {
        switch (MovementType)
        {
            case MovementTypes.Sine: SineMove(); break;
            case MovementTypes.Circular: CircMove(); break;
            case MovementTypes.Noise: NoiseMove(); break;
            default: SineMove(); break;
        };
    }

    void SineMove()
    {
        float yOffset = Mathf.Sin(Time.fixedTime * frequency) * amplitude;
        Vector2 targetPos = new Vector2(startPos.x, startPos.y + yOffset);
        rb.MovePosition(targetPos);
    }

    void CircMove()
    {
        float x = Mathf.Cos(Time.fixedTime * frequency) * amplitude;
        float y = Mathf.Sin(Time.fixedTime * frequency) * amplitude;
        rb.MovePosition(startPos + new Vector2(x, y));
    }

    void NoiseMove()
    {
        float x = (Mathf.PerlinNoise(Time.fixedTime * frequency, 0) - 0.5f) * amplitude;
        float y = (Mathf.PerlinNoise(0, Time.fixedTime * frequency) - 0.5f) * amplitude;
        rb.MovePosition(startPos + new Vector2(x, y));
    }

}
