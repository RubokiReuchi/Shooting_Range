using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    Rigidbody rb;
    public int axis;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (axis)
        {
            case 0:
                rb.velocity = transform.up * speed;
                break;
            case 1:
                rb.velocity = transform.forward * speed;
                break;
            case 2:
                rb.velocity = transform.right * speed;
                break;
        }
        
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0.0f) Destroy(gameObject);
    }
}
