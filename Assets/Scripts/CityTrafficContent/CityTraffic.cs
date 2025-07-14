using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace CityTrafficContent
{
    public abstract class CityTraffic<T> : MonoBehaviour,ICityTraffic where T : MonoBehaviour
    {
        [SerializeField] private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
        [SerializeField] private T[] _prefabs;
        [SerializeField] private Transform _container;
        [SerializeField] private int _spawnAmount;
        [SerializeField] protected int MaxActiveObject;
        [SerializeField] private float _otherSpawnValue;

        protected List<ObjectPool<T>> _objectPools = new List<ObjectPool<T>>();

        private int _activeNPC = 0;
        private WaitForSeconds _waitOneSecond = new WaitForSeconds(1f);
        private WaitForSeconds _waitOtherSecondsSeconds;

        private void Start()
        {
            _waitOtherSecondsSeconds = new WaitForSeconds(_otherSpawnValue);

            foreach (var labubuNpcPrefab in _prefabs)
            {
                var clientPool = new ObjectPool<T>(labubuNpcPrefab, _spawnAmount, _container);
                clientPool.EnableAutoExpand();
                _objectPools.Add(clientPool);
            }

            StartCoroutine(SpawnNPC());
        }

        protected IEnumerator SpawnNPC()
        {
            int previousIndex = -1;
            
            while (true)
            {
                if (_activeNPC > MaxActiveObject)
                    yield return _waitOtherSecondsSeconds;
                else
                    yield return _waitOneSecond;

                int index;
                do
                {
                    index = Random.Range(0, _spawnPoints.Count);
                } while (index == previousIndex && _spawnPoints.Count > 1);
                
                
                // int index = Random.Range(0, _spawnPoints.Count);
                previousIndex = index;
                SpawnRandomClient(_spawnPoints[index]);
            }
        }

        public void SpawnRandomClient(SpawnPoint spawnPoint)
        {
            ObjectPool<T> randomPool = _objectPools[Random.Range(0, _objectPools.Count)];
            T objectNpc = randomPool.GetFirstObject();

            if (objectNpc == null)
                return;

            SetPosition(objectNpc, spawnPoint);
            objectNpc.gameObject.SetActive(true);
            IncreaseActiveNPC();
        }

        private void SetPosition(T objectNpc, SpawnPoint spawnPoint)
        {
            objectNpc.transform.position = spawnPoint.spawnPosition.position;
            Init(objectNpc, spawnPoint.pathGroups[Random.Range(0, spawnPoint.pathGroups.Count)], this);
        }

        public abstract void Init(T gameNPC, GameObject path, CityTraffic<T> cityTraffic);


        public void IncreaseActiveNPC()
        {
            _activeNPC++;
        }
        
        public void DecreaseActiveNPC()
        {
            _activeNPC--;

            if (_activeNPC <= 0)
                _activeNPC = 0;
        }
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnPosition;
        public List<GameObject> pathGroups;
    }
}