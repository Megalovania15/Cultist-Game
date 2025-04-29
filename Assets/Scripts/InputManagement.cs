using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagement : MonoBehaviour
{
    private PlayerInputManager inputManager;

    private InputAction joinAction;

    void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerJoined(PlayerInput input)
    { 
        
    }

    //input manager, needs to look for the devices and assign one to a player
}
