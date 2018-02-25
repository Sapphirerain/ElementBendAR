using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField]
    public int startHealth;

    public TextMesh StatusText;

    private int health;

    public float hitWaitTime;
    private float timeUntilCanHit;

    // Use this for initialization
    void Start()
    {
        health = startHealth;
        StatusText.text = $"Life: " + health;
        timeUntilCanHit = 0.0f;
    }

    private void Update()
    {
        if (timeUntilCanHit > 0.0f)
        {
            timeUntilCanHit -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (timeUntilCanHit <= 0.0f) {
            health--;
            timeUntilCanHit = hitWaitTime;
            StatusText.text = $"Life: " + health;

            if (health <= 0)
            {
                StatusText.text = $"You Lost";

            }
        }
    }

}
