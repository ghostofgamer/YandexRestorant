using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TransferItems : MonoBehaviour
{
    public void TransferJumpListItems(int value, GameObject[] objects, Transform[] targetPositions, Action callback)
    {
        List<GameObject> activeItems = objects.Where(p => p.activeSelf).ToList();

        if (value > activeItems.Count)
            value = activeItems.Count;

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < value; i++)
        {
            int index = i;
            
            if (index < activeItems.Count && index < targetPositions.Length)
            {
                Debug.Log("Прыгнул " + activeItems[index].gameObject.name);
                sequence.Join(activeItems[index].transform.DOJump(
                    targetPositions[index].position, 1.65f, 1, 1f).SetEase(Ease.InOutQuad));
            }
        }

        sequence.OnComplete(() => { callback?.Invoke(); });

        /*Sequence sequence = DOTween.Sequence();

        sequence.Append(currentObject.transform.DOMove(targetPos.position, 0.5f)
            .SetEase(Ease.InOutQuad));

        sequence.Join(currentObject.transform
                .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear))
            .OnComplete(() => callback?.Invoke());*/
    }

    public void TransferToTray(GameObject currentObject,Transform targetPosition,Action callback)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(currentObject.transform.DOScale(1.15f, 0.3f).SetEase(Ease.InOutQuad));
        sequence.Append(currentObject.transform.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuad));
        sequence.Append(currentObject.transform.DOMove(targetPosition.position, 0.5f)
            .SetEase(Ease.InOutQuad));

        currentObject.transform.SetParent(targetPosition);

        sequence.Join(currentObject.transform
            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear));
        
        sequence.OnComplete(() => { callback?.Invoke(); });
    }
}