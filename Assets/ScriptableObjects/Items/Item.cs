using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string nameOfItem;
    public GameObject itemPrefab;
    public string[] answersToItem;
}
