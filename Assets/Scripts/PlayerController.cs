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
    #endregion

    Rigidbody2D r;
    Vector3 acceleration;
    private Interactible interactible = null;

    public void removeInteractible(Interactible i)
    {
        if (this.interactible == i)
            this.interactible = null;
    }

    public void setInteractible(Interactible i)
    {
        this.interactible = i;
    }

    // Use this for initialization
    void Start () {
        acceleration = Vector3.zero;
        r = this.GetComponent<Rigidbody2D>();
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

        if (InputController.getJump())
        {
            this.r.AddForce(Vector2.up * jumpHeight);
        }

        acceleration.x = InputController.getXAxis() * playerSpeedFactor*Time.deltaTime*accelerationFactor;
        this.transform.Translate(acceleration);

        if(interactible != null && InputController.getInteractionButton())
        {
            interactible.doSomeStuff();
            this.interactible = null;
        }

    }
}
