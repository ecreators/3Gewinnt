using System;
using UnityEngine;

public class BoardGeneratorCommand : GameCommand<BoardParameters>
{
    public override void Execute(BoardParameters parameter)
    {
        RemoveAllCells(parameter.TransformParent);

        CreateCells(parameter);
    }

    private void CreateCells(BoardParameters parameter)
    {
        var prefab = parameter.ZelleVorlage;
        var width = prefab.transform.localScale.x;
        var height = prefab.transform.localScale.y;
        var totalWidth = width * parameter.Spalten + parameter.Abstand * (parameter.Spalten - 1);
        var totalHeight = height * parameter.Zeilen + parameter.Abstand * (parameter.Zeilen - 1);

        for (int row = parameter.Zeilen - 1; row >= 0; row--)
        {
            for (int column = 0; column < parameter.Spalten; column++)
            {
                (int column, int row) index = (column, row);
                float x = column * (width + parameter.Abstand) - totalWidth / 2;
                float y = row * (height + parameter.Abstand) - totalHeight / 2;
                var position = new Vector3(x, y);
                ZelleComponent copyCell = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, parameter.TransformParent).GetComponent<ZelleComponent>();
                copyCell.Index = index;

                parameter.TriggerCellCreated(copyCell);
            }
        }
    }

    private static void RemoveAllCells(Transform transformBoard)
    {
        foreach (var child in transformBoard)
        {
            if (child is GameObject go && go.TryGetComponent(out ZelleComponent zelle))
            {
                Delete(zelle);
            }
        }
    }
}