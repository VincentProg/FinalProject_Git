using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public GameObject impactParticle;
    Vector2 director;
    float lastDistance = 1000;

    // Start is called before the first frame update
    void Start()
    {
        director = target.position - transform.position;
        director.Normalize();
        gameObject.transform.eulerAngles = director;

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = director * 200f;
        if(Vector2.Distance(transform.position, target.position) > lastDistance)
        {
            GameObject impact = Instantiate(impactParticle, transform);
            impact.transform.SetParent(null);
            impact.transform.localScale = new Vector3(10, 10, 10);
            
            Destroy(gameObject);
        }
        lastDistance = Vector2.Distance(transform.position, target.position);


    }
}
