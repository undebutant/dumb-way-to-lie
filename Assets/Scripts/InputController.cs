using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    
 
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    static public float getXAxis()
    {
        return Input.GetAxis("Horizontal");

    }

    static public float getYAxis()
    {
        return Input.GetAxis("Vertical");
    }

    static public bool getInteractionButton()
    {
        return Input.GetButtonUp("Fire1");
    }

    static public bool getSprintButtonDown()
    {
        return Input.GetButtonDown("Fire2");
    }

    static public bool getSprintButtonUp()
    {
        return Input.GetButtonUp("Fire2");
    }

    static public bool getJump()
    {
        return Input.GetButtonDown("Jump");
    }


}
