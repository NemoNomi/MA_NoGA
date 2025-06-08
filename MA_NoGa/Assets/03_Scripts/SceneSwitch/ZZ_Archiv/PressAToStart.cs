using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class VRPressAToStart : MonoBehaviour
{
    [Tooltip("Build-Index der Zielszene.")]
    public int sceneIndex = 1;

    [Tooltip("Welcher Controller abgefragt wird (meist RightHand).")]
    public XRNode controllerNode = XRNode.RightHand;

    bool loading;

    void Update()
    {
        if (loading) return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid) return;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed) && pressed)
        {
            loading = true;
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
