using System.Collections;
using Enums;
using I2.Loc;
using InputContent;
using ItemContent;
using SoContent;
using UI.Screens;
using UI.Screens.TutorialScreens;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class TutorialActivator : MonoBehaviour
    {
        [SerializeField] private NameRestaurantScreen _nameRestaurantScreen;
        [SerializeField] private LookAroundScreen _lookAroundScreen;
        [SerializeField] private MoveScreen _moveScreen;
        [SerializeField] private TutorialObject bunObject;
        [SerializeField] private TutorialObject _assemblyTable;
        [SerializeField] private TutorialObject _trash;
        [SerializeField] private TutorialObject _burgerPackageBox;
        [SerializeField] private TutorialObject _rawCutletContainer;
        [SerializeField] private TutorialObject _grillTutorObject;
        [SerializeField] private TutorialObject _openCloseRest;
        [SerializeField] private TutorialObject _cashRegister;
        [SerializeField] private TutorialObject _tableFirstClient;
        [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;
        [SerializeField] private TutorialDescription _tutorialDescription;
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private LookAroundEventTrigger _lookAroundEventTrigger;
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _touchShopImage;
        [SerializeField] private GameObject _touchSkipImage;
        [SerializeField] private BoxesCounter _boxesCounter;
        [SerializeField] private Button[] _closeCashRegisters;
        [SerializeField] private AssemblyBurgerScreen _assemblyBurgerScreen;
        [SerializeField] private GameObject _blackScreen;
        [SerializeField] private ActionButtonActivator _actionButtonActivator;
        [SerializeField] private PlayerRotator _playerRotator;

        [Header("Guidance Lines")] [SerializeField]
        private GuidanceLine.GuidanceLine _guidanceLine;

        [SerializeField] private GuidanceLine.GuidanceLine _guidanceLineStartScene;
        [SerializeField] private GuidanceLine.GuidanceLine _guidanceLineRawCutlet;
        [SerializeField] private GuidanceLine.GuidanceLine _guidanceLineRawCutletStartScene;

        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _dailyReward;
        [SerializeField] private Button _fortune;

        private ItemBasket basketRawCutlet;
        private Coroutine _searchBoxCoroutine;
        private int _counter;

        public void ActivateNameRestaurant()
        {
            _counter++;
            SetValueButtonTopUI(false);
            _nameRestaurantScreen.OpenScreen();
        }

        public void ActivateLookAround()
        {
            _counter++;
            SetValueButtonTopUI(false);
            _lookAroundScreen.OpenScreen();
        }

        public void ActivateMove()
        {
            _counter++;
            SetValueButtonTopUI(false);
            _moveScreen.OpenScreen();
        }

        public void TakeBunBox()
        {
            _counter++;
            SetValueButtonTopUI(false);
            Debug.Log("TakeBunBox");
            bunObject.gameObject.SetActive(true);

            _playerRotator.RotateToTarget(bunObject.transform);

            bunObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void PutBunsAssemblyTableBunBox()
        {
            _counter++;
            _actionButtonActivator.Completed();
            // _boxesCounter.AddBox(bunObject.gameObject);
            SetValueButtonTopUI(false);
            Debug.Log("PutBunsAssemblyTableBunBox");
            _assemblyTable.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_assemblyTable.LookPosition);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void ThrowEmptyBoxInTrash()
        {
            _counter++;
            SetValueButtonTopUI(false);
            Debug.Log("ThrowEmptyBoxInTrash");
            _assemblyTable.DeactivateTutorPoint();
            _trash.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_trash.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void TakeBoxBurgerPackages()
        {
            _counter++;
            SetValueButtonTopUI(false);
            Debug.Log("TakeBoxBurgerPackages");
            _trash.DeactivateTutorPoint();
            _burgerPackageBox.gameObject.SetActive(true);
            _burgerPackageBox.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_burgerPackageBox.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void PutPackagesAssemblyTable()
        {
            _counter++;
            // _boxesCounter.AddBox(_burgerPackageBox.gameObject);
            SetValueButtonTopUI(false);
            Debug.Log("PutPackagesAssemblyTable");
            // _burgerPackageBox.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_assemblyTable.LookPosition);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void ThrowEmptyBoxInTrashSecond()
        {
            _counter++;
            SetValueButtonTopUI(false);
            Debug.Log("ThrowEmptyBoxInTrashSecond");
            _assemblyTable.DeactivateTutorPoint();
            _trash.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_trash.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void OrderBurgerPatties()
        {
            _counter++;
            _trash.DeactivateTutorPoint();
            SetValueButtonTopUI(false);
            Debug.Log("OrderBurgerPatties");
            _blackScreen.SetActive(true);
            _shopButton.interactable = true;
            _assemblyTable.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            // _playerInput.enabled = false;
            _lookAroundEventTrigger.gameObject.SetActive(false);
            _joystick.SetActive(false);
            _touchShopImage.SetActive(true);
        }

        public void SkipDelivery()
        {
            _counter++;
            _blackScreen.SetActive(false);
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _touchSkipImage.SetActive(true);
        }

        public void TakeBoxesOutside()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _lookAroundEventTrigger.gameObject.SetActive(true);
            _joystick.SetActive(true);
            _touchSkipImage.SetActive(false);

            if (_searchBoxCoroutine != null)
                StopCoroutine(_searchBoxCoroutine);

            _searchBoxCoroutine = StartCoroutine(SearchBoxOutside());
        }

        private IEnumerator SearchBoxOutside()
        {
            yield return new WaitForSeconds(1f);
            basketRawCutlet = _boxesCounter.GetItemBasketByType(ItemType.RawCutlet);
            Debug.Log("basketRawCutlet " + basketRawCutlet.gameObject.name);
            basketRawCutlet.GetComponent<TutorialObject>().ActivateTutorPoint();

            if (_counter <= 0)
            {
                _guidanceLineStartScene.endPoint = basketRawCutlet.transform;
                _guidanceLineStartScene.gameObject.SetActive(true);
            }
            else
            {
                _guidanceLine.endPoint = basketRawCutlet.transform;
                _guidanceLine.gameObject.SetActive(true);
            }

            _counter++;
        }

        public void PutRawCutletInContainer()
        {
            if (_counter <= 0)
            {
                _guidanceLine.gameObject.SetActive(false);
                _guidanceLineStartScene.gameObject.SetActive(false);
                _guidanceLineRawCutletStartScene.gameObject.SetActive(true);
            }
            else
            {
                _guidanceLine.gameObject.SetActive(false);
                _guidanceLineStartScene.gameObject.SetActive(false);
                _guidanceLineRawCutlet.gameObject.SetActive(true);
            }

            // _boxesCounter.AddBox(basketRawCutlet.gameObject);
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _rawCutletContainer.ActivateTutorPoint();
        }

        public void ThrowEmptyBoxInTrashThird()
        {
            _guidanceLineRawCutletStartScene.gameObject.SetActive(false);
            _guidanceLineRawCutlet.gameObject.SetActive(false);
            SetValueButtonTopUI(false);
            Debug.Log("ThrowEmptyBoxInTrashThird");
            _trash.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_trash.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void TakeRawCutletInTrayPlayer()
        {
            _trash.DeactivateTutorPoint();
            SetValueButtonTopUI(false);
            _rawCutletContainer.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_rawCutletContainer.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void PutCutletsOnGrill()
        {
            SetValueButtonTopUI(false);
            _rawCutletContainer.DeactivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _grillTutorObject.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_grillTutorObject.LookPosition);
        }

        public void FryCutletGrill()
        {
            SetValueButtonTopUI(false);
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _playerRotator.RotateToTarget(_grillTutorObject.LookPosition);
        }

        public void TakeWellCutlet()
        {
            SetValueButtonTopUI(false);
            _grillTutorObject.ActivateTutorPoint();
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _playerRotator.RotateToTarget(_grillTutorObject.LookPosition);
        }

        public void PutWellCutletToContainer()
        {
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _grillTutorObject.DeactivateTutorPoint();
            _assemblyTable.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_assemblyTable.LookPosition);
        }

        public void LetsMakeFirstBurger()
        {
            SetValueButtonTopUI(false);
            _assemblyTable.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_assemblyTable.LookPosition);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void LetsSetPrice()
        {
            _assemblyBurgerScreen.CloseScreen();
            SetValueButtonTopUI(false);
            _shopButton.interactable = true;
            _blackScreen.SetActive(true);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _lookAroundEventTrigger.gameObject.SetActive(false);
            _joystick.SetActive(false);
            _touchShopImage.SetActive(true);
        }

        public void OpenRestaurant()
        {
            _blackScreen.SetActive(false);
            SetValueButtonTopUI(false);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _lookAroundEventTrigger.gameObject.SetActive(true);
            _joystick.SetActive(true);
            _openCloseRest.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_openCloseRest.transform);
        }

        public void TakeFirstOrder()
        {
            SetValueButtonTopUI(false);
            
            foreach (var closeCashRegister in _closeCashRegisters)
                closeCashRegister.interactable = false;
            
            // _closeCashRegister.interactable = false;
            _openCloseRest.DeactivateTutorPoint();
            _cashRegister.ActivateTutorPoint();
            _playerRotator.RotateToTarget(_cashRegister.transform);
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        public void CleanTable()
        {
            SetValueButtonTopUI(false);
            _cashRegister.DeactivateTutorPoint();
            _tableFirstClient.ActivateTutorPoint();
            
            foreach (var closeCashRegister in _closeCashRegisters)
                closeCashRegister.interactable = true;
            
            // _closeCashRegister.interactable = true;
            _tutorDescriptionUI.StartStage(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
            _playerRotator.RotateToTarget(_tableFirstClient.transform);
        }

        public void TutorCompleted()
        {
            _cashRegister.DeactivateTutorPoint();
            
            foreach (var closeCashRegister in _closeCashRegisters)
                closeCashRegister.interactable = true;
            
            SetValueButtonTopUI(true);
            // _tableFirstClient.DeactivateTutorPoint();
            _tutorDescriptionUI.StartCompleted(GetDescriptionText(_tutorial.CurrentType), (int)_tutorial.CurrentType);
        }

        private string GetDescriptionText(TutorialType currentType)
        {
            TutorialDescription.Description description =
                System.Array.Find(_tutorialDescription.descriptions, d => d.type == currentType);
            // return description != null ? description.text : string.Empty;
            return description != null ?  description.type.ToString() : string.Empty;
        }

        private void SetValueButtonTopUI(bool value)
        {
            _shopButton.interactable = value;
            _dailyReward.interactable = value;
            // _fortune.interactable = value;
        }
    }
}