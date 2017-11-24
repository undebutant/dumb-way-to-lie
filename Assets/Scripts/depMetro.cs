using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depMetro : MonoBehaviour {

    private GameObject player;
    private Vector3 originePos;
    [SerializeField]
    private float distAParcour=100;
    private Vector3 currentPos;
    [SerializeField]
    private float vitesse=10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(roule());

    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        originePos = this.transform.position;
        currentPos = originePos;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator roule()
    {
        SideScrolling.ChangeTarget(this.gameObject);
        player.SetActive(false);
        while(currentPos.x<originePos.x+distAParcour)
        {
            currentPos = this.transform.position;
            currentPos.x += vitesse*Time.deltaTime;
            this.transform.position = currentPos;
            Debug.Log(currentPos.x);
            yield return null;
        }
        player.SetActive(true);
        player.transform.position = new Vector3(currentPos.x, player.transform.position.y, player.transform.position.z);
        yield return new WaitForSecondsRealtime(3);
        this.transform.position = originePos;
        yield return null;
    }

}
