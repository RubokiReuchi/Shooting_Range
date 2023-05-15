using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        StartCoroutine("CheckMovement");
    }

    // Update is called once per frame
    void Update()
    {
        if (onHand && !delaying)
        {
            if (open)
            {
                if (buttonA.action.phase == InputActionPhase.Performed)
                {
                    
                }
            }
            else
            {
                if (buttonA.action.phase == InputActionPhase.Performed)
                {
                    
                }
            }
        }
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
        if (!p1.empty)
        {
            for (int i = 0; i < pellets; i++)
            {
                Transform aux = spawn1;
                aux.Rotate(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0));
                Instantiate(pellet, aux);
            }
        }
        else if (!p2.empty)
        {
            for (int i = 0; i < pellets; i++)
            {
                Transform aux = spawn2;
                aux.Rotate(new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0));
                Instantiate(pellet, aux);
            }
        }
        else
        {
            //no ammo sound
        }
    }

    public void ReloadBullet(ShotgunProjectile projectile)
    {
        if (one)
        {
            
        }
        else
        {
            p2 = projectile;
        }
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(1.0f);
        open = false;
        delaying = false;
    }

    IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(1.0f);
        open = true;
        delaying = false;

        if (p1 != null && p1.empty)
        {
            p1 = null;
            // expulsar cartucho
        }
        if (p2 != null && p2.empty)
        {
            p2 = null;
            // expulsar cartucho
        }
    }

    IEnumerator CheckMovement()
    {
        if (!onHand) yield return null;
        else
        {
            if (buttonA.action.phase == InputActionPhase.Performed)
            {
                if (transform.rotation.eulerAngles.x - lastVerticalAngle > 75.0f) // move down
                {
                    animator.SetTrigger("Open");
                    StartCoroutine("OpenDelay");
                    delaying = true;
                    Debug.Log("Open");
                }
                else if (transform.rotation.eulerAngles.x - lastVerticalAngle > -75.0f) // move up
                {
                    animator.SetTrigger("Close");
                    StartCoroutine("CloseDelay");
                    delaying = true;
                    Debug.Log("Close");
                }
            }

            lastVerticalAngle = transform.rotation.eulerAngles.x;

            yield return new WaitForSeconds(0.3f);
            StartCoroutine("CheckMovement");
        }
    }
}
