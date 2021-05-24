#nullable enable

using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TitleStart : MonoBehaviour
{
    /** Called when the user presses any key on the title screen. */
    private void OnStartGame()
    {
        SceneManager.LoadScene("Overworld");
    }
}
