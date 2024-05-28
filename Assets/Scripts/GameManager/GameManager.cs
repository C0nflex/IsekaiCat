using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] public GameObject arrow;
    public cameraFollowObject _cameraFollowObject;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip GamemusicClip;
    [SerializeField] public GameObject EnemiesSpawnPrefab;
    [SerializeField] public GameObject CurrentEnemies;
    public Material Flash;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");

                return;
            }

            Debug.Log("successfully started LootLocker session");
        });
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        //EventManager.OnTimerStart();
        audioSource.Play();
        
    }

    public void PlayMusicClip(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


    public void PlayMusic1()
    {
        PlayMusicClip(musicClip);
    }

    public void PlayMusic2()
    {
        PlayMusicClip(GamemusicClip);
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
