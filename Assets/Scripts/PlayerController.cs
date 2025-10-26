using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;

    [HideInInspector] public bool isMoving = false;
    private Vector2 movementInput;

    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (rb == null)
        {
            Debug.LogError("PlayerController requires a Rigidbody2D component.");
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            playerWidth = sr.bounds.size.x / 2;
            playerHeight = sr.bounds.size.y / 2;
        }
        else
        {
            playerWidth = 0.5f;
            playerHeight = 0.5f;
        }
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput.Normalize();

        isMoving = movementInput.magnitude > 0.01f;

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;

        viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + playerWidth, screenBounds.x - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        transform.position = viewPos;
    }

    private void UpdateAnimations()
    {
        if (anim == null) return;

        if (!isMoving)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        anim.SetFloat("Speed", 1f);
        anim.SetFloat("Horizontal", movementInput.x);
        anim.SetFloat("Vertical", movementInput.y);
    }
}