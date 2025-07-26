using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

///
/// Uploads WAV bytes to an API and streams the MP3 reply.
/// Starts playback as soon as data is available.
///

public class ApiAudioHandler : MonoBehaviour
{
    #region Inspector
    [Header("Config")]
    public ApiAudioConfig config;
    public AudioSource playSource;
    #endregion

    #region State
    public bool IsBusy { get; private set; }
    #endregion

    #region Public API
    public void UploadWavData(byte[] wavBytes)
    {
        if (IsBusy) { Debug.Log("Uploader busy."); return; }
        if (config == null) { Debug.LogError("Missing config."); return; }

        StartCoroutine(UploadAndStreamPlay(wavBytes));
    }
    #endregion

    #region Coroutine
    private IEnumerator UploadAndStreamPlay(byte[] wavBytes)
    {
        IsBusy = true;

        var form = new WWWForm();
        form.AddBinaryData(config.apiField, wavBytes, "input.wav", "audio/wav");

        var req = UnityWebRequest.Post(config.apiUrl, form);
        req.SetRequestHeader("X-API-KEY", config.apiKey);
        req.timeout = 0;

        var dhAudio = new DownloadHandlerAudioClip(config.apiUrl, AudioType.MPEG) { streamAudio = true };
        req.downloadHandler = dhAudio;

        req.SendWebRequest();
        yield return null;

        var playbackStarted = false;

        while (!req.isDone)
        {
            if (!playbackStarted)
            {
                AudioClip clip = null;
                try { clip = dhAudio.audioClip; } catch { }

                if (clip != null && clip.loadState == AudioDataLoadState.Loaded)
                {
                    playSource.clip = clip;
                    playSource.Play();
                    playbackStarted = true;
                }
            }
            yield return null;
        }

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError("API error: " + req.error);
        else if (!playbackStarted && dhAudio.audioClip != null)
        {
            playSource.clip = dhAudio.audioClip;
            playSource.Play();
        }

        IsBusy = false;
    }
    #endregion
}