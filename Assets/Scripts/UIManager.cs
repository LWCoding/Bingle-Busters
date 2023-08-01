using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI weaponText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void UpdateWeaponText(string weaponName)
    {
        weaponText.text = "Weapon Selected: " + weaponName;
    }

}
