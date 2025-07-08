using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Put on Button, loads Scene On Click.
/// </summary>
/// 

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class LoadSceneOnClick : MonoBehaviour
{
    #region Inspector
    [SerializeField] private int sceneIndex = 0;
    #endregion

    #region Setup
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Load);
    }
    #endregion

    #region Core
    private void Load()
    {
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
    #endregion
}