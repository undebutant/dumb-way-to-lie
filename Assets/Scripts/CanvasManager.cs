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

    List<Step>.Enumerator it;

    bool continueDialog = false;

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
        Debug.Log("Display");
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
        instance.isListeningPnj = true;
       
        instance.currentStep = GameManager.getNextValidStep(name);
        Debug.Log(instance.currentStep);

        string textValue = (instance.currentStep != null)
            ? instance.currentStep.textPNJ
            : "J'en ai fini avec vous.";
        GameObject panel;

        if (instance.upBubble == null)
        {
            panel = SpeechFactory.getUpPNJPanel();
            instance.upBubble = Instantiate<GameObject>(panel, instance.choiceCanvas.transform);
            instance.upBubble.GetComponentInChildren<UnityEngine.UI.Text>().text = textValue;
            UnityEngine.UI.Text t = instance.upBubble.GetComponentInChildren<UnityEngine.UI.Text>();
            t.text = textValue;
        }
        else if (instance.bottomBubble == null)
        {

            instance.bottomBubble = Instantiate<GameObject>(SpeechFactory.getDownPNJPanel(), instance.choiceCanvas.transform);
            UnityEngine.UI.Text t = instance.bottomBubble.GetComponentInChildren<UnityEngine.UI.Text>();
            t.text = textValue;
            //instance.bottomBubble.GetComponent<UnityEngine.UI.Text>().text = textValue;
        }
        else
        {
            
            panel = Instantiate<GameObject>(SpeechFactory.getDownPNJPanel(), instance.choiceCanvas.transform);
            UnityEngine.UI.Text t = panel.GetComponentInChildren<UnityEngine.UI.Text>();
            t.text = textValue;
            //instance.continueDialog = true;
            instance.StartCoroutine(displayPNJBubble(panel));
        }

        //List<Choice>.Enumerator it = instance.currentStep.getEnum();

        
        if (instance.currentStep != null)
        {
            instance.c1 = instance.currentStep.getCurrentChoice();
            instance.currentStep.incrementChoice();
            instance.c2 = instance.currentStep.getCurrentChoice();

            instance.firstChoice.text = instance.c1.textButton;
            
            instance.secondChoice.text = instance.c2.textButton; 
        }
        else
        {
            Debug.Log("COUCOU");
            instance.firstChoice.text = "Partir.";
            instance.secondChoice.text = "Partir, vraiment.";
            instance.c2 = null;
            instance.c1 = null;
        }
        instance.isListeningPnj = false;

    }


    public  void makeChoice(int idChoice)
    {
        bool coroutine = false;
        if (!instance.isListeningPnj)
        {
            
            if (instance.c1 == null && instance.c2 == null)
            {
                // instance.StartCoroutine(fadeOut(instance.choiceCanvas.GetComponent<CanvasGroup>())); //TODO Hide? setInactive
                hideChoice();
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
                    instance.upBubble = Instantiate(panel,instance.choiceCanvas.transform);
                    UnityEngine.UI.Text t = instance.upBubble.GetComponentInChildren<UnityEngine.UI.Text>();
                    instance.bottomBubble.SetActive(true);
                    t.text = textValue;
                }
                else if (instance.bottomBubble == null)
                {
                    panel = SpeechFactory.getDownPlayerPanel();
                    instance.bottomBubble = Instantiate(panel, instance.choiceCanvas.transform);
                    instance.bottomBubble.SetActive(true);
                    UnityEngine.UI.Text t = instance.bottomBubble.GetComponentInChildren<UnityEngine.UI.Text>();
                    t.text = textValue;

                }
                else
                {
                    
                    panel = Instantiate(SpeechFactory.getDownPlayerPanel(), instance.choiceCanvas.transform);
                    UnityEngine.UI.Text t = panel.GetComponentInChildren<UnityEngine.UI.Text>();
                    t.text = textValue;
                     coroutine = true;
                    instance.StartCoroutine(displayPlayerBubble(panel));//Coroutine faire monter
                }
                //TODO: UpdateBoolTABLE
            }
            if(!coroutine)
            instance.StartCoroutine(displayPlayerBubble(null));
        }
    }




    public static void hideChoice()
    {
        if (instance.choiceCanvas != null)
        {
            instance.choiceCanvas.gameObject.SetActive(false);
            instance.StartCoroutine(fadeOut(instance.choiceCanvas.GetComponent<CanvasGroup>()));
            GameManager.nextEncounter(instance.currentName);
            
            foreach(GameObject g in instance.garbage)
            {
                Destroy(g);
            }
        }
    }



    static IEnumerator displayPNJBubble(GameObject panel)
    {
        float t = 0;
        Vector3 finalPosition = instance.upBubble.transform.position;
        Vector3 startPosition = instance.bottomBubble.transform.position;
        if (panel != null)
        {
            instance.garbage.Add(instance.upBubble);
            instance.upBubble.SetActive(false);

            instance.upBubble = instance.bottomBubble;

            while (t < instance.timePopBubble*2)
            {

                instance.upBubble.transform.position = Vector3.Lerp(startPosition, finalPosition, t / (instance.timePopBubble*2));
                t += Time.deltaTime;
                yield return null;
            }
            instance.bottomBubble = panel; 
            panel.SetActive(true);
        }
         yield return null;
    }


    static IEnumerator displayPlayerBubble(GameObject panel)
    {
        float t = 0;
        Vector3 finalPosition = instance.upBubble.transform.position;
        Vector3 startPosition = instance.bottomBubble.transform.position;
        instance.isListeningPnj = true;
        if (panel != null)
        {
            instance.garbage.Add(instance.upBubble);
            instance.upBubble.SetActive(false);

            instance.upBubble = instance.bottomBubble;

            while (t < instance.timePopBubble)
            {

                instance.upBubble.transform.position = Vector3.Lerp(startPosition, finalPosition, t / instance.timePopBubble);
                t += Time.deltaTime;
                yield return null;
            }
            instance.bottomBubble = panel;
            panel.SetActive(true);
        }
        yield return null;
        if(!instance.continueDialog && (instance.c1 != null && instance.c1.response != null) || (instance.c2 != null && instance.c2.response != null))
        {
            t = 0;
            panel = Instantiate(SpeechFactory.getDownPNJPanel(), instance.choiceCanvas.transform);
            panel.GetComponentInChildren<UnityEngine.UI.Text>().text = (instance.c1 == null) ? instance.c2.response : instance.c1.response;
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
            panel.SetActive(true);
        }
        instance.isListeningPnj = false;
//if(!instance.continueDialog)
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


