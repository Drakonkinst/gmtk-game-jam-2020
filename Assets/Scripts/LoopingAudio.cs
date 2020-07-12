using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudio : MonoBehaviour
{
    public string audioName;
    public AudioClip[] tracks;
    public float volume;
    
    private AudioSource currentTrack = null;
    private int currentIndex = -1;
    private Transform myTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        if(tracks == null || tracks.Length == 0) {
            Debug.LogWarning("No tracks found!");
            return;
        }
        
        myTransform = transform;
        GameObject obj = new GameObject("Looping Audio: " + audioName);
        obj.transform.position = myTransform.position;
        obj.transform.parent = myTransform;
        currentTrack = obj.AddComponent<AudioSource>();
        currentTrack.volume = volume;
        //currentTrack.loop = true;
        
        PlayNextTrack();
    }
    
    private void PlayNextTrack() {
        AudioClip clip = ChooseNewTrack();
        Debug.Log("Playing " + clip.name);
        currentTrack.clip = clip;
        currentTrack.Play();
        Invoke("PlayNextTrack", clip.length);
    }
    
    private AudioClip ChooseNewTrack() {
        if(tracks.Length <= 1) {
            return currentTrack.clip;
        }
        int newIndex;
        do {
            newIndex = Random.Range(0, tracks.Length);
        } while(newIndex == currentIndex);
        return tracks[newIndex];
    }
}
