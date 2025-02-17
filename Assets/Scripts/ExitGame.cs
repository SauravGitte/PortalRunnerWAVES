using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#else
        Application.Quit(); // Quit the built game
#endif
    }
}
