using UnityEngine;
using System.Collections.Generic;

public class ConductiveItem : MonoBehaviour
{
    [SerializeField] private Renderer[] pointsToPower;
    [SerializeField] private bool poweredOn = false;

    private List<ConductiveItem> poweringMeOn;

    [SerializeField] private bool isPowerSource = false;

    public bool PoweredOn
    {
        get { return poweredOn; }
        set { TogglePower(value); }
    }

    [SerializeField] private Material emissiveMaterial;
    private List<Material> defaultMaterial;
    private void TogglePower(bool toggle)
    {
        poweredOn = toggle || isPowerSource;
        if (poweredOn || isPowerSource)
        {
            foreach (Renderer point in pointsToPower)
            {
                if (point != null)
                {
                    point.material = emissiveMaterial;
                }
            }
            // Logic to handle when the item is powered on
            Debug.Log($"{gameObject.name} is powered on.");
        }
        else
        {
            for (int i = 0; i < pointsToPower.Length; i++)
            {
                if (pointsToPower[i] != null && defaultMaterial != null && defaultMaterial.Count > i)
                {
                    pointsToPower[i].material = defaultMaterial[i];
                }
            }
            // Logic to handle when the item is powered off
            Debug.Log($"{gameObject.name} is powered off.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poweringMeOn = new List<ConductiveItem>();

        foreach (Renderer point in pointsToPower)
        {
            if (point != null)
            {
                if (defaultMaterial == null)
                    defaultMaterial = new List<Material>();
                defaultMaterial.Add(point.material);
            }
        }

        if (poweredOn)
        {
            TogglePower(true); // Ensure the item starts in the correct state
        }
        else
        {
            TogglePower(false); // Ensure the item starts in the correct state
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Example key to toggle power
        {
            TogglePower(!poweredOn);
        }
    }

    public void RegisterItem(ConductiveItem item)
    {
        if (!poweringMeOn.Contains(item))
        {
            poweringMeOn.Add(item);
            Debug.Log($"{gameObject.name} registered to power on {item.gameObject.name}.");
            if (!poweredOn)
            {
                TogglePower(true); // If this item is being powered on, turn it on
            }
        }
    }

    public void UnregisterItem(ConductiveItem item)
    {
        if (poweringMeOn.Contains(item))
        {
            poweringMeOn.Remove(item);
            Debug.Log($"{gameObject.name} unregistered from powering on {item.gameObject.name}.");
            if(poweringMeOn.Count <= 0)
            {
                TogglePower(false); // If no items are powering this one, turn it off
            }
        }
    }
}
