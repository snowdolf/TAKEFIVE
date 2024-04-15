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
    public Text scoreTxt;
    public GameObject endTxt;
    public GameObject resultTxt;
    public GameObject failTxt;
    public GameObject successTxt;

    AudioSource audioSource;
    public AudioClip clip;

    public int cardCount = 0;
    float time = 0.0f;
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
        score++;

        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0) EndGame();
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
        endTxt.SetActive(true);
        scoreTxt.text = score.ToString() + "¹ø";
        resultTxt.SetActive(true);
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
