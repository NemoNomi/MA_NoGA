using UnityEngine;

///
/// Holds the endpoint URL, API key, and multipart-field name used by
/// ApiAudioHandler when uploading recorded audio.
///

[CreateAssetMenu(
    fileName = "ApiAudioConfig",
    menuName = "Audio Chat/API Audio Config",
    order = 0)]
public class ApiAudioConfig : ScriptableObject
{
    [Header("Server Endpoint")]
    public string apiUrl = "xxx";

    [Header("Auth")]
    public string apiKey = "xxx";

    [Header("Multipart")]
    public string apiField = "xxx";
}
