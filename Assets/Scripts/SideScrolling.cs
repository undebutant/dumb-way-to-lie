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




    [SerializeField]
    bool test = false;

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

        //TODO: Hum... C'est pas très charlie.
        if (test)
        {
            FocusOnPlayer();
            zoomOnTarget();
            test = !test;
        }
        
	}

    public void zoomOnTarget()
    {
        StartCoroutine(zoom());
        //StartCoroutine("changeFocus", player);
    }


    public void FocusOnPlayer()
    {
        StartCoroutine(changeFocus(player));
        //StartCoroutine("changeFocus", player);
    }


    public void ChangeTarget(GameObject g)
    {
        StartCoroutine(changeFocus(g));
    }

    IEnumerator changeFocus(GameObject newTarget)
    {
        Vector3 temp = this.transform.position;
        Vector3 tmpPos = target.transform.position;
        target = newTarget;
        isLerping = true;
        float t = 0;
        while (t < time)
        {
            temp.x = Mathf.Lerp(tmpPos.x, newTarget.transform.position.x, t / time);
            temp.y = Mathf.Lerp(tmpPos.y, newTarget.transform.position.y, t / time);
            this.transform.position = temp;
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        isLerping = false;
        
    }

    IEnumerator zoom()
    {
        Camera c = this.GetComponent<Camera>();
        isZooming = true;
        bool b = (c.orthographicSize < defaultFOV);
        float t = 0;
        while (t < time)
        {
            c.orthographicSize = (b)
                ?Mathf.Lerp(zoomFOV,defaultFOV, t / time)
                :Mathf.Lerp(defaultFOV, zoomFOV, t / time);

            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        isZooming = false;

    }

}
