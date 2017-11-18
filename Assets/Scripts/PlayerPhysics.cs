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
        gravityConstant = gravityDirection * 2 * jumpHeight * jumpSpeed * jumpSpeed / (jumpDistance * jumpDistance);
        acceleration = Vector3.zero;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Jamgon");
        if (Vector3.Dot(this.velocity, collision.contacts[0].normal) <= 0)
        {
            Debug.Log("Saucisson");
            if (Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, Vector3.up)) > 0.5)
            {
                finalTranslation.Scale(Vector3.right);
                this.velocity.Scale(Vector3.right);
                gravityEnable = false;
            }
            else
            {
                finalTranslation.Scale(Vector3.up);
                this.velocity.Scale(Vector3.up);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        this.gravityEnable = true;
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


        RaycastHit2D rch;
        if(Vector3.Dot(finalTranslation, Vector3.left) > 0)
        {
            rch = Physics2D.Raycast(this.transform.position, Vector3.left, LayerMask.GetMask("Platforms"));
            if (rch.collider != null)
            {
                if (Vector3.Distance(this.transform.position, rch.point) > Mathf.Abs(finalTranslation.x))
                {
                    finalTranslation.x = -Vector3.Distance(this.transform.position, rch.point);
                    velocity.x = 0;
                }
            }
        }
        else
        {
            rch = Physics2D.Raycast(this.transform.position, Vector3.right, LayerMask.GetMask("Platforms"));
            if (rch.collider != null)
            {
                if (Vector3.Distance(this.transform.position, rch.point) > Mathf.Abs(finalTranslation.x))
                {
                    finalTranslation.x = Vector3.Distance(this.transform.position, rch.point);
                    velocity.x = 0;
                }
            }
        }

        if (Vector3.Dot(finalTranslation, Vector3.up) > 0)
        {
            rch = Physics2D.Raycast(this.transform.position, Vector3.up, LayerMask.GetMask("Platforms"));
            if (rch.collider != null)
            {
                if (Vector3.Distance(this.transform.position, rch.point) > Mathf.Abs(finalTranslation.y))
                {
                    finalTranslation.y = Vector3.Distance(this.transform.position, rch.point);
                    velocity.y = 0;
                }
            }
        }
        else
        {
            rch = Physics2D.Raycast(this.transform.position, Vector3.down, LayerMask.GetMask("Platforms"));
            if (rch.collider != null)
            {
                Debug.Log("Genre");
                if (Vector3.Distance(this.transform.position, rch.point) > Mathf.Abs(finalTranslation.y))
                {
                    finalTranslation.y = -Vector3.Distance(this.transform.position, rch.point);
                    velocity.y = 0;
                }
            }
        }





        this.transform.Translate(finalTranslation);//TEMPORARY


        //this.GetComponent<Rigidbody2D>().AddForce(new Vector2(finalTranslation.x, finalTranslation.y));
        //this.GetComponent<Rigidbody2D>().velocity = finalTranslation;
        
    }
}
