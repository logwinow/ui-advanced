using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWindow : MonoBehaviour
{
    private bool _initialized;
    
    [SerializeField] private bool _blockOthers;
    [SerializeField] private bool _ignoreBlock;
    
    public UnityEvent OnOpenEvent;
    public UnityEvent OnCloseEvent;

    public bool BlockOthers => _blockOthers;
    public bool IgnoreBlock => _ignoreBlock;
    public bool IsOpened => gameObject.activeSelf;

    protected virtual bool ValidateOpening() 
    {
        return true;
    }

    protected void Initialize()
    {
        if (_initialized)
            return;
        
        OnInitialize();

        _initialized = true;
    }
    
    protected virtual void OnInitialize() {}
    
    public void Open()
    {
        Initialize();
    
        if (!ValidateOpening())
            return;

        if (!UIBrain.Instance.TryRegisterWindow(this))
            return;
        
        gameObject.SetActive(true);
        
        OnOpen();
        
        OnOpenEvent.Invoke();
    }
    
    protected virtual void OnOpen() {}

    public void Close()
    {
        UIBrain.Instance.UnregisterWindow(this);
        
        gameObject.SetActive(false);

        OnClose();
        
        OnCloseEvent.Invoke();
    }
    
    protected virtual void OnClose() {}

    public virtual void SetActive(bool isActive)
    {
        if (isActive)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void SwitchActive()
    {
        SetActive(!gameObject.activeSelf);
    }
}
