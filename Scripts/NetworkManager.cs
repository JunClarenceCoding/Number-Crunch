using UnityEngine;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        CheckInternetConnection();
        Application.targetFrameRate = 60;
    }
    private void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            HandleDisconnection();
        }
    }
    private void HandleDisconnection()
    {
        Debug.Log("No internet connection. Returning to StartUpScene...");
        SceneManager.LoadScene("StartupScene");
    }
}
