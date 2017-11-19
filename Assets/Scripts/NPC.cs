using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactible
{

    private SpriteRenderer PNJRenderer;
    private Animator PNJAnimator;
    private float originePosition;
    private Vector3 currentPosition;
    private float rangeMouv = 20f; //xml
    private float mouvSpeed = 5;
    private bool facingRight = true;
    private bool canMouv;
    private bool mayWalk=true;  //xml


    public override void doSomeStuff()
    {
        base.doSomeStuff();
        CanvasManager.displayChoice(this.itemName);
        SideScrolling.ChangeTarget(SideScrolling.instance.emptyTarget); //Do an offset to the left
        SideScrolling.zoomOnTarget(); //TODO Offset

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        canMouv = false;
        if(PNJAnimator)
        {
            PNJAnimator.SetBool("Walking",false);
        }
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {;
        base.OnTriggerExit2D(collision);
        canMouv = mayWalk;
        facingRight = false;
        if (PNJRenderer)
        {
            PNJRenderer.flipX = true;
        }
        if (PNJAnimator)
        {
            PNJAnimator.SetBool("Walking", mayWalk);
        }
    }

    // Use this for initialization
    void Start()
    {
        PNJRenderer = this.GetComponent<SpriteRenderer>();
        PNJAnimator = this.GetComponent<Animator>();
        if (PNJAnimator)
        {
            PNJAnimator.SetBool("Walking", mayWalk);
        }
        originePosition = this.transform.position.x;
        currentPosition = this.transform.position;
        canMouv = mayWalk;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMouv)
        {
            mouvement();
        }
        
    }

    private void mouvement()
    {
        currentPosition.x = this.transform.position.x;
        if (facingRight)
        {
            currentPosition.x += mouvSpeed * Time.deltaTime;
            if (currentPosition.x > originePosition + (rangeMouv / 2))
            {
                facingRight = false;
                if (PNJRenderer)
                {
                    PNJRenderer.flipX = true;
                }
                
            }
        }
        else
        {
            currentPosition.x -= mouvSpeed * Time.deltaTime;
            if (currentPosition.x < originePosition - (rangeMouv / 2))
            {
                facingRight = true;
                if (PNJRenderer)
                {
                    PNJRenderer.flipX = false;
                }
                
            }
        }
        this.transform.position = currentPosition;
    }

    public void setRangeMouv(float x)
    {
        rangeMouv = x;
    }
}
