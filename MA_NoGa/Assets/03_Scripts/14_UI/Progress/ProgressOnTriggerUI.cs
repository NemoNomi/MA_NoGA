using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a vertical fill-bar while a collider with triggering tag
/// stays inside this trigger.  
/// </summary>

[RequireComponent(typeof(Collider))]
public class ProgressOnTriggerUI : MonoBehaviour
{
    #region Inspector
    [Header("Trigger")]
    [SerializeField] private string triggeringTag = "PlayerHand";
    [SerializeField] private float holdTime = 5f;

    [Header("UI Image (Filled ▸ Vertical)")]
    [SerializeField] private Image progressImage;
    #endregion

    #region State
    private float timer;
    private bool inside;
    #endregion

    #region Trigger Events
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(triggeringTag)) inside = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(triggeringTag)) inside = false;
    }
    #endregion

    #region Update Loop
    private void Update()
    {
        timer += (inside ? 1 : -1) * Time.deltaTime;
        timer = Mathf.Clamp(timer, 0f, holdTime);

        if (progressImage)
            progressImage.fillAmount = timer / holdTime;
    }
    #endregion
}
