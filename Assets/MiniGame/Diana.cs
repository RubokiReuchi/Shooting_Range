using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : MonoBehaviour
{
    public bool started;
    public float startDelay;
    public float lifeTime;
    public Vector3 direction;
    public float speed;
    Rigidbody rb;
    public MiniGameManager manager;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        startPos = transform.position;

        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0.0f)
        {
            manager.dianasDone++;
            transform.position = startPos;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            manager.points++;
            manager.dianasDone++;
            transform.position = startPos;
            gameObject.SetActive(false);
        }
    }
}
