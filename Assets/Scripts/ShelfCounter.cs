using Enums;
using UnityEngine;

public class ShelfCounter : MonoBehaviour
{
    [SerializeField] private GameObject[] _shelf;
    
    private void Start()
    {
        int value = PlayerPrefs.GetInt("ShelfBuyed" + EquipmentType.Shelf, -1);
        
        if (value >= 0)
        {
            for (int i = 0; i <= value; i++)
                ActivateShelf(i);
        }
    }
    
    private void ActivateShelf(int index)
    {
        _shelf[index].SetActive(true);
        Debug.Log("Activating shelf at index: " + index);
    }
}
