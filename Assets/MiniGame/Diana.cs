using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diana : MonoBehaviour
{
    public bool started;
    public float startDelay;
    public float lifeTime;
    float maxLifeTime;
    public Vector3 direction;
    public float speed;
    Rigidbody rb;
    public MiniGameManager manager;
    Vector3 startPos;

    public ParticleSystem destroyPs;
    public ParticleSystem disapearPs;

    bool hitted;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        hitted = false;
        startPos = transform.position;
        maxLifeTime = lifeTime;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0.0f && !hitted)
        {
            StartCoroutine("Disapear");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && !hitted)
        {
            manager.points++;
            StartCoroutine("Destroy");
        }
    }

    IEnumerator Destroy()
    {
        hitted = true;
        destroyPs.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        manager.dianasDone++;
        yield return new WaitForSeconds(1.0f);
        hitted = false;
        transform.position = startPos;
        lifeTime = maxLifeTime;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.SetActive(false);
    }

    IEnumerator Disapear()
    {
        hitted = true;
        disapearPs.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        manager.dianasDone++;
        yield return new WaitForSeconds(1.0f);
        hitted = false;
        transform.position = startPos;
        lifeTime = maxLifeTime;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.SetActive(false);
    }
}
