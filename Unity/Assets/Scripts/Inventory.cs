using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<GameObject> inventory;
    public int numSlots;
    public Image[] slots;
    public bool[] slotStates;
    void Start()
    {
        slotStates = new bool[numSlots];
    }
    void Update()
    {
        // make empty slots invisible
        foreach (Image slot in slots)
            if (!slot.sprite)
                slot.color = Color.clear;
    }
    public void Stash(GameObject obj)
    {
        if (inventory.Count < numSlots)
        {
            inventory.Add(obj);

            // find the first open slot 
            int slot;
            for (slot = 0; slot < numSlots; slot++)
                if (!slotStates[slot])
                    break;

            slotStates[slot] = true;
            slots[slot].sprite = obj.GetComponent<Trash>().icon;
            slots[slot].color = Color.white;
        }
    }
    public void DropAll(Vector3 pos, Transform parent)
    {
        StartCoroutine(Dropall(pos, parent));
    }
    IEnumerator Dropall(Vector3 pos, Transform parent)
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            inventory[i].SetActive(true);
            inventory[i].transform.position = pos;
            inventory[i].transform.rotation = Random.rotation;

            slots[i].sprite = null;
            slotStates[i] = false;

            yield return new WaitForSeconds(.1f);
        }
        inventory.Clear();
    }
}
