using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomLocationSounds : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;
    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 5f;
    public float waitTimeCountdown = -1f;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (waitTimeCountdown < 0f)
        {
            currentClip = audioClips[Random.Range(0, audioClips.Count)];
            source.clip = currentClip;
            //source.Play();
            PlaySoundRandomLocation();
            waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            // StopCoroutine ("FadeOut");
        }
        // else if (waitTimeCountdown < 1f) {
        //     StartCoroutine(FadeOut(source, 1f));
        //     waitTimeCountdown -= Time.deltaTime;
        // }
        else
        {
            waitTimeCountdown -= Time.deltaTime;
        }
    }

        private void PlaySoundRandomLocation() {
            Vector3 position = transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count)], position);
        }
}
