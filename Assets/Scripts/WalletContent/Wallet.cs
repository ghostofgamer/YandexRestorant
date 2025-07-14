using System;
using I2.Loc;
using TutorialContent;
using UI;
using UnityEngine;

namespace WalletContent
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private FlyValue _flyValue;
        [SerializeField] private TutorDescriptionUI _tutorDescriptionUI;

        public DollarValue DollarValue { get; private set; }

        public event Action<DollarValue> DollarValueChanged;

        public event Action<int> IncomeChanged;
        public event Action<int> ExpensesChanged;

        private void OnEnable()
        {
            _tutorDescriptionUI.TutorialCompleted += AddPrizeTutorialMoney;
        }

        private void OnDisable()
        {
            _tutorDescriptionUI.TutorialCompleted -= AddPrizeTutorialMoney;
        }

        private void Start()
        {
            var localis = LocalizationManager.CurrentLanguage;
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!localis " + localis);

            LoadDollarValue();
            // DollarValue = new DollarValue(100, 10);
            DollarValueChanged.Invoke(DollarValue);
        }

        public void AddTest()
        {
            Add(new DollarValue(1003, 65));
        }

        public void SubtractTest()
        {
            Subtract(new DollarValue(135, 06));
        }

        public void Add(DollarValue other)
        {
            int totalCents = ToTotalCents(DollarValue) + ToTotalCents(other);
            DollarValue = FromTotalCents(totalCents);
            DollarValueChanged.Invoke(DollarValue);
            _flyValue.ShowFly(other, true);
            IncomeChanged?.Invoke(ToTotalCents(other));
            SaveDollarValue();
        }

        public void Subtract(DollarValue other)
        {
            int totalCents = ToTotalCents(DollarValue) - ToTotalCents(other);

            if (totalCents <= 0)
                totalCents = 0;

            DollarValue = FromTotalCents(totalCents);
            DollarValueChanged.Invoke(DollarValue);
            _flyValue.ShowFly(other, false);
            ExpensesChanged?.Invoke(ToTotalCents(other));
            SaveDollarValue();
        }

        public int ToTotalCents(DollarValue dollarValue)
        {
            return dollarValue.Dollars * 100 + dollarValue.Cents;
        }

        public DollarValue FromTotalCents(int totalCents)
        {
            int dollars = totalCents / 100;
            int cents = totalCents % 100;
            return new DollarValue(dollars, cents);
        }

        private void SaveDollarValue()
        {
            PlayerPrefs.SetInt("DollarValue_Dollars", DollarValue.Dollars);
            PlayerPrefs.SetInt("DollarValue_Cents", DollarValue.Cents);
            PlayerPrefs.Save();
        }

        private void LoadDollarValue()
        {
            if (PlayerPrefs.HasKey("DollarValue_Dollars") && PlayerPrefs.HasKey("DollarValue_Cents"))
            {
                int dollars = PlayerPrefs.GetInt("DollarValue_Dollars");
                int cents = PlayerPrefs.GetInt("DollarValue_Cents");

                if (dollars <= 0)
                    dollars = 0;

                if (cents <= 0)
                    cents = 0;

                DollarValue = new DollarValue(dollars, cents);
            }
            else
            {
                DollarValue = new DollarValue(25, 00);
            }
        }

        private void AddPrizeTutorialMoney()
        {
            Add(new DollarValue(100, 00));
        }
    }
}