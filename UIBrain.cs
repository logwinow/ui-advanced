using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBrain : SingletonMono<UIBrain>
{
    [SerializeField] private bool _throwExceptions = false;
    private List<UIWindow> _activeWindows = new List<UIWindow>();

    private UIWindow _blockOn;

    public List<UIWindow> ActiveWindows => _activeWindows;

    public bool TryRegisterWindow(UIWindow window)
    {
        if (_activeWindows.Contains(window))
        {
            if (_throwExceptions)
                throw new Exception($"Try to register the same window {window.name}");

            return false;
        }

        // TODO: If window is a child of _blockOn, than it could be opened
        if (_blockOn)
        {
            if (!window.IgnoreBlock)
                return false;
        }

        if (window.BlockOthers)
            _blockOn = window;

        _activeWindows.Add(window);

        return true;
    }

    public void UnregisterWindow(UIWindow window)
    {
        if (!_activeWindows.Contains(window))
        {
            if (_throwExceptions)
                throw new Exception($"Try to unregister unregistered window {window.name}");

            return;
        }

        _activeWindows.Remove(window);

        if (window.BlockOthers && window == _blockOn)
            _blockOn = null;
    }

    private ConflictInfo ResolveConflict(ConflictInfo conflictInfo)
    {
        switch (conflictInfo.SolveConflictBehaviour)
        {
            case SolveConflictBehaviour.DoNothing:
                return null;
            case SolveConflictBehaviour.Close:
                conflictInfo.Window.Close();
                return null;
            case SolveConflictBehaviour.Hide:
                conflictInfo.Window.Close();
                return conflictInfo;
            default:
                Debug.LogWarning($"Unknown {nameof(SolveConflictBehaviour)} value: '{conflictInfo.SolveConflictBehaviour}'");
                break;
        }

        return null;
    }

    public ConflictInfo[] ResolveConflictsWithActive(SolveConflictBehaviour solveConflictBehaviour, params UIWindow[] unaffectedWindows)
    {
        return ResolveConflicts(
            (from window in _activeWindows.Except(unaffectedWindows)
                select new ConflictInfo(window, solveConflictBehaviour)).ToArray());
    }

    public ConflictInfo[] ResolveConflicts(params ConflictInfo[] conflicts)
    {
        return (from conflict in conflicts
                let savedConflict = ResolveConflict(conflict)
                where savedConflict != null
                select conflict)
            .ToArray();
    }
}
