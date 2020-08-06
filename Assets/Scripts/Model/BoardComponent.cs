using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BoardComponent : MonoBehaviour
{
    public int spalten = 10;
    public int zeilen = 10;
    public ZelleComponent zelleVorlage;
    public ContentCreationConfig[] poolData;
    public bool contentErstBeiStart = true;

    public static readonly BoardGeneratorCommand boardCreationCommand = new BoardGeneratorCommand();
    public static readonly CellInitCommand cellInitCommand = new CellInitCommand();

    public void BuildCells()
    {
        boardCreationCommand.Execute(new BoardParameters
        {
            Spalten = spalten,
            Zeilen = zeilen,
            Abstand = 0.1f,
            TransformParent = transform,
            ZelleVorlage = zelleVorlage,
            OnCellCreatedHandler = this.OnCellCreated
        });

        Debug.Log($"Alle Zellen ({spalten * zeilen}) erstellt");
    }

    private void OnCellCreated(ZelleComponent newCell)
    {
        Debug.Log($"Zelle erstellt in Coordinate {newCell.Index}");

        newCell.ContentReachedCellEvent += EvaluateMatchOfCellContentInCell;

        if (contentErstBeiStart)
        {
            return;
        }

        CreateContentInCell(newCell);
    }

    private void EvaluateMatchOfCellContentInCell(ZellInhaltComponent contentInCellNotMoving)
    {
        // TODO - test match!
    }

    private void CreateContentInCell(ZelleComponent newCell)
    {
        StartCoroutine(CreateContentInCell(new CellInitParameters
        {
            PoolData = poolData,
            ZelleCopy = newCell,
            FindCellWithContentTypeHandler = FindCellsWithContentType,
            OnCellContentCreatedHandler = OnCellContentCreated
        }));
    }

    private void OnCellContentCreated(ZellInhaltComponent content)
    {
        Debug.Log($"Content erstellt in Zelle {content.Cell.Index}");
    }

    private (int column, int row)[] FindCellsWithContentType(EContentType contentType)
    {
        return (from content in FindObjectsOfType<ZellInhaltComponent>()
                where content.contentType == contentType
                select content.Cell.Index).ToArray();
    }

    private IEnumerator CreateContentInCell(CellInitParameters cellInitParameters)
    {
        yield return new WaitUntil(() =>
        {
            cellInitCommand.Execute(cellInitParameters);
            return true;
        });
    }

    // Use this for initialization
    void Start()
    {
        if (contentErstBeiStart)
        {
            StartCoroutine(CreateContentInCells());
        }
    }

    private IEnumerator CreateContentInCells()
    {
        foreach (var child in transform)
        {
            if (child is MonoBehaviour mb && mb.TryGetComponent(out ZelleComponent zelle))
            {
                CreateContentInCell(zelle);
            }
            yield return null;
        }
    }
}