using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //################### SINGLETON ###################
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }
    //################### END SINGLETON ###############

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
