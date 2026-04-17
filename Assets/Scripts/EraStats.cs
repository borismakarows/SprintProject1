using UnityEngine;

[CreateAssetMenu(fileName = "NewEra", menuName = "Add New Era")]
public class EraStats : ScriptableObject
{
    [Header("Movement")]
    public float maxSpeed;
    public float acc;
    public float deacc; 
    public float velPow;

    [Header("Verticality")]
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float climbSpeed;
    public float gravityScale;

    [Header("Animation Control")]
    public RuntimeAnimatorController animator;
 
}
