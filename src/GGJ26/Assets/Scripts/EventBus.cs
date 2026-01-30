using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Action<float> OnPlayerHealthChange;
    public Action<float> OnBossHealthChange;
    public Action OnBossDefeat;
    public Action OnPlayerDeath;
}
