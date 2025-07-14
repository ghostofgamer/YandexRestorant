using KitchenEquipmentContent;
using KitchenEquipmentContent.FryerContent;
using MysteryGiftContent;
using PlayerContent.LevelContent;
using RestaurantContent.CashRegisterContent;
using UI.Screens.AssemblyScreens;
using UI.Screens.NewLevelContent;
using UnityEngine;

namespace UI.Screens
{
    public class WindowOpener : MonoBehaviour
    {
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;
        [SerializeField] private AssemblyCoffeeScreen _assemblyCoffeeScreen;
        [SerializeField] private AssemblySodaScreen _assemblySodaScreen;
        [SerializeField] private AssemblyTable _assemblyTable;
        [SerializeField] private AssemblyDrinkTable _assemblyDrinkTable;
        [SerializeField] private AssemblyDrinkTable _assemblySodaTable;
        [SerializeField] private AssemblyCashRegisterOrderScreen _assemblyCashRegisterOrderScreen;
        [SerializeField] private AssemblyCardRegisterOrderScreen _assemblyCardRegisterOrderScreen;
        [SerializeField] private CashRegister _cashRegister;
        [SerializeField] private AssemblyFryerTable _assemblyFryerTable;
        [SerializeField] private AssemblyFryerScreen _assemblyFryerScreen;
        [SerializeField] private MysteryGift _mysteryGift;
        [SerializeField] private MysteryBoxScreen _mysteryBoxScreen;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private NewLevelUpScreen _newLevelUpScreen;

        private void OnEnable()
        {
            _assemblyTable.BurgerAssemblyBeginig += OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig += OpenAssemblyCoffeeScreen;
            _assemblySodaTable.DrinkAssemblyBeginig += OpenAssemblySodaScreen;
            _cashRegister.CashRegisterAssemblyBeginig += OpenCashRegisterOrderScreen;
            _cashRegister.CashRegisterOrderCompleted += CloseCashRegisterScreen;
            _assemblyFryerTable.FriersAssemblyBeginig += OpenAssemblyFryerScreen;
            _mysteryGift.BoxActivation += OpenMysteryBoxScreen;
            _playerLevel.LevelAdded += OpenNewLevelScreen;
        }

        private void OnDisable()
        {
            _assemblyTable.BurgerAssemblyBeginig -= OpenAssemblyBurgerScreen;
            _assemblyDrinkTable.DrinkAssemblyBeginig -= OpenAssemblyCoffeeScreen;
            _assemblySodaTable.DrinkAssemblyBeginig -= OpenAssemblySodaScreen;
            _cashRegister.CashRegisterAssemblyBeginig -= OpenCashRegisterOrderScreen;
            _cashRegister.CashRegisterOrderCompleted -= CloseCashRegisterScreen;
            _assemblyFryerTable.FriersAssemblyBeginig -= OpenAssemblyFryerScreen;
            _mysteryGift.BoxActivation -= OpenMysteryBoxScreen;
            _playerLevel.LevelAdded -= OpenNewLevelScreen;
        }

        private void OpenAssemblyBurgerScreen()
        {
            _assemblyBurgerScreen.OpenScreen();
        }

        private void OpenAssemblyCoffeeScreen()
        {
            _assemblyCoffeeScreen.OpenScreen();
        }

        private void OpenAssemblySodaScreen()
        {
            _assemblySodaScreen.OpenScreen();
        }

        private void OpenCashRegisterOrderScreen(bool isCard)
        {
            if (!isCard)
                _assemblyCashRegisterOrderScreen.OpenScreen();
            else
                _assemblyCardRegisterOrderScreen.OpenScreen();
        }

        private void CloseCashRegisterScreen()
        {
            _assemblyCashRegisterOrderScreen.CloseScreen();
            _assemblyCardRegisterOrderScreen.CloseScreen();
        }

        private void OpenAssemblyFryerScreen()
        {
            _assemblyFryerScreen.OpenScreen();
        }

        private void OpenMysteryBoxScreen()
        {
            _mysteryBoxScreen.OpenScreen();
        }

        private void OpenNewLevelScreen()
        {
            _newLevelUpScreen.OpenScreen();
        }
    }
}