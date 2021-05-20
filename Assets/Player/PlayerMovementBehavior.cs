using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehavior : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator animator;
    private Vector2 moveVec = Vector2.zero;
    [SerializeField] float speed = 1.0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + (moveVec * Time.deltaTime * speed));
        animate();
    }

    private void OnMove(InputValue input)
    {
        moveVec = input.Get<Vector2>();
    }

    private void animate()
    {
        if (moveVec.y > 0) animator.SetTrigger("Walk Up");
        else if (moveVec.y < 0) animator.SetTrigger("Walk Down");
        else if (moveVec.x < 0) animator.SetTrigger("Walk Left");
        else if (moveVec.x > 0) animator.SetTrigger("Walk Right");
        else animator.SetTrigger("Stop");
    }
}
