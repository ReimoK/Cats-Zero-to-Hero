using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float baseMoveSpeed = 3f;
    [HideInInspector] public float currentMoveSpeed;

    public float rotationOffset = 0f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isBound = false;
    private bool isSlowed = false;

    void Start()
    {
        currentMoveSpeed = baseMoveSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
    }

    void FixedUpdate()
    {
        if (isBound)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Transform target = player;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Trap") && hit.GetComponent<MouseTrap>().hasCheese)
            {
                target = hit.transform;
                break;
            }
        }

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            if (rb != null)
            {
                rb.linearVelocity = direction * currentMoveSpeed;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, currentMoveSpeed * Time.fixedDeltaTime);
            }

            if (anim != null)
            {
                anim.SetFloat("InputX", direction.x);
                anim.SetFloat("InputY", direction.y);
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + rotationOffset));
        }
    }


    public void ApplyBind(float duration)
    {
        if (!isBound) StartCoroutine(BindRoutine(duration));
    }

    private IEnumerator BindRoutine(float duration)
    {
        isBound = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;

        yield return new WaitForSeconds(duration);

        isBound = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ApplySlow(float amount, float duration)
    {
        if (!isSlowed) StartCoroutine(SlowRoutine(amount, duration));
    }

    private IEnumerator SlowRoutine(float percentage, float duration)
    {
        isSlowed = true;
        currentMoveSpeed = baseMoveSpeed * percentage;
        GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 1f);

        yield return new WaitForSeconds(duration);

        currentMoveSpeed = baseMoveSpeed;
        isSlowed = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}