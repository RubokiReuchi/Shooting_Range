using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    Vector3 idlePos;
    public Transform pressedPos;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource audioSource;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isPressed = false;
        idlePos = button.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.position = pressedPos.position;
            presser = other.gameObject;
            onPress.Invoke();
            audioSource.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.position = idlePos;
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
