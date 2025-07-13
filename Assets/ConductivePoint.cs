using UnityEngine;

public class ConductivePoint : MonoBehaviour
{
    public ConductiveItem conductiveItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conductiveItem = GetComponentInParent<ConductiveItem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
        if (item != null)
        {
            if(conductiveItem.PoweredOn && !item.PoweredOn)
            {
                // If the conductive item is powered on and the colliding item is not, power it on
                Debug.Log($"Powering on {item.gameObject.name} from {conductiveItem.gameObject.name}.");
                item.PoweredOn = true; // Power on the item when it collides with this point
                item.RegisterItem(conductiveItem);
            }
            else if (!conductiveItem.PoweredOn && item.PoweredOn)
            {
                // If the conductive item is not powered on and the colliding item is, power condutrive item on
                Debug.Log($"Powering oon {conductiveItem.gameObject.name} from {item.gameObject.name}.");
                conductiveItem.PoweredOn = true; // Power off the item when it collides with this point
                conductiveItem.RegisterItem(item);
            }
            else if (conductiveItem.PoweredOn && item.PoweredOn)
            {
                 Debug.Log("Both items are powered on, no action taken.");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
        if (item != null)
        {
            // If the conductive item is powered on and the colliding item is, power it off
            Debug.Log($"Powering off {item.gameObject.name} from {conductiveItem.gameObject.name}.");
            item.PoweredOn = false; // Power off the item when it exits collision with this point
            item.UnregisterItem(conductiveItem);
            conductiveItem.UnregisterItem(item);
        }
    }
}
