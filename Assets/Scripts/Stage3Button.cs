using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3Button : MonoBehaviour
{
    public void Stage3()
    {
        GameManager.stage = 3;
        SceneManager.LoadScene("MainScene");
    }
}
