using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {


    private bool isDisplay = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player" && !isDisplay)
        {
            //Player player = collision.gameObject.GetComponent<Player>();
            //if (player != null)
            //{
            Player.setCheckPoint(this);
            //}
            displayLight();
        }
    }


    void displayLight()
    {
        //TODO: Render Animation
        isDisplay = true;
        Debug.Log("Checkpoint Activé");
    }

    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
