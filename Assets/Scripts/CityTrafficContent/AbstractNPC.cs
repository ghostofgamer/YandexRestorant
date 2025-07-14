using Interfaces;
using UnityEngine;

namespace CityTrafficContent
{
    public abstract class AbstractNPC : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _minDistance;

        private ICityTraffic _npcTraffic;
        private Transform[] _points;
        private int _index = 0;
        private bool _destinationSet = false;
        
        private void Update()
        {
            SetNextDestination();
            Roam();
        }

        public abstract void InitUniqueData();

        public void Init(GameObject path, ICityTraffic npcTraffic)
        {
            InitUniqueData();
            _npcTraffic = npcTraffic;
            _points = new Transform[path.transform.childCount];

            for (int i = 0; i < _points.Length; i++)
                _points[i] = path.transform.GetChild(i);
        }

        private void Roam()
        {
            if (!_destinationSet)
            {
                return;
            }
            
            if (Vector3.Distance(transform.position, _points[_index].position) < _minDistance)
            {
                _index = (_index + 1) % _points.Length;

                if (_index == 0)
                {
                    _npcTraffic.DecreaseActiveNPC();
                    gameObject.SetActive(false);
                    return;
                }
                
                SetNextDestination();
            }

            // _navMeshAgent.SetDestination(_points[_index].position);
            
            if (_animator != null)
                _animator.SetFloat("Vertical", !_navMeshAgent.isStopped ? 1 : 0);
        }
        
        private void SetNextDestination()
        {
            _navMeshAgent.SetDestination(_points[_index].position);
            _destinationSet = true;
        }
    }
}