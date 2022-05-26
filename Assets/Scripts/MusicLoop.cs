using UnityEngine;
using System.Collections;


public class MusicLoop : MonoBehaviour
{
    [SerializeField] private AudioSource musicStartSource;
    [SerializeField] private AudioSource musicLoopSource;
    void Start()
    {
        StartCoroutine(playMusic());
    }

    IEnumerator playMusic()
    {
        musicStartSource.loop = false;
        musicStartSource.Play();
        yield return new WaitForSeconds(musicStartSource.clip.length);
        musicLoopSource.loop = true;
        musicLoopSource.Play();
    }
}