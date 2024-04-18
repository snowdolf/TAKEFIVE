using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;

    // 게임 스테이지와 카드 배치에 필요한 변수들 선언
    int stage = 1;
    int xLength = 4;
    int yLength = 4;
    float distance = 1.4f;
    float positionX = -2.1f;
    float positionY = -3.0f;
    float frontScale = 1.0f;

    void Start()
    {
        // StageManager에서 현재 스테이지 정보를 가져와서 설정
        stage = StageManager.Instance.stage;

        // 스테이지에 따라 다른 카드 프리팹 선택 및 변수 설정
        if (stage == 1)
        {
            card = card1;
        }
        else if (stage == 2)
        {
            card = card2;
            yLength = 5;
            distance = 1.25f;
            positionX = -1.87f;
            positionY = -3.75f;
            frontScale = 0.8f;
        }
        else if (stage == 3)
        {
            card = card3;
            xLength = 6;
            yLength = 6;
            distance = 0.95f;
            positionX = -2.37f;
            positionY = -3.3f;
            frontScale = 0.6f;
        }

        // 카드 배열 초기화
        int[] arr = new int[xLength * yLength];

        // 카드 매칭을 위해 숫자 쌍 생성
        for (int i = 0; i < arr.Length / 2; i++)
        {
            arr[2 * i] = i;
            arr[2 * i + 1] = i;
        }

        // 카드 배열을 무작위로 섞음
        arr = arr.OrderBy(x => Random.Range(0, arr.Length)).ToArray();

        // 카드 생성과 배치를 위한 코루틴 호출
        StartCoroutine(DistributeCards(arr));
    }

    // 카드를 점진적으로 배치하는 코루틴 메서드
    IEnumerator DistributeCards(int[] arr)
    {
        GameObject[] cards = new GameObject[arr.Length];

        // 카드 생성과 초기 위치 설정
        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, this.transform);
            go.transform.position = Vector2.zero;
            cards[i] = go;
            go.GetComponent<Card>().Setting(arr[i], frontScale);
        }

        // 일정 시간 동안 카드를 초기 위치에 유지
        yield return new WaitForSeconds(1.0f);

        // 각 카드를 목표 위치로 점진적으로 이동시키는 보간 처리
        for (int i = 0; i < cards.Length; i++)
        {
            float targetX = (i % xLength) * distance + positionX;
            float targetY = (i / xLength) * distance + positionY;
            Vector2 targetPosition = new Vector2(targetX, targetY);

            float duration = 0.1f; // 카드 이동에 걸리는 시간
            float elapsed = 0f;
            Vector2 startPosition = Vector2.zero;

            while (elapsed < duration)
            {
                // 카드를 보간하여 점진적으로 목표 위치로 이동
                cards[i].transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }

            // 마지막 위치로 보정하여 정확하게 배치
            cards[i].transform.position = targetPosition;
        }

        // 게임 매니저에 카드 개수 정보 전달
        GameManager.Instance.cardCount = cards.Length;
        GameManager.Instance.cardNum = cards.Length;
    }
}