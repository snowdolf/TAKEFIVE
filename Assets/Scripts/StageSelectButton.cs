using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    public void StageSelect()
    {
        SceneManager.LoadScene("MainScene");
    }
}
