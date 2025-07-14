using UnityEngine;

namespace AssemblyBurgerContent
{
    public class BurgerBoard : MonoBehaviour
    {
        [SerializeField] private Transform _centerPosition;

        public Transform CenterPosition => _centerPosition;
    }
}