﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible:MonoBehaviour
{
    [SerializeField] [Tooltip("Nombre d'intéractions possible avec cet item. (-1 pour illimité)")]
    protected int nbInteraction = 3;
    protected int countInteraction = 0;
    protected string itemName;
    protected bool isDisplay;
    [SerializeField] [Tooltip("Bulle d'interaction à afficher. Doit être positionnée au bon endroit.")]
    protected GameObject balloon;

    protected void displayBalloon()
    {
        isDisplay = true;
        balloon.SetActive(true);
    }

    protected void hideBalloon()
    {
        isDisplay = false;
        balloon.SetActive(false);

    }

    public void setItemName(string it)
    {
        this.itemName = it; 
    }

    /// <summary>
    /// This function is called by the player, when he interact with this item. Then, it should either start a dialog or break some stuff.
    /// </summary>
    public virtual void doSomeStuff()
    {
        this.countInteraction++; //Permet d'éviter au joueur de reparler à un PNJ déjà abordé
        this.hideBalloon();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isDisplay && (countInteraction < nbInteraction || nbInteraction < 0))
        {
            Interactor interactor = collision.gameObject.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.setInteractible(this);
            }

            displayBalloon();

        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isDisplay)
        {
            Interactor interactor = collision.gameObject.GetComponent<Interactor>();
            if (interactor != null)
            {
                interactor.removeInteractible(this);
            }
            hideBalloon();

        }
    }


}
