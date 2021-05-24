#nullable enable

using UnityEngine;

public sealed class CheckBehavior : MonoBehaviour
{
    [SerializeField] Randomization randomization = null!;
    [SerializeField] ItemMapping itemMapping = null!;
    [SerializeField] Check check;

    public void Awake()
    {
        // Find the item randomized to this check location.
        var item = randomization.GetItemForCheck(check);

        // Look up the prefab for the item at this location.
        var itemPrefab = itemMapping.GetPrefab(item);

        // Create a new instance of that prefab at this check's location.
        var transform = GetComponent<Transform>();
        Instantiate(itemPrefab, transform);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
