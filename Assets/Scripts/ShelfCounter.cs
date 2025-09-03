using System;
using Enums;
using LoadingSceneContent;
using SaveContent;
using UnityEngine;

public class ShelfCounter : MonoBehaviour
{
    [SerializeField] private GameObject[] _shelf;
    [SerializeField] private LoadingGame _loadingGame;

    private void OnEnable()
    {
        _loadingGame.MirraSDKInitialization += Init;
    }

    private void OnDisable()
    {
        _loadingGame.MirraSDKInitialization -= Init;
    }
    /*
    private void Start()
    {
        int value = PlayerPrefs.GetInt("ShelfBuyed" + EquipmentType.Shelf, -1);

        if (value >= 0)
        {
            for (int i = 0; i <= value; i++)
                ActivateShelf(i);
        }
    }
    */

    private void Init()
    {
        // int value = PlayerPrefs.GetInt("ShelfBuyed" + EquipmentType.Shelf, -1);
        int value = StorageHelper.GetInt("ShelfBuyed" + EquipmentType.Shelf, -1);
        
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