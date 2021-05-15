using UnityEngine;
using DevelWithoutACause.Randomizer;

public class ItemBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log(Randomizer.Randomize());
            Destroy(gameObject);
        }
    }
}
