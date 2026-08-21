using Unity.VisualScripting;
using UnityEngine;

public class CashComponent : MonoBehaviour
{

    [SerializeField] private int initialCash;
    private int cashHeld;

    private void Start()
    {
        SetCashHeld(initialCash);
    }

    public int GetCashHeld()
    {
        return cashHeld;
    }

    public void SetCashHeld(int newCashHeld)
    {
        cashHeld = newCashHeld;
        if (cashHeld < 0) { cashHeld = 0; }
    }

    public void LoseCash(int amount)
    {
        cashHeld -= amount;
        if(cashHeld < 0) { cashHeld = 0; }
    }

    public void TransferTo(CashComponent cashComponent, int amount)
    {
        amount = Mathf.Min(amount, cashHeld);
        cashHeld -= amount;
        cashComponent.SetCashHeld(cashComponent.GetCashHeld() + amount);

    }

}
