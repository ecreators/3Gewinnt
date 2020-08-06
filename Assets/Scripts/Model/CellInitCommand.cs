using UnityEngine;

public class CellInitCommand : GameCommand<CellInitParameters>
{
    public override void Execute(CellInitParameters parameter)
    {
        DeleteContentFromCell(parameter.ZelleCopy);

        if (parameter.PoolData.Length < 0)
        {
            return;
        }

        ContentCreationConfig prefab = GetRandomPoolContent(parameter);

        if (prefab == null)
        {
            return;
        }

        Vector3 position = parameter.CellPosition;
        ZellInhaltComponent prefabCopy = Object.Instantiate(prefab.contentTypePrefab, position, Quaternion.identity, parameter.ZelleCopy.transform);
        prefabCopy.Cell = parameter.ZelleCopy;
        parameter.TriggerContentCreatedInCell(prefabCopy);
    }

    private static void DeleteContentFromCell(ZelleComponent zelle)
    {
        var content = zelle.GetComponentInChildren<ZellInhaltComponent>();
        if (content)
        {
            Object.Destroy(content.gameObject);
        }
    }

    private ContentCreationConfig GetRandomPoolContent(CellInitParameters parameter)
    {
        bool notUnique;
        ContentCreationConfig prefab;
        do
        {
            prefab = RandomPrefabFrom(parameter.PoolData);
            bool isUnique = CheckUniqueType(parameter.ZelleCopy.Index, parameter.FindCellsWithContentType(prefab.ContentType));
            notUnique = isUnique == false;
        } while (notUnique);
        return prefab;
    }

    private ContentCreationConfig RandomPrefabFrom(ContentCreationConfig[] poolData)
    {
        return poolData[RandomIndex(poolData)];
    }

    private static int RandomIndex(object[] array)
    {
        int maxIndex = array.Length - 1;
        return Random.Range(0, maxIndex);
    }

    private bool CheckUniqueType((int column, int row) index, (int column, int row)[] cellsWithContentType)
    {
        foreach ((int column, int row) in cellsWithContentType)
        {
            if (column == index.column && (row == index.row || row + 1 == index.row || row - 1 == index.row))
            {
                return false;
            }

            if (row == index.row && (column == index.column || column + 1 == index.column || column - 1 == index.column))
            {
                return false;
            }
        }

        return true;
    }
}