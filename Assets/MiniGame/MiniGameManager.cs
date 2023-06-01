using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public AudioSource buttonAudio;

    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        maxPoints = dianas.Length;

        recordPointsText = recordPoints.GetComponent<TextMeshProUGUI>();
        recordPointsText.text = PlayerPrefs.GetInt(recordString, 0).ToString();
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
        buttonAudio.Play();
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

        recordPoints.SetActive(true);
    }
}
