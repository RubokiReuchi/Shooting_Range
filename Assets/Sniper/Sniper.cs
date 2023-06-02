using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]

public class Sniper : MonoBehaviour
{

    

    [SerializeField] protected float shootingForce;
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] protected float recoilForce;

    public GameObject bullet;

    [Header("Sounds")]
    AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip ReloadSound;
    public AudioClip noAmmoSound;
    public AudioClip openSound;
    public AudioClip closeSound;

    private Rigidbody rigidBody;
    private XRGrabInteractable interactableWeapon;
    public XRInteractorLineVisual vrLine;

    bool onHand = false;
    bool open = true;
    Collider collider;


    protected virtual void Awake()
    {
        interactableWeapon = GetComponent<XRGrabInteractable>();
        rigidBody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Grab()
    {
        onHand = true;
        collider.isTrigger = true;
        vrLine.enabled = false;

        StartCoroutine("CheckMovement");
    }

    public void Release()
    {
        onHand = false;
        collider.isTrigger = false;
        vrLine.enabled = true;
    }


    public void Shoot()
    {
        //if (p1 && !p1.empty)
        //{
        //    for (int i = 0; i < pellets; i++)
        //    {
        //        Quaternion aux = spawn1.rotation;
        //        aux *= Quaternion.Euler(new Vector3(Random.Range(-dispersion, dispersion), 0, Random.Range(-dispersion, dispersion)));
        //        Instantiate(pellet, spawn1.position, aux);
        //    }
        //    p1.empty = true;

        //    audioSource.clip = shotSound;
        //    audioSource.Play();
        //}
        //else if (p2 && !p2.empty)
        //{
        //    for (int i = 0; i < pellets; i++)
        //    {
        //        Quaternion aux = spawn2.rotation;
        //        aux *= Quaternion.Euler(new Vector3(Random.Range(-dispersion, dispersion), 0, Random.Range(-dispersion, dispersion)));
        //        Instantiate(pellet, spawn2.position, aux);
        //    }
        //    p2.empty = true;

        //    audioSource.clip = shotSound;
        //    audioSource.Play();
        //}
        //else
        //{
        //    audioSource.clip = noAmmoSound;
        //    audioSource.Play();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
