using UnityEngine;

public class PCObjectsChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToDeactivate;
    [SerializeField] private GameObject[] _objectsToActivate;

    private void Start()
    {
        if (!Application.isMobilePlatform)
        {
            foreach (var obj in _objectsToDeactivate)
                obj.SetActive(false);
            
            foreach (var obj in _objectsToActivate)
                obj.SetActive(true);
        }
    }
}