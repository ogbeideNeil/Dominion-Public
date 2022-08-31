using UnityEngine;
using UnityEngine.Assertions;

public static class MonoBehaviourExtensions
{
    public static T GetComponentIfNull<T>(this MonoBehaviour behaviour, T component) where T : class
    {
        return component ?? behaviour.GetComponent<T>();
    }

    public static T GetComponentAssertNotNull<T>(this MonoBehaviour behaviour) where T : class
    {
        T component = behaviour.GetComponent<T>();
        Assert.IsNotNull(component, $"MonoBehaviourExtensions, Retrieved null component of type {typeof(T)} in object {behaviour.gameObject.name}");

        return component;
    }

    public static T GetComponentInChildrenAssertNotNull<T>(this MonoBehaviour behaviour) where T : class
    {
        T component = behaviour.GetComponentInChildren<T>();
        Assert.IsNotNull(component, $"MonoBehaviourExtensions, Retrieved null component of type {typeof(T)} in object {behaviour.gameObject.name}");

        return component;
    }

    public static T GetComponentWarnNull<T>(this MonoBehaviour behaviour) where T : class
    {
        T component = behaviour.GetComponent<T>();

        if (component == null)
        {
            Debug.LogWarning($"MonoBehaviourExtensions, Retrieved null component of type {typeof(T)} in object {behaviour.gameObject.name}");
        }

        return component;
    }

    public static T GetComponentInChildrenWarnNull<T>(this MonoBehaviour behaviour) where T : class
    {
        T component = behaviour.GetComponentInChildren<T>();

        if (component == null)
        {
            Debug.LogWarning(
                $"MonoBehaviourExtensions, Retrieved null component of type {typeof(T)} in object {behaviour.gameObject.name}");
        }

        return component;
    }
}