using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Audio/AudioCue Event Channel")]
public class AudioEventChannelSO : ScriptableObject
{
    public UnityAction<AudioCueSO, AudioConfigurationSO, Vector3, char> OnAudioCueRequestPlay;

    public void RaiseEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfig, Vector3 position, char restart)
    {
        OnAudioCueRequestPlay?.Invoke(audioCue, audioConfig, position, restart);

        //if (OnAudioCueRequestPlay == null)
            //Debug.Log("Audio Cue request failed.");
    }
}
