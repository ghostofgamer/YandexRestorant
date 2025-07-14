using UnityEngine;

namespace SettingsContent.SoundContent
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _wheelNeedle;
        [SerializeField] private AudioClip _dailyReward;
        [SerializeField] private AudioClip _error;
        [SerializeField] private AudioClip _payment;
        [SerializeField] private AudioClip _levelUp;
        [SerializeField] private AudioClip _refillDrinkMachine;
        [SerializeField] private AudioClip _pourDrink;
        [SerializeField] private AudioClip _cashRegister;
        [SerializeField] private AudioClip _grillWell;
        [SerializeField] private AudioClip _putTray;
        [SerializeField] private AudioClip _pickUp;
        [SerializeField] private AudioClip _throw;
        [SerializeField] private AudioClip _coins;
        [SerializeField] private AudioClip _bills;
        [SerializeField] private AudioClip _dostavka;
        [SerializeField] private AudioClip _fortunaReward;
        [SerializeField] private AudioClip _mysteryBox;

        public static SoundPlayer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayButtonClick()
        {
            _audioSource.PlayOneShot(_buttonClick);
        }

        public void PlayWheelFortune()
        {
            _audioSource.PlayOneShot(_wheelNeedle);
        }

        public void PlayDailyReward()
        {
            _audioSource.PlayOneShot(_dailyReward);
        }

        public void PlayError()
        {
            _audioSource.PlayOneShot(_error);
        }

        public void PlayPayment()
        {
            _audioSource.PlayOneShot(_payment);
        }

        public void PlayLevelUp()
        {
            _audioSource.PlayOneShot(_levelUp);
        }

        public void PlayRefillDrinksMachine()
        {
            _audioSource.PlayOneShot(_refillDrinkMachine);
        }

        public void PlayPourDrink()
        {
            _audioSource.PlayOneShot(_pourDrink);
        }

        public void PlayCashRegister()
        {
            _audioSource.PlayOneShot(_cashRegister);
        }

        public void PlayGrillWell()
        {
            _audioSource.PlayOneShot(_grillWell);
        }

        public void PlayPutTray()
        {
            _audioSource.PlayOneShot(_putTray);
        }

        public void PlayPickUp()
        {
            _audioSource.PlayOneShot(_pickUp);
        }

        public void PlayThrow()
        {
            _audioSource.PlayOneShot(_throw);
        }

        public void PlayCoins()
        {
            _audioSource.PlayOneShot(_coins);
        }

        public void PlayBills()
        {
            _audioSource.PlayOneShot(_bills);
        }

        public void PlayDostavka()
        {
            _audioSource.PlayOneShot(_dostavka);
        }

        public void PlayFortunePrize()
        {
            _audioSource.PlayOneShot(_fortunaReward);
        }

        public void PlayMysteryBoxPrize()
        {
            _audioSource.PlayOneShot(_mysteryBox);
        }
    }
}