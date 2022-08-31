using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThrowable : MonoBehaviour
{
    [SerializeField]
    private Transform spawnFrom;

    [SerializeField]
    private Throwable throwable;

    [SerializeField]
    private float detachTime = 2f;

    public void InstantiateThrowable()
    {
        StartCoroutine(StartInstantiating(detachTime));
    }

    private IEnumerator StartInstantiating(float time)
    {
        GameObject spawnedObject = Instantiate(throwable.gameObject, spawnFrom);
        Vector3 scale = spawnedObject.transform.localScale;

        spawnedObject.transform.parent = spawnFrom.root.transform;
        spawnedObject.transform.localScale = scale;
        spawnedObject.transform.parent = spawnFrom;

        yield return new WaitForSeconds(time);
        spawnedObject.transform.parent = null;
        spawnedObject.GetComponent<Throwable>().Throw();

    }
}
