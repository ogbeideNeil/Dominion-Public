using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageResponse
{
    [SerializeField]
    private AnimatedBar healthBar;

    [SerializeField]
    private AnimatedBar manaBar;

    private Damageable damageable;

    private void Awake()
    {
        damageable = this.GetComponentAssertNotNull<Damageable>();
    }

    private void Update()
    {
        if (CombatHandler.instance.isDodging)
        {
            StartCoroutine(Disable());
        }

        if (!EnemyDirector.AnyAlertedEnemies() && damageable.Health < damageable.MaxHealth)
        {
            damageable.Heal(10f * Time.deltaTime);
            healthBar.SetFillAmount(damageable.Health / damageable.MaxHealth);
        }
    }

    public void OnDamage(GameObject damageSource)
    {
        if (healthBar.FillAmount == 0) return;

        float newFillPercent = damageable.Health / damageable.MaxHealth;
        if (newFillPercent <= 0)
        {
            newFillPercent = 0;
            Die();
        }
        else if (damageSource.tag == "Golem")
        {
            AnimatorStateInfo anim = damageSource.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            gameObject.GetComponent<Animator>().Play("Knockdown");
        }
        else if (damageSource.tag == "Rock")
        {
            gameObject.GetComponent<Animator>().Play("Knockback");
        }
        else if (damageSource.tag == "Treant")
        {
            gameObject.GetComponent<Animator>().Play("Hit");
        }
        Debug.Log(damageSource);
        healthBar.SetFillAmount(newFillPercent);
    }

    private void Die()
    {
        gameObject.GetComponent<Animator>().Play("KnockdownDie");
        gameObject.GetComponent<CharacterRootMotionController>().enabled = false;
        enabled = false;

        StartCoroutine(RestartGame());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Disable()
    {
        gameObject.GetComponent<CharacterController>().detectCollisions = true;
        gameObject.GetComponent<CharacterController>().detectCollisions = false;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<CharacterController>().detectCollisions = true;
    }
}
