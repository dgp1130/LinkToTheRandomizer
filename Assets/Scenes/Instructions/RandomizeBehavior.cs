#nullable enable

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class RandomizeBehavior : MonoBehaviour
{
    private TMP_InputField input = null!;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
        Randomize();
    }

    /** Generates a random seed and puts it in the input box. */
    public void Randomize()
    {
        input.text = ((int) Mathf.Floor(Random.value * Mathf.Pow(2, 16))).ToString();
    }
}
