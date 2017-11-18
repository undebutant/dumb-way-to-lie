using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {


#region playerSettings
    [SerializeField] [Tooltip("Facteur de vitesse sur l'axe X du joueur.")]
    private float playerSpeedFactor = 1;
    [SerializeField] [Tooltip("Hauteur de saut atteinte par le joueur.")]
    private float jumpHeight = 5;
    [SerializeField] [Tooltip("Temps pour atteindre la hauteur maximale divisé par la distance.")]
    private float jumpSpeed = 3;
    [SerializeField][Tooltip("Distance pour atteindre la hauteur de saut maximum.")]
    private float jumpDistance = 3;
#endregion




 #region Parametres liés à la gravité
    [SerializeField][Tooltip("Attribut booléen définissant la gravité du joueur est activée ou non.")]
    private bool gravityEnable = false;
    private Vector3 gravityConstant;
    private float gravityFactor = 1;
    private Vector3 gravityDirection = Vector3.down;

    #endregion

    private bool canJump = true;
    private bool jump;
    private float userInputX;

    private Vector3 velocity; //Vitesse globale du joueur
    private Vector3 acceleration; //Acceleration lors de cette frame
    private Vector3 finalTranslation; // velocity + acceleration

	// Use this for initialization
	void Start ()
    {
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
	
    

	// Update is called once per frame
	void Update ()
    {
        acceleration.x = Input.GetAxis("Horizontal") * playerSpeedFactor;
        if(canJump)
            jump = Input.GetAxis("Jump") > 0.2;



        if(!gravityEnable)//Le joueur est au sol
        {
            gravityConstant = gravityDirection * 2 * jumpHeight * jumpSpeed * jumpSpeed / (jumpDistance * jumpDistance);  //On met à jour la constante de gravité
            gravityFactor = 1.0f;
            //Add inertia?
            canJump = true;
        }


        if (jump)
        {
            velocity.y = 2 * jumpHeight * jumpSpeed / jumpDistance;
            canJump = false;
            gravityEnable = true;
        }


        //Permet de tomber plus vite après avoir atteint le pallier de saut.
        if(gravityEnable && velocity.y <= 0)
        {
            gravityFactor = 2;
        }


        finalTranslation = (gravityEnable)
            ? velocity + gravityConstant * gravityFactor * Time.deltaTime + acceleration
            : velocity + acceleration;

        velocity = (gravityEnable)
            ? velocity + gravityConstant * gravityFactor * Time.deltaTime
            : velocity;

        finalTranslation = (velocity + acceleration) * Time.deltaTime;
        this.transform.Translate(new Vector3(userInputX*playerSpeedFactor*Time.deltaTime, 0));//TEMPORARY
	}
}
