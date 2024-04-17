using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage3Button : MonoBehaviour
{
    public Button button;

    public void Stage3()
    {
        if (StageManager.Instance.stageUnLocked[3] == true)
        {
            StageManager.Instance.stage = 3;
            SceneManager.LoadScene("MainScene");
        }
    }

    private void Start()
    {
        ColorBlock colorBlock = button.colors;
        if (StageManager.Instance.stageUnLocked[3] == true)
        {
            colorBlock.normalColor = Color.white;
        }
        else
        {
            colorBlock.normalColor = Color.black;
        }
        button.colors = colorBlock;
    }
}
