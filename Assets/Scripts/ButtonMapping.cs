using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMapping : MonoBehaviour {

    //if on windows
    public string winHorizAxis, winVertAxis;
    public string winUseButton, winThrowButton;
    public string winStartButton;

    //if on mac
    public string macHorizAxis, macVertAxis;
    public string macUseButton, macThrowButton;
    public string macStartButton;

    private PlayerController pc;

    void Awake()
    {
        pc = GetComponent<PlayerController>();
        SetButtons();
    }

    public void SetButtons()
    {
        //if on windows
        #if UNITY_STANDALONE_WIN
            pc.horizAxis = winHorizAxis;
            pc.vertAxis = winVertAxis;
            pc.useButton = winUseButton;
            pc.throwButton = winThrowButton;
            pc.startButton = winStartButton;
        #endif

        //if on mac
        #if UNITY_STANDALONE_OSX
            pc.horizAxis = macHorizAxis;
            pc.vertAxis = macVertAxis;
            pc.useButton = macUseButton;
            pc.throwButton = macThrowButton;
            pc.startButton = macStartButton;
        #endif
    }
}
