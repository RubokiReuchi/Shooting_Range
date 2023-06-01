using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAmmo : MonoBehaviour
{
    public GameObject prefab;
    public float amount;
    public Transform origin;
    bool picking;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        picking = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Used()
    {
        if (picking) return;
        StartCoroutine("Spawn");
        picking = true;
        audioSource.Play();
    }

    public void Released()
    {
        if (picking) picking = false;
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(prefab, origin.position, origin.rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
