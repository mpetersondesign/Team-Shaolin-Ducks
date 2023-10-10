using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Audio/AudioCue")]
public class AudioCueSO : ScriptableObject
{
    public bool isLooping = false;
    [SerializeField] private AudioClip[] audioClips;
    public playMethod method = 0;

    private int lastPlayed = -1;
    private int nextToPlay = -1;



    public AudioClip GetClip(char restart)
    {
        switch(method)
        {
            case playMethod.Random:
                nextToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                break;
            case playMethod.RandomNoRepeat:
                while (nextToPlay == lastPlayed)
                    nextToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                break;
            case playMethod.Sequential:
                if (restart == 't')
                    nextToPlay = 0;
                else
                    nextToPlay = ++nextToPlay % audioClips.Length;
                break;
        }

        lastPlayed = nextToPlay;
        //Debug.Log("Playing: " + nextToPlay);
        return audioClips[nextToPlay];
    }

    public enum playMethod
    {
        Random,
        RandomNoRepeat,
        Sequential
    }

}
