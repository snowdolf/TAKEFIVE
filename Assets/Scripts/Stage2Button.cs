using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage2Button : MonoBehaviour
{
    public Button button;

    public void Stage2()
    {
        if (StageManager.Instance.stageUnLocked[2] == true)
        {
            StageManager.Instance.stage = 2;
            SceneManager.LoadScene("MainScene");
        }
    }

    private void Start()
    {
        ColorBlock colorBlock = button.colors;
        if (StageManager.Instance.stageUnLocked[2] == true)
        {
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.selectedColor = Color.white;
        }
        else
        {
            colorBlock.normalColor = Color.black;
            colorBlock.highlightedColor = Color.black;
            colorBlock.selectedColor = Color.black;
        }
        button.colors = colorBlock;
    }
}
