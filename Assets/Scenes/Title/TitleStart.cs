#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TitleStart : MonoBehaviour
{
    private IEnumerator autoStarter = null!;

    private void Awake()
    {
        autoStarter = autoStart();
        StartCoroutine(autoStarter);
    }

    /** Automatically start the game in 5 seconds. */
    private IEnumerator autoStart()
    {
        yield return new WaitForSeconds(5);
        startGame();
    }

    /** Called when the user presses any key on the title screen. */
    private void OnStartGame()
    {
        StopCoroutine(autoStarter);
        startGame();
    }

    private void startGame() => SceneManager.LoadScene("Instructions");
}
