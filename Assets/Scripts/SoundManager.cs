using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private const float DefaultVolume = 1.0f;
    private const float DefaultPitch = 1.0f;
    
    void Start() {
        if(Instance != null) {
            Debug.LogWarning("Multiple instances of SoundManager detected!");
            return;
        }
        Instance = this;
    }
    
    public AudioSource Play(AudioClip clip, Transform emitter) {
        return Play(clip, emitter, DefaultVolume, DefaultPitch);
    }
    
    public AudioSource Play(AudioClip clip, Transform emitter, float volume) {
        return Play(clip, emitter, volume, DefaultPitch);
    }
    
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch) {
        GameObject obj = new GameObject("Audio: " + clip.name);
        obj.transform.position = emitter.position;
        obj.transform.parent = emitter;
        return MakeAudioSource(obj, clip, volume, pitch);
    }

    public AudioSource Play(AudioClip clip, Vector3 point) {
        return Play(clip, point, DefaultVolume, DefaultPitch);
    }
    
    public AudioSource Play(AudioClip clip, Vector3 point, float volume) {
        return Play(clip, point, volume, DefaultPitch);
    }
    
    public AudioSource Play(AudioClip clip, Vector3 point, float volume, float pitch) {
        GameObject obj = new GameObject("Audio: " + clip.name);
        obj.transform.position = point;
        return MakeAudioSource(obj, clip, volume, pitch);
    }
    
    private AudioSource MakeAudioSource(GameObject obj, AudioClip clip, float volume, float pitch) {
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        Destroy(obj, clip.length);
        return source;
    }
}