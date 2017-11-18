using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJ
{
    public string name;
    public List<Encounter> encounters;
}

public class Encounter
{
    public string id;
    public string posX;
    public string posY;
    public List<Step> steps;
}

public class Step
{
    public string requiredEventID;
    public string incompatibleEventID;
    public string textPNJ;
    public List<Choice> choices;
}

public class Choice
{
    public string textButton;
    public string textDisplay;
    public string response;
    public string eventID;
}
