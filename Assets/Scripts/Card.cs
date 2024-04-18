using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public string nickname = "°¡³ª´Ù";

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
                nickname = "¹Ì¿µ";
                break;
            case 1:
                nickname = "¿ìÇõ";
                break;
            case 2:
                nickname = "¿ìÇõ2";
                break;
            case 3:
                nickname = "¿ìÇõ3";
                break;
            case 4:
                nickname = "³«Å¸";
                break;
            case 5:
                nickname = "Àçºó";
                break;
            case 6:
                nickname = "Àçºó2";
                break;
            case 7:
                nickname = "´ó´óÀÌ";
                break;
            case 8:
                nickname = "Çüµ·";
                break;
            case 9:
                nickname = "µµÇö";
                break;
            case 10:
                nickname = "·çÇÇ";
                break;
            case 11:
                nickname = "´ö±¸";
                break;
            case 12:
                nickname = "¹Î¼®";
                break;
            case 13:
                nickname = "²¿ºÎ±â";
                break;
            case 14:
                nickname = "¹Î¼®2";
                break;
            case 15:
                nickname = "µµÇö2";
                break;
            case 16:
                nickname = "ÀÎÇü";
                break;
            case 17:
                nickname = "±×¸²";
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
