using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    new public string name = "New MyScriptableObject"; //suprascrie atributul name
    public string objectName = "New MyScriptableObject";
    public bool colorIsRandom = false;
    public Color thisColor = Color.white;
    public Sprite icon;
    public Vector3[] spawnPoints;
}

public class InventoryManager : MonoBehaviour
{

    // singleton
    public static InventoryManager instance;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    void Awake()
    {
        instance = this;
    }

    //lista de obiecte
    public List<Item> items = new List<Item>();

    //metode pentru gestionare
    public void Add(Item item)
    {
        items.Add(item);
        onInventoryChangedCallback.Invoke(); //notifica despre modificare
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onInventoryChangedCallback.Invoke(); //notifica despre  modificare
    }
}
