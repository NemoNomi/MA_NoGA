using UnityEngine;

///
/// When the GameObject that owns this component becomes active,
/// it disables another GameObject.  
///

public class DeactivateOnActivation : MonoBehaviour
{
    [Tooltip("This object will be turned OFF when the trigger object turns ON.")]
    [SerializeField] private GameObject objectToDeactivate;


    private void OnEnable()
    {
        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(false);
    }

}
