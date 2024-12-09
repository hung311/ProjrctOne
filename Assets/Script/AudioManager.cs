using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Soutce ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip backGround;
    public AudioClip death;
    public AudioClip attack;
    public AudioClip kill;
    public AudioClip walk;
    public AudioClip backGroundMenu;
    public AudioClip button;

    private void Start()
    {
        musicSource.clip = backGroundMenu;
        musicSource.Play();
    }

    public void PlaySFXOneShot(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
