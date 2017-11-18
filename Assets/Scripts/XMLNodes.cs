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


    private List<Encounter>.Enumerator it;

    public List<Encounter>.Enumerator getEnum()
    {
        if(it.Current == null)
        {
            it = encounters.GetEnumerator();
            it.MoveNext();
        }
        return it;
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

    private List<Step>.Enumerator it;

    public List<Step>.Enumerator getEnum()
    {
        if (it.Current == null)
        {
            it = steps.GetEnumerator();
            it.MoveNext();
        }
        return it;
    }


}

public class Step
{
    Step(){ choices = new List<Choice>();}
    [XmlAttribute("idStep")]
    public string idStep;
    [XmlAttribute("requiredEventID")]
    public string requiredEventID;
    [XmlAttribute("incompatibleEventID")]
    public string incompatibleEventID;
    public string textPNJ;

    [XmlElement("Choice")]
    public List<Choice> choices;

    private List<Choice>.Enumerator it;

    public List<Choice>.Enumerator getEnum()
    {
        if (it.Current == null)
        {
            it = choices.GetEnumerator();
            it.MoveNext();
        }
        return it;
    }
}

public class Choice
{
    public string textButton;
    public string textDisplay;
    public string response;
    public string eventID;
}
