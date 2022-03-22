using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition 
{
    public string Name{ get; set; }
    public string Description{ get; set; }
    public string StartMessage{ get; set; }

    public Func<Monster, bool> OnBeforeMove { get; set; }
    public Func<Monster, bool> OnFocusTarget { get; set; }
    public Action<Monster> OnAfterTurn { get; set; }
    public Action<Monster> OnStart { get; set; }


}
