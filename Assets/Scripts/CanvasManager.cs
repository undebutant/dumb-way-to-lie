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
        }
        //TODO get Text from file //GameManager.getDataFrom(idEvent)
    }

    public static void hideChoice()
    {
        if (instance.choiceCanvas != null)
        {
            instance.choiceCanvas.gameObject.SetActive(false);
            instance.StartCoroutine(fadeOut(instance.choiceCanvas.GetComponent<CanvasGroup>()));
        }
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


