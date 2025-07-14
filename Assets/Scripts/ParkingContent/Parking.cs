using ParkingContent;
using UnityEngine;

public class Parking : MonoBehaviour
{
    [SerializeField] private ParkingSpace[] _parkingPositions;

    public int GetCountFreeParkingPositions()
    {
        int freeCount = 0;

        foreach (var position in _parkingPositions)
        {
            if (!position.IsBusy)
                freeCount++;
        }

        return freeCount;
    }

    public ParkingSpace GetFreeParkingPosition()
    {
        foreach (var position in _parkingPositions)
        {
            if (!position.IsBusy)
                return position;
        }

        return null;
    }
}