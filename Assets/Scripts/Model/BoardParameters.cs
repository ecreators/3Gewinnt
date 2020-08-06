using System;
using UnityEngine;

public class BoardParameters
{
    public int Spalten { get; set; }
    public int Zeilen { get; set; }
    public float Abstand { get; set; }
    public ZelleComponent ZelleVorlage { get; set; }
    public Transform TransformParent { get; set; }

    public Action<ZelleComponent> OnCellCreatedHandler { get; set; }

    public void TriggerCellCreated(ZelleComponent copyCell)
    {
        OnCellCreatedHandler?.Invoke(copyCell);
    }
}