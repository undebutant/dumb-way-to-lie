using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour {

    public static SideScrolling instance;

   

    [SerializeField] [Tooltip("Cible de la caméra")]
    GameObject target;

    [SerializeField][Tooltip("Joueur à suivre")]
    GameObject player;

    [SerializeField]
    [Tooltip("Temps pour changer de focus")]
    float time = 2;




    [SerializeField]
    float defaultFOV = 5f;

    [SerializeField]
    float zoomFOV = 2.5f;



    private static bool isLerping = false;
    private static bool isZooming = false;
    private static Vector3 tmp;




    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public static float getZoomDuration()
    {
        return instance.time;
    }

    // Use this for initialization
    void Start () {
        tmp = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLerping)
        {
            tmp = this.transform.position;
            tmp.x = target.transform.position.x;
            this.transform.position = tmp;
        }
        
	}

    public static void zoomOnTarget()
    {
        instance.StartCoroutine(zoom());
        
        //StartCoroutine("changeFocus", player);
    }

    public static void FocusOnPlayer()
    {
        instance.StartCoroutine(changeFocus(instance.player));
        //StartCoroutine("changeFocus", player);
    }


    public static void ChangeTarget(GameObject g)
    {
        instance.StartCoroutine(changeFocus(g));
    }

    static IEnumerator changeFocus(GameObject newTarget)
    {
        Vector3 temp = instance.transform.position;
        Vector3 tmpPos = instance.target.transform.position;
        instance.target = newTarget;
        isLerping = true;
        float t = 0;
        while (t < instance.time)
        {
            temp.x = Mathf.Lerp(tmpPos.x, newTarget.transform.position.x, t / instance.time);
            temp.y = Mathf.Lerp(tmpPos.y, newTarget.transform.position.y, t / instance.time);
            instance.transform.position = temp;
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        isLerping = false;
        
    }

    static IEnumerator zoom()
    {
        Camera c = instance.GetComponent<Camera>();
        isZooming = true;
        bool b = (c.orthographicSize < instance.defaultFOV);
        float t = 0;
        while (t < instance.time)
        {
            c.orthographicSize = (b)
                ?Mathf.Lerp(instance.zoomFOV, instance.defaultFOV, t / instance.time)
                :Mathf.Lerp(instance.defaultFOV, instance.zoomFOV, t / instance.time);

            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        c.orthographicSize = Mathf.Round(c.orthographicSize);

        isZooming = false;

    }

}
