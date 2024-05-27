using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource SoundSource;
    [SerializeField]
    private AudioSource LoopSource;
    [SerializeField]
    private AudioClip OutOfGameMusic;
    [SerializeField]
    private AudioClip InGameMusic;
    [SerializeField]
    private AudioClip buttonClickedSound;
    [SerializeField]
    private AudioClip sharkBiteSound;
    [SerializeField]
    private AudioClip gameOverSound;
    [SerializeField]
    private AudioClip BubbleSound;
    [SerializeField]
    private AudioClip UnderwaterSound;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("multiple PlayerMovement components");
    }

    public void PlaySharkBite() => SoundSource.PlayOneShot(sharkBiteSound);

    public void PlayButtonClicked() => SoundSource.PlayOneShot(buttonClickedSound);

    public void playSceneTransition() => SoundSource.PlayOneShot(BubbleSound, 1);

    public void PlayGameOver() => SoundSource.PlayOneShot(gameOverSound);

    public void Bubblesound() => LoopSource.PlayOneShot(BubbleSound,0.1f);

    public void UnderWaterSound() => LoopSource.PlayOneShot(UnderwaterSound, 0.2f);

    public void PlayOutOfGameMusic()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(OutOfGameMusic);
    }

    public void PlayInGameMusic()
    {
        musicSource.Stop();
        musicSource.PlayOneShot(InGameMusic);
    }
    public void ChangeMusicSourcePitch(float amount)
    {
        musicSource.pitch = amount + 3 * (1f - amount) / 4f;
    }

    public void StopMusic() => musicSource.Stop();
}
