using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    bool playing;

    public Diana[] dianas;
    float clock;

    public int points;
    int maxPoints;
    public int dianasDone;

    public TextMeshProUGUI pointsText;
    public GameObject recordPoints;
    TextMeshProUGUI recordPointsText;
    public string recordString;

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
        playing = false;
        maxPoints = dianas.Length;

        recordPointsText = recordPoints.GetComponent<TextMeshProUGUI>();
        recordPointsText.text = PlayerPrefs.GetInt(recordString, 0).ToString();

        audioSource = GetComponent<AudioSource>();
        isPressed = false;
        idlePos = button.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = points.ToString();

        if (!playing) return;

        if (dianasDone >= maxPoints)
        {
            Finish();
            return;
        }

        for (int i = 0; i < dianas.Length; i++)
        {
            if (!dianas[i].started && dianas[i].startDelay <= clock)
            {
                dianas[i].manager = this;
                dianas[i].gameObject.SetActive(true);
                dianas[i].started = true;
            }
        }

        clock += Time.deltaTime;
    }

    public void StartMiniGame()
    {
        if (playing) return;

        clock = 0;
        points = 0;
        dianasDone = 0;
        playing = true;
        recordPoints.SetActive(false);
    }

    void Finish()
    {
        playing = false;
        for (int i = 0; i < dianas.Length; i++)
        {
            dianas[i].started = false;
        }

        if (points > PlayerPrefs.GetInt(recordString, 0))
        {
            PlayerPrefs.SetInt(recordString, points);
            recordPointsText.text = points.ToString();
        }

        if (!isPressed) button.transform.position = idlePos;

        recordPoints.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && !playing)
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
            isPressed = false;
        }
    }
}
