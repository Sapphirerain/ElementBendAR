using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShot : MonoBehaviour {

    [SerializeField]
    public GameObject target;

    public float shotSpeed;
    public float lifeTime; 

    //private Vector3 targetDirection;
    private Vector3 targetPosition;
    private Vector3 posPastTarget;

    // Use this for initialization
    void Start () {
        target = GameManager.instance.player;
        targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        posPastTarget = 2 * targetPosition - gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float step = shotSpeed * Time.deltaTime;
        //gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, step);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, posPastTarget, step);
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
