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

    // ���� ���������� ī�� ��ġ�� �ʿ��� ������ ����
    int stage = 1;
    int xLength = 4;
    int yLength = 4;
    float distance = 1.4f;
    float positionX = -2.1f;
    float positionY = -3.0f;
    float frontScale = 1.0f;

    void Start()
    {
        // StageManager���� ���� �������� ������ �����ͼ� ����
        stage = StageManager.Instance.stage;

        // ���������� ���� �ٸ� ī�� ������ ���� �� ���� ����
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

        // ī�� �迭 �ʱ�ȭ
        int[] arr = new int[xLength * yLength];

        // ī�� ��Ī�� ���� ���� �� ����
        for (int i = 0; i < arr.Length / 2; i++)
        {
            arr[2 * i] = i;
            arr[2 * i + 1] = i;
        }

        // ī�� �迭�� �������� ����
        arr = arr.OrderBy(x => Random.Range(0, arr.Length)).ToArray();

        // ī�� ������ ��ġ�� ���� �ڷ�ƾ ȣ��
        StartCoroutine(DistributeCards(arr));
    }

    // ī�带 ���������� ��ġ�ϴ� �ڷ�ƾ �޼���
    IEnumerator DistributeCards(int[] arr)
    {
        GameObject[] cards = new GameObject[arr.Length];

        // ī�� ������ �ʱ� ��ġ ����
        for (int i = 0; i < arr.Length; i++)
        {
            GameObject go = Instantiate(card, this.transform);
            go.transform.position = Vector2.zero;
            cards[i] = go;
            go.GetComponent<Card>().Setting(arr[i], frontScale);
        }

        // ���� �ð� ���� ī�带 �ʱ� ��ġ�� ����
        yield return new WaitForSeconds(1.0f);

        // �� ī�带 ��ǥ ��ġ�� ���������� �̵���Ű�� ���� ó��
        for (int i = 0; i < cards.Length; i++)
        {
            float targetX = (i % xLength) * distance + positionX;
            float targetY = (i / xLength) * distance + positionY;
            Vector2 targetPosition = new Vector2(targetX, targetY);

            float duration = 0.1f; // ī�� �̵��� �ɸ��� �ð�
            float elapsed = 0f;
            Vector2 startPosition = Vector2.zero;

            while (elapsed < duration)
            {
                // ī�带 �����Ͽ� ���������� ��ǥ ��ġ�� �̵�
                cards[i].transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null; // ���� �����ӱ��� ���
            }

            // ������ ��ġ�� �����Ͽ� ��Ȯ�ϰ� ��ġ
            cards[i].transform.position = targetPosition;
        }

        // ���� �Ŵ����� ī�� ���� ���� ����
        GameManager.Instance.cardCount = cards.Length;
        GameManager.Instance.cardNum = cards.Length;
    }
}