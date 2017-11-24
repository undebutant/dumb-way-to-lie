using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private static Player instance;

    private static CheckPoint chckpt = null;
    private static Rigidbody2D r= null;
    private static Vector3 startPosition;
    private Animator playerAnimator;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            startPosition = this.transform.position;
        }
        else
        {
            Destroy(this);
        }

    }


    // Use this for initialization
    void Start () {
        r = this.GetComponent<Rigidbody2D>();
        playerAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void setCheckPoint(CheckPoint c)
    {
        chckpt = c;
    }

    public static void die()
    {
        r.velocity = Vector2.zero;
        Player.instance.transform.position = (chckpt!= null)
                                            ? chckpt.transform.position
                                            :startPosition;

        /*if (instance.playerAnimator)
        {
            instance.playerAnimator.SetBool("Jumping", false);
        }*/
    }


    


}
