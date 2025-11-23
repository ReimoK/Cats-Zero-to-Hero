using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Yarn Ball (Bind)")]
    public bool hasYarn = false;
    public float bindDuration = 2f;

    [Header("Milk (Slow)")]
    public bool hasMilk = false;
    public float slowAmount = 0.5f;
    public float slowDuration = 3f;

    [Header("Mouse Trap")]
    public bool hasTraps = false;
    public bool hasCheeseUpgrade = false;
    public GameObject trapPrefab;
    public float trapCooldown = 5f;
    private float nextTrapTime;

    void Update()
    {
        if (hasTraps && Time.time >= nextTrapTime)
        {
            DropTrap();
            nextTrapTime = Time.time + trapCooldown;
        }
    }

    void DropTrap()
    {
        if (trapPrefab == null) return;

        Vector2 randomPos = Random.insideUnitCircle * 3f;
        Vector3 spawnPos = transform.position + (Vector3)randomPos;

        GameObject trap = Instantiate(trapPrefab, spawnPos, Quaternion.identity);

        MouseTrap trapScript = trap.GetComponent<MouseTrap>();
        if (trapScript != null)
        {
            trapScript.hasCheese = hasCheeseUpgrade;
            if (hasCheeseUpgrade)
            {
                trap.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }
}