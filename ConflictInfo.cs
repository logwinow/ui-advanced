using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConflictInfo
{
    public ConflictInfo(UIWindow window, SolveConflictBehaviour solveConflictBehaviour)
    {
        Window = window;
        SolveConflictBehaviour = solveConflictBehaviour;
    }
    
    [field: SerializeField]
    public UIWindow Window { get; private set; }
    [field: SerializeField]
    public SolveConflictBehaviour SolveConflictBehaviour { get; private set; }
}
