using UnityEngine;

public class CooldownHolder : MonoBehaviour
{
    float _absMinCooldownMult = 0.3f;
    static float _globalCooldownMultiplier;

    public static float GlobalCooldownMultiplier => _globalCooldownMultiplier;

    void Start()
    {
        EventBus.Instance.OnBossHealthChange += ReduceGlobalCooldown;
        _globalCooldownMultiplier = 1f;
    }

    void ReduceGlobalCooldown(float percentage)
    {
        _globalCooldownMultiplier = Mathf.Max(percentage, _absMinCooldownMult);
        Debug.Log($"Cooldown multiplier is now: {_globalCooldownMultiplier}");
    }
}
