using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip JumpSound, RunSound, ShotSound, BulletClashSound, CoinSound;
    private static AudioSource audioSource;

    void Start()
    {
        JumpSound = Resources.Load<AudioClip>("JumpSound");
        RunSound = Resources.Load<AudioClip>("RunSound");
        ShotSound = Resources.Load<AudioClip>("ShotSound");
        BulletClashSound = Resources.Load<AudioClip>("BulletClashSound");
        CoinSound = Resources.Load<AudioClip>("CoinSound");
        audioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "JumpSound":
                audioSource.PlayOneShot(JumpSound);
                break;
            case "RunSound":
                audioSource.PlayOneShot(RunSound);
                break;
            case "ShotSound":
                audioSource.PlayOneShot(ShotSound);
                break;
            case "BulletClashSound":
                audioSource.PlayOneShot(BulletClashSound);
                break;
            case "CoinSound":
                audioSource.PlayOneShot(CoinSound);
                break;
        }
    }

    public void MainMenuMusic()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;

        }
        else
        {
            AudioListener.volume = 0;
        }

    }


    /// <summary>
    //Below Code is to conrtol dethaudio and ingame music.
    /// </summary>

    public AudioSource levelMusic;
    public AudioSource DeathSound;


    public bool levelSong = true;
    public bool DeathSong = false;

    public void LevelMusicPlay()
    {
        levelSong = true;
        DeathSong = false;
        levelMusic.Play();
    }

    public void DeathSoundPlay()
    {
        if (levelMusic.isPlaying)
            levelSong = false;

        {
            levelMusic.Stop();
        }
        if (!DeathSound.isPlaying && DeathSong == false)
        {
            DeathSound.Play();
            DeathSong = true;
        }
    }
}

