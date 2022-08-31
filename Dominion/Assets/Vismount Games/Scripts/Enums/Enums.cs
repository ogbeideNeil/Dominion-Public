public enum Layers : sbyte
{
    Default,
    TransparentFX,
    IgnoreRaycast,
    Enemy,
    Water,
    UI,
    Terrain,
    Player
}

public enum Tags : sbyte
{
    Untagged,
    Respawn,
    Finish,
    EditorOnly,
    MainCamera,
    Player,
    GameController
}

public enum EnemyType : sbyte
{
    Default,
    Treant,
    Golem
}

/// <summary>
/// Used in a ScriptableObject to indicate the type of the item about which the SO is storing information
/// </summary>
/// <see cref="Objects.BaseObjectData"/>
public enum ItemType : sbyte
{
    Area, Building, Equipment, Food, Medicine, NPC, Player, Story, Weapon
}

/// <summary>
/// Used to indicate priority (e.g. completion of objectives)
/// </summary>
/// <see cref="GD.ScriptableTypes.RuntimeGameObjectiveList"/>
public enum PriorityType : sbyte
{
    //assigning explicit values here allows us to sort whatever entity (e.g. GameObjective) use this type
    Low = 0, Normal = 1, High = 2, Critical = 3
}

/// <summary>
/// Used to specify if an onscreen object (e.g. a waypoint) is always shown
/// </summary>
/// <see cref="GD.ScriptableTypes.RuntimeGameObjectiveList"/>
public enum VisibilityStrategyType : sbyte
{
    ShowAlways, ShowWithin, ShowNever
}