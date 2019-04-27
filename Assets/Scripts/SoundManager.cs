using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    public AudioSource music;
    public AudioSource sfx;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic(bool on) {
        if (on) {
            music.volume = 0.8f;
        } else {
            music.volume = 0;
        }
    }

    public bool GetMusicToggle() {
        if (music.volume == 0)
            return false;
        return true;
    }

    public void ToggleSfx(bool on) {
        if (on) {
            sfx.volume = 0.9f;
        } else {
            sfx.volume = 0;
        }
    }

    public bool GetSfxToggle() {
        if (sfx.volume == 0)
            return false;
        return true;
    }

    public void PlaySfx(AudioClip clip, bool randPitch = true) {
        if (randPitch) {
            sfx.pitch = Random.Range(0.95f, 1.05f);
        } else {
            sfx.pitch = 1;
        }
        sfx.PlayOneShot(clip);
    }
}
