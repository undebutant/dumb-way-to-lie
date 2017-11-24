using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortieBureau : MonoBehaviour {
    private Transform player;
    private Vector3 newPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        newPos.x = player.position.x + 5;
        newPos.y = player.position.y + 3.3f;
        newPos.z = 0;
        player.position = newPos;
        player.localScale = new Vector3(3, 3, 1);

    }


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
