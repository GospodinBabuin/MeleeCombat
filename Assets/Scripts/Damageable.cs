using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public int maxHitPoints;
    public int currentHitPoints;

    public bool isInvulnerable = false;
    private float _invulnerabiltyTime = 0.35f;

    public UnityEvent OnTakeHit;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    public void ApplyDamage(int damage)
    {
        if (currentHitPoints <= 0)
        {
            return;
        }

        if (isInvulnerable)
        {
            return;
        }

        OnTakeHit.Invoke();
        currentHitPoints -= damage;
        StartCoroutine(BecomeInvalnuravle());
    }

    private IEnumerator BecomeInvalnuravle()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerabiltyTime);
        isInvulnerable = false;
    }
}
