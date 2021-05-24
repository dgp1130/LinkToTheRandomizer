#nullable enable

using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartGameBehavior : MonoBehaviour
{
    /** Starts the game by loading the overworld. */
    public void StartGame()
    {
        SceneManager.LoadScene("Overworld");
    }
}
