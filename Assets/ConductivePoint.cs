using UnityEngine;

public class ConductivePoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ConductiveItem item = collision.gameObject.GetComponent<ConductiveItem>();
        if (item != null)
        {
            
            if (item != null)
            {
                item.TogglePower(true); // Power on the item when it collides with this point
            }
        }
    }
}
