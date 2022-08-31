using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget;

    private void LateUpdate()
    {
        Vector3 position = followTarget.position;
        position.y = transform.position.y;
        transform.position = position;
    }
}
