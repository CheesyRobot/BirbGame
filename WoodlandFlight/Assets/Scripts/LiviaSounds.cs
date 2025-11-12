using UnityEngine;
using System.Collections.Generic;

public class LiviaSounds : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private List<AudioClip> walkClips;
    [SerializeField] private List<AudioClip> flapClips;
    /*void Awake()
    {
        
    }*/

    // Update is called once per frame
    public void PlayWalkSound()
    {
        //if (!source.isPlaying)
        //{
            //source.clip = audioClips[Random.Range(0, 0)];
            source.PlayOneShot(walkClips[Random.Range(0,walkClips.Count)]);
            //source.Play();
        //}
    }

    public void PlayFlyingSound()
    {
            //source.clip = audioClips[flapClips[Random.Range(0, flapClips.Count)]];
            source.PlayOneShot(flapClips[Random.Range(0, flapClips.Count)]);
            //source.Play();
    }
}
