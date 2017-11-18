using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager instance;

    [SerializeField] [Tooltip("Canvas du choix utilisateur lors de dialogues")]
    private Canvas choiceCanvas;

    [SerializeField] [Tooltip("Fade in / out time duration")]
    private float time = 2;


    [SerializeField]
    private UnityEngine.UI.Text firstChoice;
    [SerializeField]
    private UnityEngine.UI.Text secondChoice;

    Choice c1;
    Choice c2;

    string currentName;

    bool isListeningPnj = false;

    private List<GameObject> garbage;


    [SerializeField]
    float timePopBubble = 0.5f;

    GameObject upBubble;
    GameObject bottomBubble;

    private Step currentStep;
    
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
        garbage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void displayChoice(string name)
    {
        if(instance.choiceCanvas != null)
        {
            instance.choiceCanvas.gameObject.SetActive(true);
            instance.StartCoroutine(fadeIn(instance.choiceCanvas.GetComponent<CanvasGroup>()));
            instance.currentName = name;
            continueConversation(name);
        }
    }

    public static void continueConversation(string name)
    {
        instance.currentStep = GameManager.getNextValidStep(name);

        string textValue = (instance.currentStep != null)
            ? instance.currentStep.textPNJ
            : "J'en ai fini avec vous.";

        GameObject panel;

        if (instance.upBubble == null)
        {
            panel = SpeechFactory.getUpPNJPanel();
            instance.upBubble = panel;
            panel.GetComponent<UnityEngine.UI.Text>().text = textValue;
        }
        else if (instance.bottomBubble == null)
        {
            panel = SpeechFactory.getDownPNJPanel();
            instance.bottomBubble = panel;
            panel.GetComponent<UnityEngine.UI.Text>().text = textValue;
        }

        List<Choice>.Enumerator it = instance.currentStep.getEnum();
        if(it.Current != null)
        {
            instance.firstChoice.text = it.Current.textButton;
            instance.c1 = it.Current;

            instance.secondChoice.text = it.Current.textButton; //TODO Sorry.
            instance.c2 = it.Current;

            if (it.MoveNext())
            {
                instance.secondChoice.text = it.Current.textButton;
                instance.c2 = it.Current;
            }
        }
        else
        {
            instance.firstChoice.text = "Partir.";
            instance.secondChoice.text = "Partir, vraiment.";
            instance.c2 = null;
            instance.c1 = null;
        }

    }


    public static void makeChoice(int idChoice)
    {
        if (instance.isListeningPnj)
        {

            if (instance.c1 == null && instance.c2 == null)
            {
                instance.StartCoroutine(fadeOut(instance.choiceCanvas.GetComponent<CanvasGroup>()));
            }
            else
            {
                string textValue="";
                if (idChoice == 0)
                {
                    textValue = instance.c1.textDisplay;
                    instance.c2 = null;
                }
                else
                {
                    instance.c1 = null;
                    textValue = instance.c2.textDisplay;
                }
                

                GameObject panel;
                if (instance.upBubble == null)
                {
                    panel = SpeechFactory.getUpPlayerPanel();
                    instance.upBubble = panel;
                    panel.GetComponent<UnityEngine.UI.Text>().text = textValue;
                }
                else if (instance.bottomBubble == null)
                {
                    panel = SpeechFactory.getDownPlayerPanel();
                    instance.bottomBubble = panel;
                    panel.GetComponent<UnityEngine.UI.Text>().text = textValue;
                }
                else
                {
                    panel = SpeechFactory.getDownPlayerPanel();
                    instance.StartCoroutine(displayPlayerBubble(panel));//Coroutine faire monter
                }
                //TODO: UpdateBoolTABLE
            }


        }
        //COROUTIN



        //Locker les boutons
        //Coroutine (1 secondes) qui affiche la réponse.
        //MoveNext
        //Recharche le contenu des boutons.
    }




    public static void hideChoice()
    {
        if (instance.choiceCanvas != null)
        {
            instance.choiceCanvas.gameObject.SetActive(false);
            instance.StartCoroutine(fadeOut(instance.choiceCanvas.GetComponent<CanvasGroup>()));
        }
    }


    static IEnumerator displayPlayerBubble(GameObject panel)
    {
        instance.isListeningPnj = true;
        float t = 0;
        Vector3 finalPosition = instance.upBubble.transform.position;
        Vector3 startPosition = instance.bottomBubble.transform.position;
        instance.garbage.Add(instance.upBubble);
        instance.upBubble.SetActive(false);

        instance.upBubble = instance.bottomBubble;

        while (t < instance.timePopBubble)
        {
           
            instance.upBubble.transform.position = Vector3.Lerp(startPosition, finalPosition, t/instance.timePopBubble);
            t += Time.deltaTime;
            yield return null;
        }
        instance.bottomBubble = panel;
        Instantiate(panel);
        yield return null;
        if((instance.c1 != null && instance.c1.response != null) || (instance.c2 != null && instance.c2.response != null))
        {
            t = 0;
            panel = SpeechFactory.getDownPNJPanel();
            panel.GetComponent<UnityEngine.UI.Text>().text = (instance.c1 == null) ? instance.c2.response : instance.c1.response;
            finalPosition = instance.upBubble.transform.position;
            startPosition = instance.bottomBubble.transform.position;
            instance.garbage.Add(instance.upBubble);
            instance.upBubble.SetActive(false);

            instance.upBubble = instance.bottomBubble;

            while (t < instance.timePopBubble*2)
            {

                instance.upBubble.transform.position = Vector3.Lerp(startPosition, finalPosition, t / instance.timePopBubble);
                t += Time.deltaTime;
                yield return null;
            }
            instance.bottomBubble = panel;
            Instantiate(panel);
        }
        instance.isListeningPnj = true;
        continueConversation(instance.currentName);
        yield return null;
    }


    static IEnumerator fadeIn(CanvasGroup g)
    {
        float t = 0;

        while (t < instance.time)
        {
            g.alpha = Mathf.Lerp(0, 1, t / instance.time);
            t += Time.deltaTime;
            yield return null;
        }
        //Prevent player from inputing during fading
        yield return null;
    }

    static IEnumerator fadeOut(CanvasGroup g)
    {
        float t = 0;
        while (t < instance.time)
        {
            g.alpha = Mathf.Lerp(1, 0, t / instance.time);
            t += Time.deltaTime;
            yield return null;
        }
        //Prevent player from inputing during fading
        yield return null;
    }
}


