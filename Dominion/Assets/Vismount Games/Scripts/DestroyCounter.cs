using UnityEngine;

public class DestroyCounter : MonoBehaviour
{
    [SerializeField]
    private int destroyAfterCount = 2;

    private int counter;

    public void AddCount()
    {
        AddCount(1);
    }

    public void AddCount(int count)
    {
        counter += count;

        if (counter >= destroyAfterCount)
        {
            Destroy(gameObject);
        }
    }
}