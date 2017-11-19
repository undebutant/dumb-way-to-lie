using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("PNJ")]
public class PNJ
{
    PNJ() { encounters = new List<Encounter>();}
    public string name;
    public string prefab;
    /// <summary>
    /// NextDayEncounter. Permet de savoir quand le personnage doit être chargé.
    /// </summary>
    [XmlElement("firstDay")]
    public string nextDayEncounter; //NextDayEncounter
    [XmlElement("Encounter")]
    public List<Encounter> encounters;


    private int currentId =0;

    public void incrementEncounter() { currentId++; }


    public Encounter getCurrentEncounter()
    {
        int i = 0;
        foreach (Encounter e in encounters)
        {
            if (i == currentId)
                return e;
            i++;
        }
        return null;
    }
    

    public bool isLoaded = false;


}

public class Encounter
{
    Encounter() { steps = new List<Step>(); }
    [XmlAttribute("id")]
    public string id;
    [XmlAttribute("posX")]
    public string posX;
    [XmlAttribute("posY")]
    public string posY;
    [XmlElement("Step")]
    public List<Step> steps;
    public string nextDayEncounter;

    private int currentId = 0;

    public void incrementStep() { currentId++; }


    public Step getCurrentStep()
    {
        int i = 0;
        foreach (Step s in steps)
        {
            if (i == currentId)
                return s;
            i++;
        }
        return null;
    }


}

public class Step
{
    Step() { choices = new List<Choice>(); }
    [XmlAttribute("idStep")]
    public string idStep;
    [XmlAttribute("requiredEventID")]
    public string requiredEventID;
    [XmlAttribute("incompatibleEventID")]
    public string incompatibleEventID;
    public string textPNJ;

    [XmlElement("Choice")]
    public List<Choice> choices;

    private int currentId = 0;

    public void incrementChoice() { currentId++; }


    public Choice getCurrentChoice()
    {
        int i = 0;
        foreach (Choice c in choices)
        {
            if (i == currentId)
                return c;
            i++;
        }
        return null;
    }
}

public class Choice
{
    public string textButton;
    public string textDisplay;
    public string response;
    public string eventID;
}
