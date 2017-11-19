using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    private static GameManager instance;
    private static Dictionary<string,PNJ> npcs;
    [SerializeField][Tooltip ("Chemin d'accès des .xml des personnages non joueurs ")]
    private string PNJPath = "./Assets/XML/PNJ/";
    [SerializeField]
    [Tooltip("Chemin d'accès des .xml des personnages non joueurs ")]
    private string EventPath = "./Assets/XML/Events/";
    private static Dictionary<string, bool> events;

    private static int day = 0;
    
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
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
        initializePNJ();
        incrementDay();    
    }
	

    void incrementDay()
    {
        NPC pnj;
        day++;
        Encounter e;
        foreach (KeyValuePair<string,PNJ> p in npcs)
        {

            if (int.Parse(p.Value.nextDayEncounter) == day)
            {
                //foreach (string s in Directory.GetFiles(".")) Debug.Log(s);
               
                pnj = Resources.Load<NPC>(p.Value.name);
                pnj = Instantiate(pnj);
                pnj.setItemName(p.Key);
                e = p.Value.getCurrentEncounter();//getEnum().Current;
                pnj.transform.position = new Vector2(float.Parse(e.posX),float.Parse(e.posY));
                //Debug.Log("JAMBON");
            }
            
        }
    }

    public static Step getNextValidStep(string name)
    {
        PNJ p = GameManager.npcs[name];
        Encounter e = p.getCurrentEncounter();
        Step s = e.getCurrentStep();
        while (s!= null)
        {
            e.incrementStep();           
            if (s.requiredEventID != null)
            {
                if (true)//CHECK
                {
                    return s;
                }
              
            }
            if (s.incompatibleEventID != null)
            {
                if (true)//CHECK !Incompatible
                {
                    return s;
                }
            }

            if (s.incompatibleEventID == s.requiredEventID)
            {
                Debug.Log("CONNARD");
                return s;
            }
            s = e.getCurrentStep();
            
        } 
        Debug.Log("Very Sad");
        return null;

    }

    void instantiateAllPNJ()
    {

    }

    void initializePNJ()
    {
        PNJ currentPNJ;
        foreach (string s in Directory.GetFiles(PNJPath,"*.xml"))
        {
            currentPNJ = XmlHelpers.DeserializeFromXML<PNJ>(s);
            if (currentPNJ != null)
            {
                npcs.Add(currentPNJ.name, currentPNJ);
            }
        }
    }

    void initializeEvent()
    {
        string currentEvent;
        foreach (string s in Directory.GetFiles(EventPath))
        {
            currentEvent = XmlHelpers.DeserializeFromXML<string>(s);
            events.Add(currentEvent, false);
        }
    }



    // Update is called once per frame
    void Update () {
		
	}

    public static void nextEncounter(string name)
    {
        if(npcs[name] != null)
        {
            npcs[name].incrementEncounter();//getEnum().MoveNext(); //TODO Peutêtrebuggé
        }
    }
}
