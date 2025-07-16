using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ConductiveItem : MonoBehaviour
{
    [SerializeField] private Renderer[] pointsToPower;
    [SerializeField] private bool poweredOn = false;

    public List<ConductiveItem> poweringMeOn;
    public List<ConductiveItem> poweringItems;


    [SerializeField] private bool isPowerSource = false;

    [SerializeField] private LayerMask conductiveLayer;

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
        poweringItems = new List<ConductiveItem>();

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

        //bool anyPoweredOn = false;
        //foreach (ConductiveItem item in poweringMeOn)
        //{
        //    if(item.isPowerSource)
        //    {
        //        anyPoweredOn = true;
        //        break;
        //    }
        //}

        //// Divide in 2 so we dont need to iterate of the big loops
        //// if we are directly connected to a power source
        //foreach (ConductiveItem item in poweringMeOn)
        //{
        //    foreach (ConductiveItem poweringItem in item.poweringMeOn)
        //    {
        //        if (poweringItem.isPowerSource)
        //        {
        //            anyPoweredOn = true;
        //            break;
        //        }
        //    }
        //}
        //TogglePower(anyPoweredOn);

        // Then in your main code:
        bool anyPoweredOn = IsConnectedToPowerSource();
        TogglePower(anyPoweredOn);

    }

    private bool IsConnectedToPowerSource()
    {
        var visited = new HashSet<ConductiveItem>();
        var queue = new Queue<ConductiveItem>();

        // Start with items directly connected to this one
        foreach (ConductiveItem item in poweringMeOn)
        {
            if (!visited.Contains(item))
            {
                queue.Enqueue(item);
                visited.Add(item);
            }
        }

        while (queue.Count > 0)
        {
            ConductiveItem current = queue.Dequeue();

            if (current.isPowerSource)
                return true;

            // Add all items connected to current item
            foreach (ConductiveItem item in current.poweringMeOn)
            {
                if (!visited.Contains(item))
                {
                    queue.Enqueue(item);
                    visited.Add(item);
                }
            }
        }

        return false;
    }



    public void RegisterItemPoweringMe(ConductiveItem item)
    {

        if(poweringItems.Contains(item))
        {
            Debug.LogWarning($"{gameObject.name} is already registered to power {item.gameObject.name}.");
            return;
        }
        poweringMeOn.Add(item);
        Debug.Log($"{gameObject.name} registered to power on {item.gameObject.name}.");

    }

    public void UnregisterItemPoweringMe(ConductiveItem item)
    {
        if(!poweringMeOn.Contains(item))
        {
            Debug.LogWarning($"{gameObject.name} is not registered to power {item.gameObject.name}.");
            return;
        }
        poweringMeOn.Remove(item);
        Debug.Log($"{gameObject.name} unregistered from powering on {item.gameObject.name}.");

    }

    public void RegisterItemImPowering(ConductiveItem item)
    {
        poweringItems.Add(item);
        Debug.Log($"{gameObject.name} registered to power {item.gameObject.name}.");
    }

    public void UnregisterItemImPowering(ConductiveItem item)
    {
        poweringItems.Remove(item);
        Debug.Log($"{gameObject.name} unregistered from powering {item.gameObject.name}.");
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    poweringMeOn.Clear();
    //    ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
    //    if (item != null && ((1 << collision.contacts[0].thisCollider.gameObject.layer) & conductiveLayer) != 0)
    //    {
    //        item.RegisterItemPoweringMe(this);
    //        RegisterItemPoweringMe(item);
    //    }
    //}



    private void OnCollisionEnter(Collision collision)
    {
        ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
        if (item != null && ((1 << collision.contacts[0].thisCollider.gameObject.layer) & conductiveLayer) != 0)
        {
            item.RegisterItemPoweringMe(this);
            RegisterItemPoweringMe(item);
            //Debug.Log("Collided with: " + collision.contacts[0].thisCollider.name);
            //if (this.PoweredOn && !item.PoweredOn)
            //{
            //     If the conductive item is powered on and the colliding item is not, power it on
            //    Debug.Log($"Powering on {item.gameObject.name} from {this.gameObject.name}.");
            //    item.PoweredOn = true;  Power on the item when it collides with this point
            //    item.RegisterItem(this);
            //    this.RegisterItem(item);
            //}
            //else if (!this.PoweredOn && item.PoweredOn)
            //{
            //     If the conductive item is not powered on and the colliding item is, power condutrive item on
            //    Debug.Log($"Powering oon {this.gameObject.name} from {item.gameObject.name}.");
            //    this.PoweredOn = true;  Power off the item when it collides with this point
            //    RegisterItem(item);
            //}
            //else if (this.PoweredOn && item.PoweredOn)
            //{
            //    Debug.Log("Both items are powered on, no action taken.");
            //}
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
        //Debug.Log(collision.contacts[0] + " just exited");
        if (item != null)
        {
      

            item.UnregisterItemPoweringMe(this);
            UnregisterItemPoweringMe(item);
        }
    }
}
