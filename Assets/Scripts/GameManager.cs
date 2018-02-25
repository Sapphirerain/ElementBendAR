using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;

	// Use this for initialization
	void Awake () {
		if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
