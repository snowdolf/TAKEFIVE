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
    public Card[] cards;        // 카드 정보 저장

    public Text timeTxt;
    public Text nameTxt;
    public Text countTxt;
    public Text scoreTxt;
    public Text stageTxt; // 스테이지 텍스트
    public Text bestTimeTxt; // 최단 시간 텍스트
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


    // 5초 카운트 변수 저장 및 Text 컴포넌트
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

        // 몇 스테이지 인지 StageTxt에 출력
        stageTxt.text = StageManager.Instance.stage.ToString();

        // 데이터를 모두 지우기
        //PlayerPrefs.DeleteAll();

        // 스테이지 별 최단 기록 BestTimeTxt에 출력
        string bestTimekey = "BestTime" + StageManager.Instance.stage.ToString();
        if (PlayerPrefs.HasKey(bestTimekey))
        {
            bestTimeTxt.text = PlayerPrefs.GetFloat(bestTimekey).ToString("N2");
        }
        else
        {
            bestTimeTxt.text = "00.00";
        }

        // 스테이지 별 시간 조정
        // 1스테이지 경고 = 20초, 끝 = 30초
        if(StageManager.Instance.stage == 1)
        {
            warningTime = 20.00f;
            endingTime = 30.00f;
        }
        // 2스테이지 경고 = 50초, 끝 = 60초
        else if (StageManager.Instance.stage == 2)
        {
            warningTime = 50.00f;
            endingTime = 60.00f;
        }
        // 3스테이지 경고 = 80초, 끝 = 90초
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

        // 카드 다 사라지면 끝나게 만들기
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


        //첫번째 카드가 열리면 카운트 Text 출력
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

            //첫번째 카드가 열린 후 5초동안 두번째 카드를 선택하지 않으면 첫번째 카드 닫음
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
        // 예외 처리 (힌트 외 다른 카드를 눌렀을 때 빨간색이 유지되는 이슈)
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


            //time -= 5.0f;  //카드를 매칭하는 경우 시간 감소

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

        //매치에 성공하거나 실패하면 SecoundsTxt를 닫는다.
        AfterSecondsTxt = 5;
        SecondsTxt.SetActive(false);
    }

    void SucessStage()
    {
        // 해금할 수 있는 스테이지가 있다면 해금
        if (StageManager.Instance.stage < 3)
        {
            StageManager.Instance.stageUnLocked[StageManager.Instance.stage + 1] = true;
        }

        // 최단 기록 갱신
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

        // 점수 = 2 * 맞춘 카드 + 남은 시간 - (매칭 시도한 횟수 / 5)
        score = 2 * (cardNum - cardCount) + ((int)endingTime - (int)time) - count / 5;

        countTxt.text = count.ToString() + "번";
        scoreTxt.text = score.ToString() + "점";

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

