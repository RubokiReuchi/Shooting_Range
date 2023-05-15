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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
                    animator.SetTrigger("Close");
                    StartCoroutine("CloseDelay");
                    delaying = true;
                }
            }
            else
            {
                if (buttonA.action.phase == InputActionPhase.Performed)
                {
                    animator.SetTrigger("Open");
                    StartCoroutine("OpenDelay");
                    delaying = true;
                }
            }
        }
    }

    public void Grab()
    {
        onHand = true;
    }

    public void Release()
    {
        onHand = false;
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
    }
}
