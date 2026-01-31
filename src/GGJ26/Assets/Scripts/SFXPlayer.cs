using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    AudioSource _sfxSource;

    public bool IsPlaying => _sfxSource.isPlaying;

    private void Start()
    {
        _sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }

    public void Stop()
    {
        _sfxSource.Stop();
    }
}
