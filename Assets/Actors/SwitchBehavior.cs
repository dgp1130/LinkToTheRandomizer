#nullable enable

using System;
using UnityEngine;
using UnityEngine.Events;

/** Behavior for a switch which flips between two states when struck. */
[RequireComponent(typeof(SpriteRenderer))]
public sealed class SwitchBehavior : MonoBehaviour
{
    [SerializeField] private bool switchEnabled = false;
    [SerializeField] private Sprite disabledSprite = null!;
    [SerializeField] private Sprite enabledSprite = null!;
    [SerializeField] private UnityEvent<bool> switched = null!;
    private new SpriteRenderer renderer = null!;
    private DefenseBehavior hurtBox = null!;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = getSprite(switchEnabled);

        hurtBox = transform.Find("HurtBox").gameObject.GetComponent<DefenseBehavior>();
        hurtBox.Hit += onHit;
    }

    private void OnDestory() => hurtBox.Hit -= onHit;

    private void onHit(object sender, EventArgs args)
    {
        switchEnabled = !switchEnabled;
        renderer.sprite = getSprite(switchEnabled);

        switched?.Invoke(switchEnabled);
    }

    private Sprite getSprite(bool enabled)
    {
        return enabled switch
        {
            false => disabledSprite,
            true => enabledSprite,
        };
    }
}
