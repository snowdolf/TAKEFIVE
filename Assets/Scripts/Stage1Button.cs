using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage1Button : MonoBehaviour
{
    public Button button;

    public void Stage1()
    {
        if (StageManager.Instance.stageUnLocked[1] == true)
        {
            StageManager.Instance.stage = 1;
            SceneManager.LoadScene("MainScene");
        }
    }

    private void Start()
    {
        ColorBlock colorBlock = button.colors;
        if (StageManager.Instance.stageUnLocked[1] == true)
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
