using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class AnimatedBar : MonoBehaviour
{
    [SerializeField]
    private Image barMask;

    [SerializeField]
    private RawImage fillBar;

    [SerializeField]
    private float animationSpeed = 0.1f;

    [SerializeField]
    private float fillAnimationTime = 1f;

    public float FillAmount => barMask.fillAmount;

    public void SetFillAmount(float fillPercentage)
    {
        Assert.IsTrue(fillPercentage >= 0 && fillPercentage <= 1, "AnimatedBar - fillPercentage should be between 0 and 1 inclusive");

        LeanTween.value(barMask.fillAmount, fillPercentage, fillAnimationTime).setOnUpdate((value) =>
        {
            barMask.fillAmount = value;
        });
    }

    private void Update()
    {
        Rect uvRect = fillBar.uvRect;
        uvRect.x += animationSpeed * Time.deltaTime;

        if (uvRect.x >= 1)
        {
            uvRect.x -= 1;
        }
        else if (uvRect.x <= -1)
        {
            uvRect.x += 1;
        }

        fillBar.uvRect = uvRect;
    }
}
