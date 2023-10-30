using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIWindow))]
public class UIOnStart : MonoBehaviour
{
    [SerializeField] private DoOnStart _doOnStart = DoOnStart.Close;
    
    private enum DoOnStart
    {
        Close, Open
    }
    
    private void Start()
    {
        var window = GetComponent<UIWindow>();
        
        switch (_doOnStart)
        {
            case DoOnStart.Close:
                window.Close();
                break;
            case DoOnStart.Open:
                window.Open();
                break;
            default:
                Debug.LogWarning($"Unknown {nameof(DoOnStart)} value: '{_doOnStart}'");
                break;
        }
    }
}
