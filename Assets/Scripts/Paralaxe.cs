using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxe : MonoBehaviour {

    //nb : les objets proches de la caméra ont une position en Z plus petite que ceux loins de la caméra.
   //  Traditionnellement le joueur est à zéro.

    private float depthLayer;
    private GameObject player;
    private PlayerController playerControlScript;
    private float depthPlayer=0.0f;
    private float depthOffSet;
    private Vector3 accelerationPlayer;
    private Vector3 accelerationLayer;

	// Use this for initialization
	void Start () {
        depthLayer = this.transform.position.z;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            depthPlayer = player.transform.position.z;
        }
        else
        {
            Debug.LogWarning("No player detected", this.gameObject);
        }
        depthOffSet = depthLayer - depthPlayer;

        playerControlScript = player.GetComponent<PlayerController>();
        if (!playerControlScript)
        {
            Debug.LogWarning("No player controller attached to the target", player);
        }

        accelerationPlayer = new Vector3();
        accelerationLayer = new Vector3(0,0,0);
	}
	
	// Update après playerController : pas de frame de décallage dans la récupération de l'accélération
	void LateUpdate ()
    {
        accelerationPlayer =playerControlScript.getAcceleration();
        accelerationLayer.x = accelerationPlayer.x *depthOffSet;
        //TODO : mettre une limite inf pour éviter le scrolling vers la gauche ?
        this.transform.Translate(accelerationLayer);
    }

}
