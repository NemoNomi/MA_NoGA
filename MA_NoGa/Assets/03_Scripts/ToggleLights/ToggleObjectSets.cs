using UnityEngine;
using System.Collections.Generic;

public class ToggleObjectSets : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSet
    {
        public List<GameObject> objects;
    }

    public List<ObjectSet> sets = new List<ObjectSet>();

    private int currentSetIndex = -1;

    public void ActivateNextSet()
    {
        foreach (var set in sets)
        {
            foreach (var obj in set.objects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        currentSetIndex++;
        if (currentSetIndex >= sets.Count)
            currentSetIndex = 0;

        foreach (var obj in sets[currentSetIndex].objects)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
