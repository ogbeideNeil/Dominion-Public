using System.Collections;
using UnityEngine;

/// <summary>
///
/// </summary>
/// <see cref="https://forum.unity.com/threads/throw-an-object-along-a-parabola.158855/"/>
public class Throwable : MonoBehaviour
{
    public void Throw()
    {
        StartCoroutine(StartThrow(EnemyDirector.Player.position, 45));
    }

    IEnumerator StartThrow(Vector3 targetPosition, float firingAngle)
    {
        // Calculate distance to target
        float target_Distance = Vector3.Distance(transform.position, targetPosition);
        firingAngle = 45f;
        float gravity = 45f;

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(targetPosition - transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        GetComponentInChildren<Shockwave>()?.Trigger();
        AttackCollision attackCollision = GetComponentInChildren<AttackCollision>();

        if (attackCollision != null)
        {
            attackCollision.enabled = false;
        }

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
