using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{


    [SerializeField] private GameObject emitterPrefab;
    private List<SoundEmitter> emitterPool;
    [SerializeField] private int poolSize = 10;

    [Header("Channels")]
    [SerializeField] private AudioEventChannelSO SFX_channel;
    [SerializeField] private AudioEventChannelSO Music_channel;

    public AudioCue audioCue;

    private void Awake()
    {
        // destroys other copies of AudioManager if multiple have been instatiated
        GameObject[] objects = GameObject.FindGameObjectsWithTag("AudioManager");
        if (objects.Length > 1)
        {
            Destroy(gameObject);
        }

        //initialize sound emitters into the pool
        emitterPool = new List<SoundEmitter>();
        for (int i = 0; i < poolSize; ++i)
        {
            emitterPool.Add(Instantiate(emitterPrefab, transform).GetComponent<SoundEmitter>());
        }

        // will probably be used later
        //SceneManager.activeSceneChanged += OnSceneChange;
    }


    private void OnEnable()
    {
        SFX_channel.OnAudioCueRequestPlay += PlayAudioCue;
        Music_channel.OnAudioCueRequestPlay += PlayAudioCue;
    }



    // Start is called before the first frame update
    void Start()
    {
        audioCue = this.GetComponent<AudioCue>();

        //initialize music and such
        // not needed at the moment
        //OnSceneChange(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO audioConfig, Vector3 position = default, char restart = 'f')
    {
        SoundEmitter emitter;
        //get emitter to play cue on
        if ((emitter = GetEmitter()) != null)
        {
            emitter.PlayAudioCue(audioCue.GetClip(restart), audioConfig, audioCue.isLooping, position);
            Debug.Log("playing audio cue");
        }
        else
        {
            Debug.Log("No emitter available for audio cue");
        }
    }

    // gets emitter, returns null if no free emitters can be found
    SoundEmitter GetEmitter()
    {
        SoundEmitter retval = null;
        for (int i = 0; i < poolSize; ++i)
        {
            if (emitterPool[i].isFree())
            {
                retval = emitterPool[i];
                break;
            }
        }

        return retval;
    }

    // Fades Music that is currently playing (fades ALL currently playing tracks)
    public void FadeMusic(float duration, float target, float start)
    {
        List<AudioSource> music = new List<AudioSource>();

        foreach (SoundEmitter e in emitterPool)
        {
            if (e.isMusic)
            {
                music.Add(e.GetComponent<AudioSource>());
            }
        }

        if (music.Count == 0)
            return;

        foreach(AudioSource source in music)
            StartCoroutine(Fade(source, duration, target, start));

    }

    // Generic Fade function for a given audio source, can fade in or out
    IEnumerator Fade(AudioSource source, float duration, float target = 0.0f, float start = 1.0f)
    {
        float current = 0;
        source.volume = start;
        while (current < duration)
        {
            current += Time.deltaTime;
            source.volume = Mathf.Lerp(start, target, current / duration);
            yield return null;
        }

        if (target == 0.0f)
        {
            source.GetComponent<SoundEmitter>().isMusic = false;
            source.Stop();
        }
        yield break;

    }

    // used for handling music changing with different scenes
    private void OnSceneChange(Scene current, Scene next)
    {
        //find music that is currently playing and fade it out
        FadeMusic(0.2f, 0, 1);
        //then, play music depending on the new scene
        if (next.name == "MainMenu")
        {
            //commented until music is implemented
            //audioCue.PlayAudioCue(0);
        }
        else if (next.name == "Level" || next.name == "Game")
        {
            //commented until music is implemented
            //audioCue.PlayAudioCue(1);
        }
    }

    // for debugging music changes
    IEnumerator WaitForMusic()
    {
        bool stillPlaying = true;
        //check if there is still any music being faded out
        while (stillPlaying)
        {
            stillPlaying = false;
            foreach (SoundEmitter e in emitterPool)
            {
                if (e.isMusic && e.GetComponent<AudioSource>().volume > 0)
                    stillPlaying = true;
            }
            Debug.Log("sp: " + stillPlaying);
            yield return null;
        }
        yield break;
    }
}
