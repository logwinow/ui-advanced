using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UIWindow))]
public sealed class UIConflictBehaviour : MonoBehaviour
{
    [SerializeField] private bool _conflictWithAll;
    [SerializeField] private SolveConflictBehaviour _solveConflictBehaviour;
    [SerializeField] private ConflictInfo[] _exclusionConflicts;
    
    [SerializeField] private ConflictInfo[] _conflicts;

    private UIWindow _window;
    private ConflictInfo[] _savedConflicts;

    private void Awake()
    {
        _window = GetComponent<UIWindow>(); 
        _window.OnOpenEvent.AddListener(OnUIWindowOpened);
        _window.OnCloseEvent.AddListener(OnUIWindowClosed);
    }

    private void OnUIWindowOpened()
    {
        if (_conflictWithAll)
        {
            _savedConflicts = UIBrain.Instance.ResolveConflictsWithActive(_solveConflictBehaviour,
                _exclusionConflicts
                    .Select(c => c.Window)
                    .Union(new[] { _window })
                    .ToArray()
                ).Union(UIBrain.Instance.ResolveConflicts(_exclusionConflicts))
                .ToArray();
        }
        else
        {
            _savedConflicts = UIBrain.Instance.ResolveConflicts(_conflicts);
        }
    }

    private void OnUIWindowClosed()
    {
        if (_savedConflicts == null || _savedConflicts.Length == 0)
            return;

        foreach (var savedWindow in _savedConflicts.Select(conflict => conflict.Window))
        {
            savedWindow.Open();
        }
        
        _savedConflicts = null;
    }
}
