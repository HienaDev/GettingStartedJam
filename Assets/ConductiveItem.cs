using UnityEngine;
using System.Collections.Generic;

public class ConductiveItem : MonoBehaviour
{
    [SerializeField] private Renderer[] pointsToPower;
    private bool poweredOn = false;

    [SerializeField] private Material emissiveMaterial;
    private List<Material> defaultMaterial;
    public void TogglePower(bool toggle)
    {
        poweredOn = toggle;
        if (poweredOn)
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
        foreach (Renderer point in pointsToPower)
        {
            if (point != null)
            {
                if (defaultMaterial == null)
                    defaultMaterial = new List<Material>();
                defaultMaterial.Add(point.material);
            }
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
}
