using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public static MenuManager instance;

    private void Awake()
    {
        if (instance== null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("SceneGraphique"); //TODO : Remplacer par la bonne scene
    }

    public void OnClickLeave()
    {
        Application.Quit();
    }

}
