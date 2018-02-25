using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : MonoBehaviour {

    [SerializeField]
    public int startHealth;

    private int health;

    [SerializeField]
    private GameObject replacement;

    // Use this for initialization
    void Start () {
        health = startHealth;
	}

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        health--;
        if (health <= 0)
        {
            Instantiate(replacement);
            Destroy(gameObject);
        }

    }
}
