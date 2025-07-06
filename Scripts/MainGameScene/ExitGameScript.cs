using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    public void ExitGame()
    {
        #if UNITY_ANDROID || UNITY_IOS
            Application.Quit(); 
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}