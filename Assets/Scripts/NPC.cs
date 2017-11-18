using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible {

    public override void doSomeStuff()
    {
        base.doSomeStuff();
        CanvasManager.displayChoice(this.idEvent);
        SideScrolling.ChangeTarget(this.gameObject);
        SideScrolling.zoomOnTarget();

    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
