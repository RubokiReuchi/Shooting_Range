﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    public Magazine magazine;
    public XRBaseInteractor socketInteractor;
    //private bool hasSlide = true;


    [Header("Sounds")]
    AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip noAmmoSound;
    public AudioClip ReleaseMagazine;
    public AudioClip EnterMagazine;


    public void AddMagazine()
    {
        GameObject go = socketInteractor.GetOldestInteractableSelected().transform.gameObject;
        magazine = go.GetComponent<Magazine>();
        //hasSlide = false;

        audioSource.clip = EnterMagazine;
        audioSource.Play();
    }

    public void RemoveMagazine(/*XRBaseInteractable interactable*/)
    {
        audioSource.clip = ReleaseMagazine;
        audioSource.Play();
        magazine = null;
    }

    //public void Slide()
    //{
    //    hasSlide =  true;
    //}



    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        audioSource = GetComponent<AudioSource>();

        //socketInteractor.onSelectEntered.AddListener(AddMagazine);
        //socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    public void PullTheTrigger()
    {
        if (magazine && magazine.numberOfBullets > 0)
        {
            Shoot();
            audioSource.clip = shotSound;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = noAmmoSound;
            audioSource.Play();
        }

    }


    //This function creates the bullet behavior
    void Shoot()
    {
        //hasSlide = false;
        magazine.numberOfBullets--;

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        CasingRelease();
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
