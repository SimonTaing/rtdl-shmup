using UnityEngine;
using System.Collections;


public class MusicLoop : MonoBehaviour
{
    [SerializeField] private AudioSource musicStartSource;
    [SerializeField] private AudioSource musicLoopSource;
    void Start()
    {
        StartCoroutine(playMusic());

        musicStartSource.Play();
        musicStartSource.loop = false;

        musicLoopSource.Stop();
        musicLoopSource.loop = true;

        //Debug.Log(musicStartSource.clip.length);
    }

    IEnumerator playMusic()
    {
        //yield return new WaitForSeconds(musicStartSource.clip.length);
        float startLength = musicStartSource.clip.length;
        yield return new WaitForSeconds(startLength);
        //Debug.Log("Playing loop");
        musicLoopSource.Play();
    }
}