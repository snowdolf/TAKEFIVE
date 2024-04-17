using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Button : MonoBehaviour
{
    public void Stage1()
    {
        StageManager.Instance.stage = 1;
        SceneManager.LoadScene("MainScene");
    }
}
