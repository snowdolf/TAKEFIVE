using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public string nickname = "������";

    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;
    public SpriteRenderer backImage;

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
        switch(idx)
        {
            case 0:
                nickname = "�̿�";
                break;
            case 1:
                nickname = "����";
                break;
            case 2:
                nickname = "����2";
                break;
            case 3:
                nickname = "����3";
                break;
            case 4:
                nickname = "��Ÿ";
                break;
            case 5:
                nickname = "���";
                break;
            case 6:
                nickname = "���2";
                break;
            case 7:
                nickname = "�����";
                break;
            case 8:
                nickname = "����";
                break;
            case 9:
                nickname = "����";
                break;
            case 10:
                nickname = "����";
                break;
            case 11:
                nickname = "����";
                break;
            case 12:
                nickname = "�μ�";
                break;
            case 13:
                nickname = "���α�";
                break;
            case 14:
                nickname = "�μ�2";
                break;
            case 15:
                nickname = "����2";
                break;
            case 16:
                nickname = "����";
                break;
            case 17:
                nickname = "�׸�";
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

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
        }

        Invoke("OpenCardInvoke", 0.1f);
    }

    public void OpenCardInvoke()
    {
        front.SetActive(true);
        back.SetActive(false);
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.7f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.7f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);

        var renderer = back.GetComponent<SpriteRenderer>();
        renderer.color = UnityEngine.Color.gray;
    }
}
