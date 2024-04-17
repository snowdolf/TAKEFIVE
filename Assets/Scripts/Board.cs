using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    GameObject card;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;

    // lv1 = 4 * 4 -> 8개. lv2 = 5 * 4 -> 10개. lv2 = 6 * 6 -> 18개
    int stage = 1;
    int xLength = 4;
    int yLength = 4;
    float distance = 1.4f;

    // 왼쪽 아래 구석의 카드 위치 조정
    float positionX = -2.1f;
    float positionY = -3.0f;

    // 카드 이미지의 스케일을 Card.cs에 넘겨줘서 수정
    float frontScale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        stage = StageManager.Instance.stage;

        if(stage == 1)
        {
            // card1의 scale = 1.3f
            card = card1;
        }
        else if (stage == 2)
        {
            // card2의 scale = 1.15f
            card = card2;
            yLength = 5;
            distance = 1.25f;
            positionX = -1.87f;
            positionY = -3.75f;
            frontScale = 0.8f;
        }
        else if (stage == 3)
        {
            // card3의 scale = 0.85f
            card = card3;
            xLength = 6; yLength = 6;
            distance = 0.95f;
            positionX = -2.37f;
            positionY = -3.3f;
            frontScale = 0.6f;
        }

        // arr = {0, 0 , 1, 1, ..., 7, 7, 0, 0, 1, 1, ...}
        int[] arr = new int[xLength * yLength];
        for(int i = 0; i < arr.Length / 2; i++)
        {
            arr[2 * i] = i % 8;
            arr[2 * i + 1] = i % 8;
        }

        arr = arr.OrderBy(x => Random.Range(0, arr.Length)).ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % xLength) * distance + positionX;
            float y = (i / xLength) * distance + positionY;

            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i], frontScale);
        }

        GameManager.Instance.cardCount = arr.Length;
        GameManager.Instance.cardNum = arr.Length;
    }
}
