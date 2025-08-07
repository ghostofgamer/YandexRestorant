using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectChanger : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    
    private void OnEnable()
    {
        if (_scrollRect != null)
            _scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    private IEnumerator Start()
    {
        yield return null;
        
        if (_scrollRect != null)
            _scrollRect.normalizedPosition = new Vector2(0, 1);
    }
}