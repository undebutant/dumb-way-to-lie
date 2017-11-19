using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechFactory : MonoBehaviour
{
    public static SpeechFactory instance;
    Queue<GameObject> UpPanelPlayer;
    Queue<GameObject> UpPanelPnj;
    Queue<GameObject> DownPanelPlayer;
    Queue<GameObject> DownPanelPnj;


    GameObject upSamplePanelPlayer;
    GameObject upSamplePanelPNJ;
    GameObject downSamplePanelPlayer;
    GameObject downSamplePanelPNJ;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        UpPanelPlayer = new Queue<GameObject>();
        UpPanelPnj = new Queue<GameObject>();
        DownPanelPlayer = new Queue<GameObject>();
        DownPanelPnj = new Queue<GameObject>();

        upSamplePanelPlayer = Resources.Load<GameObject>("upPanelPlayer");
        upSamplePanelPNJ = Resources.Load<GameObject>("upPanelPNJ");
        downSamplePanelPlayer = Resources.Load<GameObject>("downPanelPlayer");
        downSamplePanelPNJ = Resources.Load<GameObject>("downPanelPNJ");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject getUpPlayerPanel()
    {
        if (instance.UpPanelPlayer.Count == 0)
        {
            return (instance.upSamplePanelPlayer);
        }
        else
        {
            return instance.UpPanelPlayer.Dequeue();
        }
    }

    public static GameObject getUpPNJPanel()
    {
        if (instance.UpPanelPnj.Count == 0)
        {
            
            return (instance.upSamplePanelPNJ);
        }
        else
        {
            return instance.UpPanelPnj.Dequeue();
        }
    }


    public static void giveBackUPPNJ(GameObject g)
    {
        instance.UpPanelPnj.Enqueue(g);
    }

    public static void giveBackUPPlayer(GameObject g)
    {
        instance.UpPanelPlayer.Enqueue(g);
    }


    public static GameObject getDownPlayerPanel()
    {
        if (instance.DownPanelPlayer.Count == 0)
        {
            return (instance.downSamplePanelPlayer);
        }
        else
        {
            return instance.DownPanelPlayer.Dequeue();
        }
    }

    public static GameObject getDownPNJPanel()
    {
        if (instance.DownPanelPnj.Count == 0)
        {
            return (instance.downSamplePanelPNJ);
        }
        else
        {
            return instance.DownPanelPnj.Dequeue();
        }
    }


    public static void giveBackDownPNJ(GameObject g)
    {
        instance.DownPanelPnj.Enqueue(g);
    }

    public static void giveBackDownPlayer(GameObject g)
    {
        instance.DownPanelPlayer.Enqueue(g);
    }

}
