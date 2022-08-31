using GD.Objects;
using UnityEngine;

public class ZoneObject : BaseObject
{
    [Header("Object Data")]
    [Tooltip("Specify the scriptable object (of type ZoneObject) which stores the data on this zone object")]
    public ZoneObjectData ZoneData;
}