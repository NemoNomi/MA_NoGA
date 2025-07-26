using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

///
/// Records microphone input when the user presses a button,
/// stops on the next press, transform to WAV,
/// and passes that to the ApiAudioHandler for uploading and playback.
/// The button label switches between “record” and “stop”.
/// Written by ChatGPT.
///

public class SimpleMicRecorder : MonoBehaviour
{
    #region Audio-Parameter
    private const int SAMPLE_RATE = 16000;
    private const int CHANNELS = 1;
    private const int BUFFER_SEC = 1;
    #endregion

    #region Runtime-State
    private AudioClip clip;
    private string micDevice;
    private bool isRecording;
    private int lastSamplePos;
    private readonly List<float> recorded = new();
    #endregion

    #region Upload
    [Header("Upload")]
    public ApiAudioHandler apiAudioHandler;
    #endregion

    #region UI references & colours
    [Header("UI")]
    public TMP_Text labelTMP;

    [Tooltip("Image you want to tint while recording")]
    public Image targetImage;

    [Tooltip("Tint when NOT recording")]
    public Color idleColour = Color.white;

    [Tooltip("Tint while recording")]
    public Color recordColour = new Color(0.85f, 0.15f, 0.15f);
    #endregion

    #region Button OnClick-Event
    public void ToggleRecording()
    {
        if (isRecording) StopRecording();
        else StartRecording();

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (labelTMP != null)
            labelTMP.text = isRecording ? "stop" : "record";

        EnsureImageReference();
        if (targetImage != null)
            targetImage.color = isRecording ? recordColour : idleColour;
    }
    #endregion

    #region Helper
    private void EnsureImageReference()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }
    #endregion

    #region start and stop recording
    private void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone available");
            return;
        }

        micDevice = Microphone.devices[0];
        clip = Microphone.Start(micDevice, true, BUFFER_SEC, SAMPLE_RATE);
        lastSamplePos = 0;
        recorded.Clear();
        isRecording = true;

        Debug.Log("Recording started...");
    }

    private void StopRecording()
    {
        if (!isRecording)
        {
            Debug.LogWarning("tried to stop but no recording was active.");
            return;
        }


        GrabNewSamples();
        Microphone.End(micDevice);
        isRecording = false;

        if (recorded.Count == 0)
        {
            Debug.LogWarning("No samples recorded.");
            return;
        }

        byte[] wav = EncodeWav(recorded.ToArray(), CHANNELS, SAMPLE_RATE);

        if (apiAudioHandler != null && !apiAudioHandler.IsBusy)
        {
            apiAudioHandler.UploadWavData(wav);
            Debug.Log("Uploading Audio.");
        }
        else
        {
            Debug.LogWarning("Upload skipped (Handler busy or missing).");
        }
    }
    #endregion

    private void Update()
    {
        if (isRecording) GrabNewSamples();
    }

    private void GrabNewSamples()
    {
        int pos = Microphone.GetPosition(micDevice);
        if (pos < 0 || pos == lastSamplePos) return;

        int diff = pos - lastSamplePos;
        if (diff < 0) diff += clip.samples;

        float[] buf = new float[diff * CHANNELS];
        clip.GetData(buf, lastSamplePos);
        recorded.AddRange(buf);

        lastSamplePos = pos;
    }

    #region WAV-Encoder
    private static byte[] EncodeWav(float[] samples, int channels, int rate)
    {
        short[] sData = new short[samples.Length];
        byte[] bytes = new byte[samples.Length * 2];

        for (int i = 0; i < samples.Length; i++)
        {
            sData[i] = (short)(Mathf.Clamp(samples[i], -1, 1) * short.MaxValue);
            BitConverter.GetBytes(sData[i]).CopyTo(bytes, i * 2);
        }

        int byteRate = rate * channels * 2;

        using var mem = new MemoryStream();
        using var bw = new BinaryWriter(mem);

        bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
        bw.Write(36 + bytes.Length);
        bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
        bw.Write(16); bw.Write((short)1);
        bw.Write((short)channels);
        bw.Write(rate); bw.Write(byteRate);
        bw.Write((short)(channels * 2)); bw.Write((short)16);
        bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));
        bw.Write(bytes.Length); bw.Write(bytes);

        return mem.ToArray();
    }
    #endregion
}
