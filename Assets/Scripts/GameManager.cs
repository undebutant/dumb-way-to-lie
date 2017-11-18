using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    private static Dictionary<string,PNJ> npcs;
    [SerializeField][Tooltip ("Chemin d'accès des .xml des personnages non joueurs ")]
    private string PNJPath = "./XML/PNJ/";
    [SerializeField]
    [Tooltip("Chemin d'accès des .xml des personnages non joueurs ")]
    private string EventPath = "./XML/Events/";
    private static Dictionary<string, bool> events;

    private void Awake()
    {
        if (instance != null)
        {

            Destroy(this);
        }else{
            instance = this;
        }
            
    }
    
    // Use this for initialization
    void Start () {
        npcs = new Dictionary<string, PNJ>();
      
	}
	
    void initializePNJ()
    {
        PNJ currentPNJ;
        foreach (string s in Directory.GetFiles(PNJPath))
        {
            currentPNJ = XmlHelpers.DeserializeFromXML<PNJ>(PNJPath+s);
            npcs.Add(currentPNJ.name, currentPNJ);
        }
    }

    void initializeEvent()
    {
        string currentEvent;
        foreach (string s in Directory.GetFiles(EventPath))
        {
            currentEvent = XmlHelpers.DeserializeFromXML<string>(EventPath + s);
            events.Add(currentEvent, false);
        }
    }



    // Update is called once per frame
    void Update () {
		
	}
}
