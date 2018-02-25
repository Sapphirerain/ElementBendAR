using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAndFire : MonoBehaviour {

    [SerializeField]
    public GameObject gun;
    [SerializeField]
    public GameObject barrel;
    [SerializeField]
    public GameObject rotator;
    //[SerializeField]
    public GameObject target;

    public GameObject ArtilleryShot;

    [SerializeField]
    public float fireDelay;
    [SerializeField]
    public float startDelay;
    private float timeLeftUntilFire;

    // Use this for initialization
    void Start () {
        target = GameManager.instance.player;
        timeLeftUntilFire = startDelay;
	}
	
	// Update is called once per frame
	void Update () {
        gun.transform.forward = Vector3.Normalize(barrel.transform.position - target.transform.position);
        timeLeftUntilFire -= Time.deltaTime;
        if (timeLeftUntilFire < 0.0f)
        {
            Instantiate(ArtilleryShot).transform.position = barrel.transform.position;
            timeLeftUntilFire = fireDelay;
        }
    }


}
