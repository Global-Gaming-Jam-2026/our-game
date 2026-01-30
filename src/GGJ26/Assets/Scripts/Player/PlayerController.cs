using UnityEngine;

public class PlayerController : Entity
{
    //################### SINGLETON ###################
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }
    //################### END SINGLETON ###############

    [Header("States")]
    [SerializeField] StateBase _idleState;

    //private void Start()
    //{
    //    InitMachine();
    //    _machine.SetState(_idleState);
    //}
}
