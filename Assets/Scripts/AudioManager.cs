using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        BgmPlay();
    }

    public void BgmPlay()
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void BgmStop()
    {
        audioSource.clip = clip;
        audioSource.Stop();
    }
}
