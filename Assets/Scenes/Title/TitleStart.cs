#nullable enable

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class TitleStart : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(autoStart());
    }

    /** Automatically start the game in 5 seconds. */
    private IEnumerator autoStart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Instructions");
    }
}
