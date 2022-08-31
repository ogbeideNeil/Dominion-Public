using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMinimap : MonoBehaviour
{
    [SerializeField]
    private RectTransform mapTransform;

    [SerializeField]
    private RectTransform playerIcon;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Vector2 mapReferencePoint1;

    [SerializeField]
    private Vector2 mapReferencePoint2;

    [SerializeField]
    private Vector2 worldReferencePoint1;

    [SerializeField]
    private Vector2 worldReferencePoint2;

    private float scale;
    private Vector2 mapStartPosition;


    private void Awake()
    {
        float mapDistance = Vector2.Distance(mapReferencePoint1, mapReferencePoint2);
        float worldDistance = Vector2.Distance(worldReferencePoint1, worldReferencePoint2);

        scale = worldDistance / mapDistance;
        scale *= -1;
        mapStartPosition = mapTransform.localPosition.XYToVector2() - (playerTransform.position.XZToVector2() / scale);
    }

    private void Update()
    {
        Quaternion rotation = playerTransform.rotation * Quaternion.Euler(0, 180, 0);
        rotation.x = 0;
        rotation.z = rotation.y * -1;
        rotation.y = 0;

        playerIcon.rotation = rotation;
        mapTransform.localPosition = (playerTransform.position.XZToVector2() / scale) + mapStartPosition;
    }
}
