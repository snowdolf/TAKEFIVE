using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public string nickname = "������";

    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;

    AudioSource audioSource;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setting(int number, float frontScale)
    {
        idx = number;
        switch(idx / 2)
        {
            case 0:
                nickname = "�赵��";
                break;
            case 1:
                nickname = "�����";
                break;
            case 2:
                nickname = "�ֹμ�";
                break;
            case 3:
                nickname = "�̹̿�";
                break;
            case 4:
                nickname = "õ����";
                break;
            default:
                break;
        }
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
        front.transform.localScale = new Vector3(frontScale, frontScale, 1f);
    }

    public void OpenCard()
    {
        if (GameManager.Instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);
        Invoke("OpenCardInvoke", 0.1f);

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
        }
    }

    public void OpenCardInvoke()
    {
        front.SetActive(true);
        back.SetActive(false);
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.8f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.8f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
