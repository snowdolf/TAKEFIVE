using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2Button : MonoBehaviour
{
    public void Stage2()
    {
        GameManager.stage = 2;
        SceneManager.LoadScene("MainScene");
    }
}