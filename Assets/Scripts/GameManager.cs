using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card firstCard;
    public Card secondCard;

    public Text timeTxt;
    public Text nameTxt;
    public Text countTxt;
    public Text scoreTxt;
    public GameObject board;
    public GameObject endPanel;
    public GameObject failTxt;
    public GameObject successTxt;

    AudioSource audioSource;
    public AudioClip clip;

    public int cardCount = 0;
    public int cardNum = 0;
    float time = 0.0f;
    int count = 0;
    int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
        board.SetActive(true);
        endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time >= 30.0f) EndGame();
    }

    public void Matched()
    {
        count++;

        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                if(StageManager.Instance.stage < 3)
                {
                    StageManager.Instance.stageUnLocked[StageManager.Instance.stage + 1] = true;
                }
                EndGame();
            }
            else SuccessMatch();
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            FailMatch();
        }

        firstCard = null;
        secondCard = null;
    }

    void EndGame()
    {
        Time.timeScale = 0.0f;

        // 점수 = 2 * 맞춘 카드 + 남은 시간 - (매칭 시도한 횟수 / 5)
        score = 2 * (cardNum - cardCount) + (30 - (int)time) - count / 5; 

        countTxt.text = count.ToString() + "번";
        scoreTxt.text = score.ToString() + "점";

        board.SetActive(false);
        endPanel.SetActive(true);
    }

    void FailMatch()
    {
        failTxt.SetActive(true);
        Invoke("FailMatchInvoke", 0.5f);
    }

    void FailMatchInvoke()
    {
        failTxt.SetActive(false);
    }

    void SuccessMatch()
    {
        nameTxt.text = firstCard.nickname;
        successTxt.SetActive(true);
        Invoke("SuccessMatchInvoke", 0.5f);
    }

    void SuccessMatchInvoke()
    {
        successTxt.SetActive(false);
    }
}
