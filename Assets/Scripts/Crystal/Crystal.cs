using System;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public CrystalData _crystalData;
    public static event Action OnTakeCrystal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            if (OnTakeCrystal != null)
                OnTakeCrystal.Invoke();
        }

    }
}
