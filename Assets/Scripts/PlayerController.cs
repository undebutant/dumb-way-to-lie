using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    #region playerSettings
    [SerializeField]
    [Tooltip("Facteur de vitesse sur l'axe X du joueur.")]
    private float playerSpeedFactor = 1;
    [SerializeField]
    [Tooltip("Hauteur de saut atteinte par le joueur.Alors non plus maintenant.")]
    private float jumpHeight = 5;

    #endregion

    Rigidbody2D r;
    
    Vector3 acceleration;

    // Use this for initialization
    void Start () {
        acceleration = Vector3.zero;
        r = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InputController.getJump())
        {
            this.r.AddForce(Vector2.up * jumpHeight);
        }

        acceleration.x = InputController.getXAxis() * playerSpeedFactor*Time.deltaTime;
        this.transform.Translate(acceleration);
	}
}
