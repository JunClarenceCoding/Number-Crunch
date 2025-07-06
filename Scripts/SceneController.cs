using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject uiManagerPrefab;
    public GameObject startUpHandlerPrefab;
    // public GameObject logoutControllerPrefab;

    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            Instantiate(uiManagerPrefab);
        }
        
        if (StartUpHandler.Instance == null)
        {
            Instantiate(startUpHandlerPrefab);
        }

        // if (FindObjectOfType<LogoutController>() == null)
        // {
        //     Instantiate(logoutControllerPrefab);
        // }
    }
}
