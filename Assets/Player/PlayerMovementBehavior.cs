using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehavior : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 moveVec = Vector2.zero;
    [SerializeField] float speed = 1.0f;

    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.MovePosition(body.position + (moveVec * Time.deltaTime * speed));
    }

    public void OnMove(InputValue input)
    {
        moveVec = input.Get<Vector2>();
    }
}
