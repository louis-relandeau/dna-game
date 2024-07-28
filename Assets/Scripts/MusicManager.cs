using UnityEngine;

public class MusicManager : MonoBehaviour {
    // Singleton instance
    public static MusicManager Instance;

    // Audio source to play music
    private AudioSource musicSource;
    private AudioSource sfxSource;

    // Assign clips in the Inspector
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    private void Awake() {
        // Implement Singleton pattern
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Create and configure the audio sources
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            sfxSource = gameObject.AddComponent<AudioSource>();
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int clipIndex) {
        if (clipIndex >= 0 && clipIndex < musicClips.Length) {
            musicSource.clip = musicClips[clipIndex];
            musicSource.Play();
        } else {
            Debug.LogError("Music clip index out of range!");
        }
    }

    public void PlaySFX(int clipIndex, float volume = 1) {
        SetSFXVolume(volume);
        if (clipIndex >= 0 && clipIndex < sfxClips.Length) {
            sfxSource.PlayOneShot(sfxClips[clipIndex]);
        } else {
            Debug.LogError("SFX clip index out of range!");
        }
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume) {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume) {
        sfxSource.volume = volume;
    }

    public void SetMusicSpeed(float newSpeed) {
        musicSource.pitch = newSpeed;
    }
}
