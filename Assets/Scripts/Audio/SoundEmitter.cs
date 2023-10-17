using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioConfigurationSO musicChannelRef;
    public bool isMusic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioCue(AudioClip clip, AudioConfigurationSO audioConfig, bool isLoop, Vector3 position = default)
    {
        audioConfig.Apply(audioSource);
        audioSource.loop = isLoop;
        audioSource.clip = clip;
        audioSource.transform.position = position;
        audioSource.time = 0f;
        if (audioConfig == musicChannelRef)
            isMusic = true;
        else
            isMusic = false;

        audioSource.Play();
    }

    public bool isFree()
    {
        if (audioSource.isPlaying)
            return false;

        isMusic = false;
        return true;
    }
    

}
