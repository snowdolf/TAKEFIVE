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
    public GameObject namePanel;
    public GameObject endPanel;
    public GameObject failTxt;
    public GameObject successTxt;

    AudioSource audioSource;
    public AudioClip clip;
    public AudioClip failSound;


    // 5�� ī��Ʈ ���� ���� �� Text ������Ʈ
    public GameObject SecondsTxt;
    public Text secondsTxt;
    public float AfterSecondsTxt = 5;

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


        //ù��° ī�尡 ������ ī��Ʈ Text ���
        if (firstCard != null)
        {
            SecondsTxt.SetActive(true);
            AfterSecondsTxt -= Time.deltaTime;
            secondsTxt.text = AfterSecondsTxt.ToString("N1");

            //ù��° ī�尡 ���� �� 5�ʵ��� �ι�° ī�带 �������� ������ ù��° ī�� ����
            if(AfterSecondsTxt <= 0)
            {
                firstCard.CloseCard();
                firstCard = null;
                AfterSecondsTxt = 5;
                SecondsTxt.SetActive(false);
            }
        }
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

        //��ġ�� �����ϰų� �����ϸ� SecoundsTxt�� �ݴ´�.
        AfterSecondsTxt = 5;
        SecondsTxt.SetActive(false);
    }

    void EndGame()
    {
        Time.timeScale = 0.0f;

        // ���� = 2 * ���� ī�� + ���� �ð� - (��Ī �õ��� Ƚ�� / 5)
        score = 2 * (cardNum - cardCount) + (30 - (int)time) - count / 5; 

        countTxt.text = count.ToString() + "��";
        scoreTxt.text = score.ToString() + "��";

        board.SetActive(false);
        namePanel.SetActive(false);
        endPanel.SetActive(true);
    }

    void FailMatch()
    {
        namePanel.SetActive(true);
        failTxt.SetActive(true);
        Invoke("FailMatchInvoke", 0.8f);
    }

    void FailMatchInvoke()
    {
        namePanel.SetActive(false);
        failTxt.SetActive(false);
        audioSource.PlayOneShot(failSound);
    }

    void SuccessMatch()
    {
        namePanel.SetActive(true);
        nameTxt.text = firstCard.nickname;
        successTxt.SetActive(true);
        Invoke("SuccessMatchInvoke", 0.8f);
    }

    void SuccessMatchInvoke()
    {
        namePanel.SetActive(false);
        successTxt.SetActive(false);
    }
}
