#nullable enable

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public sealed class TriforceBehavior : MonoBehaviour
{
    [SerializeField] private TMP_Text victoryText = null!;
    [SerializeField] private PlayerMovementBehavior player = null!;
    private SpriteRenderer sprite = null!;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Ignore collisions with anything that's not the player.
        if (collider.gameObject.tag != "Player") return;

        victoryText.gameObject.SetActive(true);
        StartCoroutine(endGame());
        sprite.enabled = false; // Hide the Triforce to simulate the player picking it up.
    }

    /** End the game after a few seconds by loading the title screen. */
    private IEnumerator endGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }
}
