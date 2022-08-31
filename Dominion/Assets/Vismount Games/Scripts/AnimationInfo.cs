using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInfo : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    private Animator animator;
    private Vector3 bodyPosition;

    public Vector3 DistanceFromBodyPosition { get; private set; }

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
    }

    public void StoreBodyPosition()
    {
        bodyPosition = meshRenderer.bones[0].position;
    }

    public void UpdateDistanceFromStoredPosition()
    {
        DistanceFromBodyPosition = meshRenderer.bones[0].position - bodyPosition;
    }

    public void UpdatePosition()
    {
        animator.Play("Idle");
        transform.position += DistanceFromBodyPosition;
    }
}
