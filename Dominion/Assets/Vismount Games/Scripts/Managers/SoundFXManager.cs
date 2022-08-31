using System.Linq;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] attackSounds;

    [SerializeField]
    private Sound[] bigAttackSounds;

    [SerializeField]
    private Sound[] airAttackSounds;

    [SerializeField]
    private Sound[] bigSwingSounds;

    [SerializeField]
    private Sound[] jumpSounds;

    [SerializeField]
    private Sound[] stepSounds;

    [SerializeField]
    private Sound[] skillSounds;

    [SerializeField]
    private Sound[] startSounds;

    [SerializeField]
    private Sound[] endSounds;

    [SerializeField]
    private Sound[] getHitSound;

    public void Atk()
    {
        if (!attackSounds.Any())
        {
            Debug.Log("SoundFxManager -> No attack sounds loaded");
            return;
        }

        int index = Random.Range(0, attackSounds.Length);
        SoundManager.PlaySound(attackSounds[index], transform.position);
    }

    public void BigAtk()
    {
        if (!bigAttackSounds.Any())
        {
            Debug.Log("SoundFxManager -> No bigAttack sounds loaded");
            return;
        }

        int index = Random.Range(0, bigAttackSounds.Length);
        SoundManager.PlaySound(bigAttackSounds[index], transform.position);
    }

    public void airAtk()
    {
        if (!airAttackSounds.Any())
        {
            Debug.Log("SoundFxManager -> No airAttack sounds loaded");
            return;
        }

        int index = Random.Range(0, airAttackSounds.Length);
        SoundManager.PlaySound(airAttackSounds[index], transform.position);
    }

    public void SkillAtk()
    {
        if (!skillSounds.Any())
        {
            Debug.Log("SoundFxManager -> No skillAttack sounds loaded");
            return;
        }

        int index = Random.Range(0, skillSounds.Length);
        SoundManager.PlaySound(skillSounds[index], transform.position);
    }

    public void CombatStart()
    {
        if (!startSounds.Any())
        {
            Debug.Log("SoundFxManager -> No start sounds loaded");
            return;
        }

        int index = Random.Range(0, startSounds.Length);
        SoundManager.PlaySound(startSounds[index], transform.position);
    }


    public void CombatEnd()
    {
        if (!endSounds.Any())
        {
            Debug.Log("SoundFxManager -> No end sounds loaded");
            return;
        }

        int index = Random.Range(0, endSounds.Length);
        SoundManager.PlaySound(endSounds[index], transform.position);
    }

    public void GetHit ()
    {
        if (!getHitSound.Any())
        {
            Debug.Log("SoundFxManager -> No getHit sounds loaded");
            return;
        }
        int index = Random.Range(0, getHitSound.Length);
        SoundManager.PlaySound(getHitSound[index], transform.position);
    }
    public void BigSwing()
    {
        if (!bigSwingSounds.Any())
        {
            Debug.Log("SoundFxManager -> No bigSwing sounds loaded");
            return;
        }

        int index = Random.Range(0, bigSwingSounds.Length);
        SoundManager.PlaySound(bigSwingSounds[index], transform.position);
    }

    public void Jump()
    {
        if (!jumpSounds.Any())
        {
            Debug.Log("SoundFxManager -> No jump sounds loaded");
            return;
        }

        int index = Random.Range(0, jumpSounds.Length);
        SoundManager.PlaySound(jumpSounds[index], transform.position);
    }

    public void Step()
    {
        if (!stepSounds.Any())
        {
            Debug.Log("SoundFxManager -> No step sounds loaded");
            return;
        }

        int index = Random.Range(0, stepSounds.Length);
        SoundManager.PlaySound(stepSounds[index], transform.position);
    }
}
