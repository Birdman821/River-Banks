using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;

public class Door : MonoBehaviour
{
    [SerializeField] private CashComponent playerCashComponent;
    [SerializeField] private int requiredCash = 0;
    [SerializeField] private List<Key> requiredKeys = new List<Key>();
    //[field: SerializeField] public Button requiredButton { get; private set; }
    [SerializeField] private float requiredWater;
    [SerializeField] private bool closeInWater = true; 

    void Start()
    {
        requiredKeys.ForEach((Key key) => key.InitializeDoor(this));
    }

    private void AttemptOpen()
    {
        if(requiredKeys.Count <= 0 && requiredCash <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    public void RemoveKey(Key removedKey)
    {
        requiredKeys.Remove(removedKey);
        if(requiredKeys.Count <= 0)
        {
            AttemptOpen();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(requiredCash != 0 && collision.transform.CompareTag("Player"))
        {
            if(playerCashComponent.GetCashHeld() >= requiredCash)
            {
                playerCashComponent.LoseCash(requiredCash);
                requiredCash = 0;
                AttemptOpen();
            }
        }
    }



}
