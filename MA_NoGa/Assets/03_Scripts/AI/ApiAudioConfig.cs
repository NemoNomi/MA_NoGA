using UnityEngine;

/// <summary>
/// Holds the endpoint URL, API key, and multipart-field name used by
/// <c>ApiAudioHandler</c> when uploading recorded audio.
/// </summary>

[CreateAssetMenu(
    fileName = "ApiAudioConfig",
    menuName = "Audio Chat/API Audio Config",
    order    = 0)]
public class ApiAudioConfig : ScriptableObject
{
    [Header("Server Endpoint")]
    public string apiUrl = "xxx";

    [Header("Auth")]
    public string apiKey = "xxx";

    [Header("Multipart")]
    public string apiField = "xxx";
}
