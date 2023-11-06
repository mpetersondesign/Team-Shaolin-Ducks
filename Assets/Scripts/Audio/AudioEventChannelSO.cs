using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


[CreateAssetMenu(menuName = "Audio/AudioCue Event Channel")]
public class AudioEventChannelSO : ScriptableObject
{
    public UnityAction<AudioCueSO, AudioConfigurationSO, Vector3, char> OnAudioCueRequestPlay;
    public UnityAction<AudioCueSO, AudioConfigurationSO, Vector3, string> OnAudioCueRequestStart;
    public UnityAction<string,float> OnAudioCueRequestStop;
    public void RaisePlayEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfig, Vector3 position, char restart)
    {
        OnAudioCueRequestPlay?.Invoke(audioCue, audioConfig, position, restart);

        //if (OnAudioCueRequestPlay == null)
            //Debug.Log("Audio Cue request failed.");
    }

    public void RaiseStartEvent(AudioCueSO audioCue, AudioConfigurationSO audioConfig, Vector3 position, string key)
    {
        OnAudioCueRequestStart?.Invoke(audioCue, audioConfig, position, key);

    }

    public void RaiseStopEvent(string key, float fadeDuration)
    {
        OnAudioCueRequestStop?.Invoke(key, fadeDuration);
    }
}
