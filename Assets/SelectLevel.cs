using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public void GoToLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
