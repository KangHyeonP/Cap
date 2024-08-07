using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float fireDelay = 2.0f;
    public float fireMaximumDelay = 0.15f;
    protected float fireTime = 0;
    protected SpriteRenderer spriteRenderer;
    public Coroutine kinfeCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {

    }

    protected virtual void Update()
    {
        fireTime += Time.deltaTime;

        if (InGameManager.Instance.player.AttackKey)
        {
            AttackDelay();
        }
    }

    protected virtual void AttackDelay()
    {
        if (fireTime >= fireMaximumDelay &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 10.0f + DrugManager.Instance.playerAttackDelay / 6.0f)
        {
            kinfeCoroutine = StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack()
    {
        //InGameManager.Instance.knifeEffect.SetActive(true);
        //yield return new WaitForSeconds(0.1f);
        InGameManager.Instance.knifeEffect.SetActive(true);
        InGameManager.Instance.player.KnifeAttack(true);
        spriteRenderer.enabled = false;
        InGameManager.Instance.player.isAttack = true;
        fireTime = 0;

        yield return new WaitForSeconds(0.3f);

        InGameManager.Instance.knifeEffect.SetActive(false);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;

        //yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.knifeEffect.SetActive(false);
    }

    public void CancleKnife()
    {
        gameObject.SetActive(false);
        InGameManager.Instance.knifeEffect.SetActive(false);
        if (kinfeCoroutine == null) return;

        StopCoroutine(kinfeCoroutine);
        spriteRenderer.enabled = true;
        InGameManager.Instance.player.isAttack = false;
        kinfeCoroutine = null;
    }
}
