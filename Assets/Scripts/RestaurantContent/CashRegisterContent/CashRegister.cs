using System;
using System.Collections.Generic;
using System.Linq;
using AttentionHintContent;
using CameraContent;
using ClientsContent;
using Enums;
using I2.Loc;
using InteractableContent;
using OrdersContent;
using PlayerContent;
using PlayerContent.LevelContent;
using SettingsContent.SoundContent;
using TutorialContent;
using UnityEngine;
using WalletContent;
using WorkerContent;

namespace RestaurantContent.CashRegisterContent
{
    public class CashRegister : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Restaurant _restaurant;
        [SerializeField] private InteractableObject _interactableObject;
        [SerializeField] private Transform _clientPosition;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CameraPositionChanger _cameraPositionChanger;
        [SerializeField] private Transform _cameraCurrentPosition;
        [SerializeField] private CashRegisterViewer _cashRegisterViewer;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PriceOrderCounter _priceOrderCounter;
        [SerializeField] private PlayerLevel _playerLevel;
        [SerializeField] private Transform _cashierPosition;

        private Coroutine _coroutine;
        private DollarValue _currentGivingValue;
        private DollarValue _currentChangeValue;
        private Stack<int> _changeHistory = new Stack<int>();
        private const int MaxHistorySize = 100;
        private Cashier _cashier;

        public event Action<bool> CashRegisterAssemblyBeginig;

        public event Action CashRegisterOrderCompleted;

        public event Action<DollarValue> GivingValueChanged;

        public event Action PlayerOnSiteDisabled;

        public Client CurrentClient { get; private set; }

        public bool CashierOnSite { get; private set; } = false;

        public bool PlayerOnSite { get; private set; } = false;

        public Transform ClientPosition => _clientPosition;

        public Transform CashierPosition => _cashierPosition;

        private void OnEnable()
        {
            _interactableObject.OnAction += ShowAssemblyCashRegisterOrder;
        }

        private void OnDisable()
        {
            _interactableObject.OnAction -= ShowAssemblyCashRegisterOrder;
        }

        public void SetClient(Client client)
        {
            CurrentClient = client;
            SetCanvasActive(CurrentClient != null);
            _currentChangeValue = _priceOrderCounter.GetChange(client.PriceOrder, client.Cash);

            if (_cashier != null)
                _cashier.FindClient();
            // _canvas.SetActive(_currentClient != null);
        }

        private void ShowAssemblyCashRegisterOrder(PlayerInteraction playerInteraction)
        {
            SoundPlayer.Instance.PlayButtonClick();

            if (playerInteraction.CurrentDraggable != null || playerInteraction.PlayerTray.IsActive)
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("Free your hands"));

                return;
            }

            if (CurrentClient == null)
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("There's no one in line"));

                return;
            }

            if (CashierOnSite)
            {
                AttentionHintActivator.Instance.ShowHint(
                    LocalizationManager.GetTermTranslation("CashierOnSite"));
                return;
            }

            SetPlayerValue(true);

            CashRegisterAssemblyBeginig?.Invoke(CurrentClient.IsCard);
            _cameraPositionChanger.ChangePosition(_cameraCurrentPosition);
            _currentGivingValue = new DollarValue(0, 0);
            _cashRegisterViewer.Init(CurrentClient, _currentGivingValue);
        }

        public void SetCanvasActive(bool value)
        {
            _canvas.SetActive(value);
        }

        public void SetCashierValue(bool value)
        {
            CashierOnSite = value;
        }

        public void SetCashier(Cashier cashier)
        {
            _cashier = cashier;
        }

        public void CleanCashier()
        {
            _cashier = null;
        }

        public void SetPlayerValue(bool value)
        {
            PlayerOnSite = value;

            if (value == false)
                PlayerOnSiteDisabled?.Invoke();
        }

        [ContextMenu("AcceptOrder")]
        public void AcceptOrder()
        {
            if (CurrentClient == null)
                return;

            if (_currentGivingValue.ToTotalCents() != _currentChangeValue.ToTotalCents())
            {
                SoundPlayer.Instance.PlayError();
                Debug.Log("СДАЧА НЕВЕРНАЯ");
                return;
            }

            if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
                _tutorial.SetCurrentTutorialStage(TutorialType.TakeFirstOrder);

            _playerLevel.AddExp(5);
            SoundPlayer.Instance.PlayCashRegister();

            SetPlayerValue(false);

            CashRegisterOrderCompleted?.Invoke();
            CurrentClient.Paid();
            _wallet.Add(CurrentClient.PriceOrder);
            Client client = CurrentClient;
            CurrentClient = null;
            SetCanvasActive(CurrentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
            ClearGivingValue();
        }

        public void AcceptCardOrder()
        {
            if (CurrentClient == null)
                return;

            if (_tutorial.CurrentType == TutorialType.TakeFirstOrder)
                _tutorial.SetCurrentTutorialStage(TutorialType.TakeFirstOrder);

            _playerLevel.AddExp(5);
            SoundPlayer.Instance.PlayCashRegister();

            SetPlayerValue(false);

            CashRegisterOrderCompleted?.Invoke();
            CurrentClient.Paid();
            _wallet.Add(CurrentClient.PriceOrder);
            Client client = CurrentClient;
            CurrentClient = null;
            SetCanvasActive(CurrentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
            ClearGivingValue();
        }

        public void AcceptCashierOrder()
        {
            CurrentClient.Paid();
            _wallet.Add(CurrentClient.PriceOrder);
            Client client = CurrentClient;
            CurrentClient = null;
            SetCanvasActive(CurrentClient != null);
            _restaurant.AcceptOrder(client.Order, client);
            /*ClearGivingValue();*/

            _currentGivingValue = new DollarValue(0, 0);
            _changeHistory.Clear();
        }

        public void ChangeGivingValue(int cents)
        {
            int total = _currentGivingValue.ToTotalCents() + cents;

            if (total < 0)
                total = 0;

            _changeHistory.Push(cents);

            if (_changeHistory.Count > MaxHistorySize)
            {
                _changeHistory = new Stack<int>(_changeHistory.ToArray().TakeLast(MaxHistorySize).Reverse());
            }

            _currentGivingValue = new DollarValue(0, 0).FromTotalCents(total);
            GivingValueChanged?.Invoke(_currentGivingValue);
        }

        public void ClearGivingValue()
        {
            _currentGivingValue = new DollarValue(0, 0);
            _changeHistory.Clear();
            Debug.Log(_changeHistory.Count);
            GivingValueChanged?.Invoke(_currentGivingValue);
        }

        public void UndoLastChange()
        {
            if (_changeHistory.Count > 0)
            {
                int lastChange = _changeHistory.Pop();
                int total = _currentGivingValue.ToTotalCents() - lastChange;

                if (total < 0)
                    total = 0;

                _currentGivingValue = new DollarValue(0, 0).FromTotalCents(total);
                GivingValueChanged?.Invoke(_currentGivingValue);
            }
        }
    }
}