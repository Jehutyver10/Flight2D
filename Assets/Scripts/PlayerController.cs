﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CharacterBehavior))]
public class PlayerController : MonoBehaviour {

    private CharacterBehavior character; //character controller attached to player object
    [HideInInspector]

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterBehavior>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        character.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
            Input.GetAxis("Dash") < 0 ? true:false);
        //print("DASH: " + CrossPlatformInputManager.GetAxis("Dash"));
        
	}
}
