using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Panel : MonoBehaviour
{
    [SerializeField] protected PlayerUIManager UIManager;

    protected void UpdateCashDisplay(TextMeshProUGUI cashDisplay, CashComponent cashComponent)
    {
        cashDisplay.text = "Cash: $" + cashComponent.GetCashHeld().ToString();
    }

    protected bool IsOverlapping(RectTransform rectTransformA, RectTransform rectTransformB)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];
        rectTransformA.GetWorldCorners(cornersA);
        rectTransformB.GetWorldCorners(cornersB);
        Rect rectA = new Rect(
            cornersA[0].x,
            cornersA[0].y,
            cornersA[3].x - cornersA[0].x,
            cornersA[1].y - cornersA[0].y
        );

        Rect rectB = new Rect(
            cornersB[0].x,
            cornersB[0].y,
            cornersB[3].x - cornersB[0].x,
            cornersB[1].y - cornersB[0].y
        );

        if (rectA.Overlaps(rectB))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public virtual void Enter(PlayerUIManager UIManager)
    {

    }

    public virtual void Exit(PlayerUIManager UIManager)
    {

    }

    public virtual void UpdatePanel(PlayerUIManager UIManager)
    {

    }

    public virtual void FixedUpdatePanel(PlayerUIManager UIManager)
    {

    }

}