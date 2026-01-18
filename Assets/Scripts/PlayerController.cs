using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [HideInInspector] public bool isMoving = false;
    private Vector2 movementInput;

    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    [Header("Skins System")]
    public RuntimeAnimatorController[] catAnimators;

    public Sprite[] catStaticSprites;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        int skinID = PlayerPrefs.GetInt("EquippedSkin", 0);

        SetupSkin(skinID);

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        if (sr != null)
        {
            playerWidth = sr.bounds.size.x / 2;
            playerHeight = sr.bounds.size.y / 2;
        }
    }

    void SetupSkin(int id)
    {
        if (id < catAnimators.Length && catAnimators[id] != null)
        {
            anim.enabled = true;
            anim.runtimeAnimatorController = catAnimators[id];
        }
        else
        {
            anim.enabled = false;
            if (id < catStaticSprites.Length && catStaticSprites[id] != null)
            {
                sr.sprite = catStaticSprites[id];
            }
        }
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput.Normalize();

        isMoving = movementInput.magnitude > 0.01f;

        if (anim.enabled)
        {
            UpdateAnimations();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = isMoving ? movementInput * moveSpeed : Vector2.zero;
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
        anim.SetFloat("Speed", isMoving ? 1f : 0f);
        anim.SetFloat("Horizontal", movementInput.x);
        anim.SetFloat("Vertical", movementInput.y);
    }
}