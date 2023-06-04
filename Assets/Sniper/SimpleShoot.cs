using System.Collections;
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
    public AudioClip Ping;

    bool canShoot;
    public GameObject bolt;
    Vector3 boltStartPos;
    public Vector3 boltFinalPos;
    

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

        canShoot = true;

        //socketInteractor.onSelectEntered.AddListener(AddMagazine);
        //socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

    public void PullTheTrigger()
    {
        if (magazine && magazine.numberOfBullets > 0)
        {
            if (canShoot)
            {
                boltStartPos = bolt.transform.localPosition;
                StartCoroutine("ShootCooldown");
                audioSource.clip = shotSound;
                audioSource.Play();
            }
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
        //CasingRelease();
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {

        audioSource.clip = Ping;
        audioSource.Play();

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

    IEnumerator ShootCooldown()
    {
        Shoot();
        canShoot = false;
        StartCoroutine("MoveBolt");
        yield return new WaitForSeconds(0.5f);
        CasingRelease();
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    IEnumerator MoveBolt()
    {
        yield return new WaitForSeconds(0.1f);
        while (bolt.transform.localPosition.x < boltFinalPos.x)
        {
            bolt.transform.localPosition += new Vector3((boltFinalPos.x - boltStartPos.x) / 24.0f, 0, 0);
            yield return null;
        }
        while (bolt.transform.localPosition.x > boltStartPos.x)
        {
            bolt.transform.localPosition += new Vector3((boltStartPos.x - boltFinalPos.x) / 30.0f, 0, 0);
            yield return null;
        }
    }
}
