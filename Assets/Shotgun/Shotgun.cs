using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Net.Sockets;

public class Shotgun : MonoBehaviour
{
    Animator animator;
    public InputActionReference buttonA;

    bool onHand = false;
    bool open = true;
    bool delaying = false;

    float lastVerticalAngle = 0;

    // shot
    public int pellets;
    public GameObject pellet;
    ShotgunProjectile p1 = null;
    ShotgunProjectile p2 = null;
    public Transform spawn1;
    public Transform spawn2;
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public GameObject visualCartucho1;
    public GameObject visualCartucho2;

    public Material usedCartuchosMat;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine("CheckMovement");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab()
    {
        onHand = true;

        StartCoroutine("CheckMovement");
    }

    public void Release()
    {
        onHand = false;
    }

    public void Shoot()
    {
        if (p1 && !p1.empty)
        {
            for (int i = 0; i < pellets; i++)
            {
                Transform aux = spawn1;
                aux.Rotate(new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)));
                Instantiate(pellet, aux.position, aux.rotation);
            }
            p1.empty = true;
        }
        else if (p2 && !p2.empty)
        {
            for (int i = 0; i < pellets; i++)
            {
                Transform aux = spawn2;
                aux.Rotate(new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)));
                Instantiate(pellet, aux.position, aux.rotation);
            }
            p2.empty = true;
        }
        else
        {
            //no ammo sound
        }
    }

    public void ReloadBullet(bool one)
    {
        GameObject go;
        if (one)
        {
            go = socket1.GetOldestInteractableSelected().transform.gameObject;
            p1 = go.GetComponent<ShotgunProjectile>();

            go.GetComponent<MeshRenderer>().enabled = false;
            visualCartucho1.SetActive(true);
        }
        else
        {
            go = socket2.GetOldestInteractableSelected().transform.gameObject;
            p2 = go.GetComponent<ShotgunProjectile>();

            go.GetComponent<MeshRenderer>().enabled = false;
            visualCartucho2.SetActive(true);
        }
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(0.5f);
        open = false;
        delaying = false;
    }

    IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(0.5f);
        open = true;
        delaying = false;

        if (p1 != null && p1.empty)
        {
            p1.GetComponent<MeshRenderer>().enabled = true;
            p1.GetComponent<MeshRenderer>().material = usedCartuchosMat;
            p1.GetComponent<XRGrabInteractable>().enabled = false;
            p1 = null;
            visualCartucho1.SetActive(false);
            // expulsar cartucho
            StartCoroutine("ExpulseP1");
        }
        if (p2 != null && p2.empty)
        {
            p2.GetComponent<MeshRenderer>().enabled = true;
            p2.GetComponent<MeshRenderer>().material = usedCartuchosMat;
            p2.GetComponent<XRGrabInteractable>().enabled = false;
            p2 = null;
            visualCartucho2.SetActive(false);
            // expulsar cartucho
            StartCoroutine("ExpulseP2");
        }
    }

    IEnumerator CheckMovement()
    {
        if (!onHand) yield return null;
        else
        {
            if (buttonA.action.phase == InputActionPhase.Performed)
            {
                if (!open && transform.rotation.eulerAngles.x - lastVerticalAngle < -175.0f) // move down
                {
                    animator.ResetTrigger("Close");
                    animator.SetTrigger("Open");
                    StartCoroutine("OpenDelay");
                    delaying = true;
                }
                else if (open && transform.rotation.eulerAngles.x - lastVerticalAngle > 175.0f) // move up
                {
                    animator.ResetTrigger("Open");
                    animator.SetTrigger("Close");
                    StartCoroutine("CloseDelay");
                    delaying = true;
                }
            }

            lastVerticalAngle = transform.rotation.eulerAngles.x;

            yield return new WaitForSeconds(0.1f);
            StartCoroutine("CheckMovement");
        }
    }

    IEnumerator ExpulseP1()
    {
        socket1.enabled = false;
        if (p2 == null) socket2.enabled = false;
        yield return new WaitForSeconds(0.5f);
        socket1.enabled = true;
        socket2.enabled = true;
    }

    IEnumerator ExpulseP2()
    {
        socket2.enabled = false;
        if (p1 == null) socket1.enabled = false;
        yield return new WaitForSeconds(0.5f);
        socket2.enabled = true;
        socket1.enabled = true;
    }
}