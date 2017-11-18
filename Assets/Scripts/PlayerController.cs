using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,Interactor {


    #region playerSettings
    [SerializeField]
    [Tooltip("Facteur de vitesse sur l'axe X du joueur.")]
    private float playerSpeedFactor = 1;
    [SerializeField]
    [Tooltip("Hauteur de saut atteinte par le joueur.Alors non plus maintenant.")]
    private float jumpHeight = 5;
    [SerializeField]
    [Tooltip("Facteur de vitesse, utilisé pour le sprint et potentiel bonus / malus.")]
    private float accelerationFactor = 1;
    [SerializeField]
    [Tooltip("Facteur d'inerrtie, faut pas abuser non plus. (<100 )")]
    private float inertiaFactor = 70;

    #endregion



    Rigidbody2D r;
    Vector3 acceleration;
    private Interactible interactible = null;
    private enum jump {grounded, jump, doubleJump };
    private jump jumpState=jump.grounded;

    #region playerAnimation
    private Animator playerAnimator;
    private SpriteRenderer spRender;
    private bool facingRigh = true;
    private bool oldDirection = true;
    private bool jumping = false;
    private Vector3 SpeedY; // Speed (ancienY, nouvelY, vitesseY)
    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpState = jump.grounded;
        jumping = false;
        SpeedY= Vector3.zero;
        if (playerAnimator)
        {
            playerAnimator.SetBool("Jumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        jumpState = jump.jump;
        jumping = true;
        if (playerAnimator)
        {
            playerAnimator.SetBool("Jumping", true);
        }   
    }

    public void removeInteractible(Interactible i)
    {
        if (this.interactible == i)
            this.interactible = null;
    }

    public void setCheckpoint(CheckPoint c)
    {
        throw new System.NotImplementedException();
    }

    public void setInteractible(Interactible i)
    {
        this.interactible = i;
    }

    // Use this for initialization
    void Start () {
        acceleration = Vector3.zero;
        r = this.GetComponent<Rigidbody2D>();
        spRender = this.GetComponent<SpriteRenderer>();
        playerAnimator = this.GetComponent<Animator>();
        SpeedY = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InputController.getSprintButtonDown())
        {
            accelerationFactor *= 2; //TODO: c'est très sale.
        }
        else if (InputController.getSprintButtonUp())
        {
            accelerationFactor /= 2;
        }

        acceleration.x = InputController.getXAxis() * playerSpeedFactor * Time.deltaTime * accelerationFactor;


        if (InputController.getJump())
        {
            switch (jumpState)
            {
                case jump.grounded:
                    this.r.AddForce(Vector2.up * jumpHeight);
                    this.r.AddForce(acceleration * inertiaFactor);
                    jumpState = jump.jump;
                    if (playerAnimator)
                    {
                        playerAnimator.SetBool("Jumping", true);
                    }
                    jumping = true;
                    SpeedY.x = this.transform.position.x;
                    break;
                case jump.jump:
                    this.r.AddForce(Vector2.up * jumpHeight);
                    this.r.AddForce(acceleration * inertiaFactor);
                    jumpState = jump.doubleJump;
                    break;
                case jump.doubleJump:
                    break;
            }
            
            //this.r.velocity = acceleration * inertiaFactor;
        }

        /*
        if(r.velocity.x != 0)
        {

            float tmpx = (acceleration.x > 0)
                ? Mathf.Min(0, r.velocity.x - acceleration.x)
                : Mathf.Max(0, r.velocity.x + acceleration.x);
            r.velocity = new Vector2(tmpx, r.velocity.y);

        }
        */
        acceleration.x = InputController.getXAxis() * playerSpeedFactor*Time.deltaTime*accelerationFactor;
        if (playerAnimator)
        {
            playerAnimator.SetFloat("Speed", Mathf.Abs(10 * acceleration.x));
            if (jumping)
            {
                SpeedY.y = SpeedY.x; //new devient old
                SpeedY.x = this.transform.position.y; //enregistrer la position actuelle
                SpeedY.z = SpeedY.y - SpeedY.x;
                playerAnimator.SetFloat("SpeedJump", SpeedY.z);
            }
        }
        if (acceleration.x>0.001)
        {
            facingRigh = true;
            if (!oldDirection)
            {
                spRender.flipX = false;
                oldDirection = facingRigh;
            }
            
        }

        if (acceleration.x<-0.001)
        {
            facingRigh = false;
            if (oldDirection)
            {
                spRender.flipX = true;
                oldDirection = facingRigh;
            }
        }

        this.transform.Translate(acceleration);

        if(interactible != null && InputController.getInteractionButton())
        {
            interactible.doSomeStuff();
            this.interactible = null;
        }

        if (this.transform.position.y < -20)
        {
            Player.die();
        }
    }

    public Vector3 getAcceleration()
    {
        return this.acceleration;
    }
}
