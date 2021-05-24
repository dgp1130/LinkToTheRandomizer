#nullable enable

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StartGameBehavior : MonoBehaviour
{
    [SerializeField] private Randomization randomization = null!;
    [SerializeField] private TMP_InputField seedInput = null!;
    [SerializeField] private TMP_Text errorText = null!;

    /** Starts the game by loading the overworld. */
    public void StartGame()
    {
        int seed;
        var success = int.TryParse(seedInput.text, out seed);
        if (!success || seed < 0 || seed >= Mathf.Pow(2, 16))
        {
            errorText.gameObject.SetActive(true);
            return;
        }

        randomization.Randomize(seed);
        SceneManager.LoadScene("Overworld");
    }
}
