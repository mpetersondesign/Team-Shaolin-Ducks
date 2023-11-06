using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioCue : MonoBehaviour
{


    public class Cue
    {
        public Cue(AudioCueSO a, AudioConfigurationSO b)
        {
            cue = a;
            config = b;
        }

        public AudioCueSO cue; 
        public AudioConfigurationSO config; 
    }

    public bool disabled = false;

    [Header("Audio Clips")]
    [SerializeField] AudioCueSO[] audioCues;
    private List<Cue> list = new List<Cue>();
    [Header("Configuration")]
    [SerializeField] AudioConfigurationSO[] audioConfigs;
    [SerializeField] AudioEventChannelSO audioChannel;

    private void Awake()
    {
        if (audioConfigs.Length != audioCues.Length)
        {
            disabled = true;
            //Debug.Log("AudioCue has conflicting numbers of AudioCueSO and AudioConfigurationSO. AudioCue has been disabled.");
            return;
        }


        for (int i = 0; i < audioCues.Length; ++i)
            list.Add(new Cue(audioCues[i], audioConfigs[i]));
    }


    public void PlayAudioCue(int index = 0)
    {
        if (disabled)
            return;

        if (list.Count == 0)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " has list count 0");
            return;
        }    
        
        if (index >= audioCues.Length)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " is out of range index");
            return;
        }
        
        Cue cuegroup = list[index];

        audioChannel.RaisePlayEvent(cuegroup.cue, cuegroup.config, transform.position, 'f');
    }

    //same as above, but this time resets sequential read value to account for missed audio calls
    public void PlayAudioCueNew(int index = 0)
    {
        if (disabled)
            return;

        if (list.Count == 0)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " has list count 0");
            return;
        }

        if (index >= audioCues.Length)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " is out of range index");
            return;
        }

        Cue cuegroup = list[index];

        audioChannel.RaisePlayEvent(cuegroup.cue, cuegroup.config, transform.position, 't');
    }

    public void PlayAudioCuePitched(int index = 0, float pitch = 1.0f, char restart = 'f')
    {
        if (disabled)
            return;

        if (list.Count == 0)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " has list count 0");
            return;
        }

        if (index >= audioCues.Length)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " is out of range index");
            return;
        }

        Cue cuegroup = list[index];

        cuegroup.config.pitch = pitch;

        audioChannel.RaisePlayEvent(cuegroup.cue, cuegroup.config, transform.position, restart);

    }

    public void StartAudioCue(int index = 0, string key = "0")
    {
        if (disabled)
            return;

        if (list.Count == 0)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " has list count 0");
            return;
        }

        if (index >= audioCues.Length)
        {
            //Debug.LogWarning("Play Audio Cue " + index + " is out of range index");
            return;
        }

        Cue cuegroup = list[index];

        audioChannel.RaiseStartEvent(cuegroup.cue, cuegroup.config, transform.position, key);

    }

    public void StopAudioCue(string key = "0", float fadeDuration = 0.2f)
    {
        audioChannel.RaiseStopEvent(key, fadeDuration);
    }
}
