using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {

    public float maxSize;
    public float growFactor;
    public float waitTime;
    public float thrust;

    public GameObject FireSplash;


    public Rigidbody FireRigidbody;

    // Use this for initialization
    void Start () {
        FireRigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(FireSplash).transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        Destroy(gameObject);
    }


    

    public void Shoot()
    {
        if (FireRigidbody != null)
        {

            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            FireRigidbody.isKinematic = false;
            //FireRigidbody.AddForce(0, 0, thrust, ForceMode.Impulse);
            this.transform.SetParent(null);
            //Vector3 theForce = Camera.main.transform.forward * thrust;
            Vector3 theForce = gazeDirection * thrust;

            FireRigidbody.AddForce(theForce);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
