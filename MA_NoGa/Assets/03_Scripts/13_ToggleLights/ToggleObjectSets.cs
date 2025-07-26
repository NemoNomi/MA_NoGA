using System.Collections.Generic;
using UnityEngine;

///
/// Cycles through GameObject groups, deactivating all
/// current objects and activating the next group each time
/// ActivateNextSet is called.
///

public class ToggleObjectSets : MonoBehaviour
{
    #region Data Types
    [System.Serializable]
    public class ObjectSet
    {
        public List<GameObject> objects = new();
    }
    #endregion

    #region Inspector
    [SerializeField] private List<ObjectSet> sets = new();
    #endregion

    #region State
    private int currentSetIndex = -1;
    #endregion

    #region Public API
    public void ActivateNextSet()
    {
        DeactivateAll();
        currentSetIndex = (currentSetIndex + 1) % sets.Count;
        SetActive(sets[currentSetIndex].objects, true);
    }
    #endregion

    #region Helpers
    private void DeactivateAll()
    {
        foreach (var set in sets) SetActive(set.objects, false);
    }

    private static void SetActive(IEnumerable<GameObject> objs, bool value)
    {
        foreach (var obj in objs)
            if (obj) obj.SetActive(value);
    }
    #endregion
}
