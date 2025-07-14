using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialContent
{
    public class TutorialAssemblyBurger : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        [SerializeField] private Collider _bunContainer;
        [SerializeField] private Collider _cutletContainer;
        [SerializeField] private Collider _ketchupCollider;
        [SerializeField] private Collider _mustardCollider;


        [SerializeField] private GameObject _rawCutlet;
        [SerializeField] private GameObject _rawBun;
        [SerializeField] private GameObject _rawPackage;
        [SerializeField] private GameObject _rawKetchup;

        [SerializeField] private TutorialObject _assemblyTable;

        public void StartTutorAssemblyBurger()
        {
            StartCoroutine(StartFirstBurger());
        }

        private IEnumerator StartFirstBurger()
        {
            yield return new WaitForSeconds(0.1f);

            _assemblyTable.DeactivateTutorPoint();
            _closeButton.interactable = false;
            _mustardCollider.enabled = false;
            _cutletContainer.enabled = false;
            _ketchupCollider.enabled = false;
            _rawBun.SetActive(true);
        }

        public void NextItemCutlet()
        {
            _rawBun.SetActive(false);
            _rawCutlet.SetActive(true);
            _bunContainer.enabled = false;
            _cutletContainer.enabled = true;
        }

        public void NextItemKetchup()
        {
            _rawCutlet.SetActive(false);
            _rawKetchup.SetActive(true);
            _cutletContainer.enabled = false;
            _ketchupCollider.enabled = true;
        }

        public void NextItemBunTop()
        {
            _rawKetchup.SetActive(false);
            _ketchupCollider.enabled = false;
            _bunContainer.enabled = true;
            _rawBun.SetActive(true);
        }

        public void NextItemPackages()
        {
            _bunContainer.enabled = false;
            _rawBun.SetActive(false);
            _rawPackage.SetActive(true);
        }

        public void CompletedAssemblyBurger()
        {
            _closeButton.interactable = true;
            _mustardCollider.enabled = true;
            _bunContainer.enabled = true;
            _cutletContainer.enabled = true;
            _ketchupCollider.enabled = true;
            _rawPackage.SetActive(false);
        }
    }
}