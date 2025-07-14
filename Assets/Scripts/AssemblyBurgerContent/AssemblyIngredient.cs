using UnityEngine;

namespace AssemblyBurgerContent
{
    public class AssemblyIngredient : MonoBehaviour
    {
        [SerializeField] private Transform _positionUpIngredient;

        public Transform PositionUpIngredient=>_positionUpIngredient;
    }
}