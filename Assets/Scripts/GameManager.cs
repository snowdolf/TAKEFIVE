using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card firstCard;
    public Card secondCard;
    public Card wrongCard;
    public Card[] cards;        // ī�� ���� ����

    public Text timeTxt;
    public Text nameTxt;
    public Text countTxt;
    public Text scoreTxt;
    public Text stageTxt; // �������� �ؽ�Ʈ
    public Text bestTimeTxt; // �ִ� �ð� �ؽ�Ʈ
    public GameObject board;
    public GameObject namePlate_Success; 
    public GameObject namePlate_Failed;
    public GameObject endPanel;
    public GameObject failTxt;
    public GameObject successTxt;

    AudioSource audioSource;
    public AudioClip clip;
    public AudioClip clip1;
    public AudioClip warningClip;

    public bool isWarning = false;

    int wrong = 0;


    // 5�� ī��Ʈ ���� ���� �� Text ������Ʈ
    public GameObject SecondsTxt;
    public Text secondsTxt;
    public float AfterSecondsTxt = 5;

    public int cardCount = 0;
    public int cardNum = 0;
    float time = 0.0f;

    float warningTime = 20.00f;
    float endingTime = 30.00f;

    int count = 0;
    int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
        board.SetActive(true);
        endPanel.SetActive(false);

        // �� �������� ���� StageTxt�� ���
        stageTxt.text = StageManager.Instance.stage.ToString();

        // �����͸� ��� �����
        //PlayerPrefs.DeleteAll();

        // �������� �� �ִ� ��� BestTimeTxt�� ���
        string bestTimekey = "BestTime" + StageManager.Instance.stage.ToString();
        if (PlayerPrefs.HasKey(bestTimekey))
        {
            bestTimeTxt.text = PlayerPrefs.GetFloat(bestTimekey).ToString("N2");
        }
        else
        {
            bestTimeTxt.text = "00.00";
        }

        // �������� �� �ð� ����
        // 1�������� ��� = 20��, �� = 30��
        if(StageManager.Instance.stage == 1)
        {
            warningTime = 20.00f;
            endingTime = 30.00f;
        }
        // 2�������� ��� = 50��, �� = 60��
        else if (StageManager.Instance.stage == 2)
        {
            warningTime = 50.00f;
            endingTime = 60.00f;
        }
        // 3�������� ��� = 80��, �� = 90��
        else if (StageManager.Instance.stage == 3)
        {
            warningTime = 80.00f;
            endingTime = 90.00f;
        }

    }

    void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        

        if (time >= warningTime)
        {
            AudioManager.Instance.BgmStop();
            timeTxt.color = Color.red;
            if(!isWarning)
            {
                audioSource.PlayOneShot(warningClip);
                isWarning = true;
            }
        }

        if (time >= endingTime) EndGame();

        // ī�� �� ������� ������ �����
        bool isNoCard = true;
        foreach (Card card in cards)
        {
            if (card != null)
            {
                isNoCard = false;
                break;
            }
        }
        if (isNoCard)
        {
            SucessStage();

            EndGame();
        }


        //ù��° ī�尡 ������ ī��Ʈ Text ���
        if (firstCard != null)
        {
            SecondsTxt.SetActive(true);
            AfterSecondsTxt -= Time.deltaTime;
            secondsTxt.text = AfterSecondsTxt.ToString("N1");

            if (wrong >= 3)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i] != firstCard && cards[i].idx == firstCard.idx)
                    {
                        wrongCard = cards[i];
                        wrongCard.backImage.color = Color.red;
                    }
                }

            }

            //ù��° ī�尡 ���� �� 5�ʵ��� �ι�° ī�带 �������� ������ ù��° ī�� ����
            if (AfterSecondsTxt <= 0)
            {
                firstCard.CloseCard();
                firstCard = null;
                AfterSecondsTxt = 5;
                SecondsTxt.SetActive(false);

                if (wrongCard != null && wrongCard.backImage != null && wrongCard.backImage.color == Color.red)
                {
                    wrongCard.backImage.color = Color.gray;
                }


                time += 1.0f;
                wrong++;
            }
        }
    }

    public void Matched()
    {
        count++;
        // ���� ó�� (��Ʈ �� �ٸ� ī�带 ������ �� �������� �����Ǵ� �̽�)
        if (wrongCard != null)
        {
            wrongCard.backImage.color = Color.gray;
            wrongCard = null;
        }

        if (firstCard.idx == secondCard.idx)
        {
            wrong = 0;
           

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;


            //time -= 5.0f;  //ī�带 ��Ī�ϴ� ��� �ð� ����

            if (cardCount == 0)
            {
                SucessStage();

                EndGame();
            }
            else SuccessMatch();
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            FailMatch();
            time += 1.0f;
            wrong++;
        }

        firstCard = null;
        secondCard = null;

        //��ġ�� �����ϰų� �����ϸ� SecoundsTxt�� �ݴ´�.
        AfterSecondsTxt = 5;
        SecondsTxt.SetActive(false);
    }

    void SucessStage()
    {
        // �ر��� �� �ִ� ���������� �ִٸ� �ر�
        if (StageManager.Instance.stage < 3)
        {
            StageManager.Instance.stageUnLocked[StageManager.Instance.stage + 1] = true;
        }

        // �ִ� ��� ����
        string bestTimekey = "BestTime" + StageManager.Instance.stage.ToString();
        if (PlayerPrefs.HasKey(bestTimekey))
        {
            float best = PlayerPrefs.GetFloat(bestTimekey);
            if (best > time)
            {
                PlayerPrefs.SetFloat(bestTimekey, time);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(bestTimekey, time);
        }
    }

    void EndGame()
    {
        audioSource.Stop();
        isWarning = false;
        Time.timeScale = 0.0f;

        // ���� = 2 * ���� ī�� + ���� �ð� - (��Ī �õ��� Ƚ�� / 5)
        score = 2 * (cardNum - cardCount) + ((int)endingTime - (int)time) - count / 5;

        countTxt.text = count.ToString() + "��";
        scoreTxt.text = score.ToString() + "��";

        board.SetActive(false);
        endPanel.SetActive(true);        
    }

    void FailMatch()
    {
        namePlate_Failed.SetActive(true);
        failTxt.SetActive(true);
        Invoke("FailMatchInvoke", 0.7f);
    }

    void FailMatchInvoke()
    {
        namePlate_Failed.SetActive(false);
        failTxt.SetActive(false);
        audioSource.PlayOneShot(clip1);
    }

    void SuccessMatch()
    {
        namePlate_Success.SetActive(true);
        nameTxt.text = firstCard.nickname;
        successTxt.SetActive(true);
        Invoke("SuccessMatchInvoke", 0.7f);
    }

    void SuccessMatchInvoke()
    {        
        namePlate_Success.SetActive(false);
        successTxt.SetActive(false);
        audioSource.PlayOneShot(clip);
    }
}

