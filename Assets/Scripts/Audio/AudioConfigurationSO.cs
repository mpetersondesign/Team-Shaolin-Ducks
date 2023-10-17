using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio/AudioConfiguration")]
public class AudioConfigurationSO : ScriptableObject
{
    public AudioMixerGroup mixerGroup = null;

    [Range(0, 1)] public float volume = 1.0f;
    [Range(0.5f, 2)] public float pitch = 1.0f;
    public bool enablePitchVariation = false;
    [Range(0, 0.2f)] public float pitchVariation = 0.0f;
    [Range(0, 1)] public float spatialBlend = 0.0f;

    //for audio falloff
    public FalloffMethod rolloff = 0;
    public float rolloffMin = 1;
    public float rolloffMax = 500;

    
    public void Apply(AudioSource source)
    {
        source.outputAudioMixerGroup = mixerGroup;
        source.volume = volume;
        source.spatialBlend = spatialBlend;

        source.minDistance = rolloffMin;
        source.maxDistance = rolloffMax;

        if (enablePitchVariation)
        {
            source.pitch = Random.Range(pitch - pitchVariation, pitch + pitchVariation);
        }
        else
        {
            source.pitch = pitch;
        }

        switch(rolloff)
        {
            case FalloffMethod.Logarithmic:
                source.rolloffMode = AudioRolloffMode.Logarithmic;
                break;
            case FalloffMethod.Linear:
                source.rolloffMode = AudioRolloffMode.Linear;
                break;
            case FalloffMethod.Custom:
                source.rolloffMode = AudioRolloffMode.Custom;
                break;
        }
    }

    public enum FalloffMethod
    {
        Logarithmic,
        Linear,
        Custom
    }
}

