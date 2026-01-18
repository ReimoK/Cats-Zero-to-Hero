using UnityEngine;

public class GoldItem : MonoBehaviour
{
    public int goldValue = 10;
    public float attractDistance = 3f;
    public float attractSpeed = 10f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        if (Vector2.Distance(transform.position, player.position) < attractDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play(AudioManager.SoundType.Pickup_Coin);
            GameManager.Instance.AddGold(goldValue);
            Destroy(gameObject);
        }
    }
}