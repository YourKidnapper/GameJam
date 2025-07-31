using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float attackBoostMultiplier = 1f;
    private bool boostReady = false;

    public bool HasBoost => boostReady;

    public void ApplyAttackBoost(float multiplier)
    {
        attackBoostMultiplier = multiplier;
        boostReady = true;
        Debug.Log($"Attack boost активований: x{multiplier} (на одну атаку)");
    }

    public float ConsumeAttackMultiplier()
    {
        if (boostReady)
        {
            boostReady = false;
            float usedMultiplier = attackBoostMultiplier;
            attackBoostMultiplier = 1f;
            Debug.Log($"Attack boost використано: x{usedMultiplier}");
            return usedMultiplier;
        }

        return 1f;
    }
}
