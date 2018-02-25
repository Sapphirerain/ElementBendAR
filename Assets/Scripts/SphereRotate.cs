using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotate : MonoBehaviour {

    public float rotationSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(gameObject.transform.forward * (rotationSpeed * Time.deltaTime));
    }
}
