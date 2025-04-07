using UnityEngine;

public class ApplicationExit : MonoBehaviour
{
    // Call this method to quit the application
    public void QuitApplication()
    {
        // This works in standalone builds
        Application.Quit();
        
        // This message will appear in the Unity Editor console since Application.Quit() 
        // doesn't work in the Editor
        Debug.Log("Application has quit");
        
        // This will stop play mode in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}