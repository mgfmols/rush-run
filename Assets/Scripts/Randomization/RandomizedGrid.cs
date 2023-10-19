using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GridRow
{

    public int rowNumber;
    public Vector3 rowStartPosition;
    public List<GridEntry> gridEntries;

    public GridRow(int rowNumber, Vector3 rowStartPosition, List<GridEntry> gridEntries)
    {
        this.rowNumber = rowNumber;
        this.rowStartPosition = rowStartPosition;
        this.gridEntries = gridEntries;
    }
}

[System.Serializable]
public class GridEntry
{
    public int gridNumber;
    public Vector3 gridEntryPosition;
    public RandomizedObject randomizedObject;

    public GridEntry(int gridNumber, Vector3 gridEntryPosition)
    {
        this.gridNumber = gridNumber;
        this.gridEntryPosition = gridEntryPosition;
    }
}

[System.Serializable]
public class ObjectType
{
    public Form form;
    public int spawnChance;
    public GameObject gameObject;
}

public class RandomizedGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] int amountOfRows = 5;
    [SerializeField] int rowLength = 5;
    [SerializeField] int gridEntrySize = 30;
    [Space]
    [SerializeField] int seed;
    [Space]
    [SerializeField] List<ObjectType> spawnChances;
    [Header("Data")]
    [SerializeField] List<GridRow> gridRows = new List<GridRow>();

    public void Awake()
    {
        for(int i = 1; i < spawnChances.Count; i++)
        {
            spawnChances[i].spawnChance += spawnChances[i - 1].spawnChance; 
        }
        SpawnGrid();
        FinishGrid();
    }


    /// Spawn the grid
    private void SpawnGrid()
    {
        /// Set seed
        if (seed == 0)
        {
            seed = Random.Range(1000000, 9999999);
        }

        /// position - new Vector3(62, 0, 62) = Bottom left grid entry
        Vector3 rowPosition = transform.position - new Vector3(gridEntrySize * (rowLength - 1f) / 2, 0, gridEntrySize * (amountOfRows - 1f) / 2);
        for (int i = 0; i < amountOfRows; i++)
        {
            switch (i)
            {
                default:
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    GridRow row = new GridRow(i, rowPosition, new List<GridEntry>());
                    /// Check to make sure every row has at least 1 non empty entry
                    List<GridEntry> entriesInThisRow = new List<GridEntry>();
                    bool containsNonEmpty = false;
                    while (!containsNonEmpty)
                    {
                        //Debug.Log($"Making items for row {i}");
                        /// Clear each time the while loop is repeated
                        Vector3 gridEntryPosition = rowPosition;
                        foreach(GridEntry entry in entriesInThisRow)
                        {
                            Destroy(entry.randomizedObject.gameObject);
                        }
                        row.gridEntries.Clear();
                        entriesInThisRow.Clear();
                        /// For every item out of a row, spawn a new entry
                        for (int j = 0; j < rowLength; j++)
                        {
                            GridEntry toBeAdded = SpawnGridEntry(i, j, gridEntryPosition);
                            row.gridEntries.Add(toBeAdded);
                            entriesInThisRow.Add(toBeAdded);
                            gridEntryPosition += new Vector3(gridEntrySize, 0, 0);
                        }
                        /// Check for each entry if a row if it contains a non empty row
                        foreach (GridEntry entry in entriesInThisRow)
                        {
                            if (!entry.randomizedObject.form.Equals(Form.Empty))
                            {
                                /// This exits the loop
                                containsNonEmpty = true;
                            }
                        }
                    }
                    gridRows.Add(row);
                    break;
            }
            rowPosition += new Vector3(0, 0, gridEntrySize);
        }
    }


    /// <summary>
    /// Finish up the grid
    /// </summary>
    private void FinishGrid()
    {
        foreach(GridRow row in gridRows)
        {
            foreach (GridEntry entry in row.gridEntries)
            {
                /// Check surrounding entries
                GridEntry comparison;
                /// Checking GridEntry north
                if (!(gridRows.IndexOf(row) + 1 >= gridRows.Count))
                {
                    comparison = gridRows[gridRows.IndexOf(row) + 1].gridEntries[row.gridEntries.IndexOf(entry)];
                    CheckEdges(entry, comparison, Direction.NORTH);
                }
                /// Checking GridEntry east
                if (!(row.gridEntries.IndexOf(entry) + 1 >= row.gridEntries.Count))
                {
                    comparison = row.gridEntries[row.gridEntries.IndexOf(entry) + 1];
                    CheckEdges(entry, comparison, Direction.EAST);
                }
                /// Checking GridEntry south
                if (!(gridRows.IndexOf(row) - 1 < 0))
                {
                    comparison = gridRows[gridRows.IndexOf(row) - 1].gridEntries[row.gridEntries.IndexOf(entry)];
                    CheckEdges(entry, comparison, Direction.SOUTH);
                }
                /// Checking GridEntry south
                if (!(row.gridEntries.IndexOf(entry) - 1 < 0))
                {
                    comparison = row.gridEntries[row.gridEntries.IndexOf(entry) - 1];
                    CheckEdges(entry, comparison, Direction.WEST);
                }
            }
        }
    }

    private void CheckEdges(GridEntry target, GridEntry comparison, Direction targetDirection)
    {
        if (target == null || comparison == null)
        {
            Debug.LogError("An edge is null");
            return;
        }
        List<RemovableEdge> edgesOfTarget = target.randomizedObject.removableEdges;
        List<RemovableEdge> edgesOfComparison = comparison.randomizedObject.removableEdges;
        //string targetCode = $"n{target.gridNumber}f{target.randomizedObject.form.ToString()[0]}";
        //string comparisonCode = $"n{comparison.gridNumber}f{comparison.randomizedObject.form.ToString()[0]}";
        //string debugMessage = $"Checking {targetCode} against {comparisonCode}";
        /// Searching for correct edge to check for
        foreach (RemovableEdge targetEdge in edgesOfTarget)
        {
            if (targetEdge.direction.Equals(targetDirection))
            {
                /// Setting edge to look and check for
                Direction oppositeOfDirection = targetDirection + 2;
                oppositeOfDirection = RemovableEdge.LoopDirectionIfNecessary(oppositeOfDirection);
                /// Searching for an edge in that right direction
                foreach (RemovableEdge comparisonEdge in edgesOfComparison)
                {
                    //debugMessage += $"\nChecking {targetCode}({targetDirection}) against {comparisonCode}({comparisonEdge.direction})";
                    if (comparisonEdge.direction.Equals(oppositeOfDirection))
                    {
                        target.randomizedObject.removeEdge(targetDirection);
                        comparison.randomizedObject.removeEdge(comparisonEdge.direction);
                        //debugMessage += " - Success";
                        //Debug.Log(debugMessage);
                        return;
                    }
                    else
                    {   
                        //debugMessage += " - Fail";
                    }
                }
            }
        }
    }

    public GridEntry SpawnGridEntry(int i, int j, Vector3 gridEntryPosition)
    {
        /// Create object
        int gridNumber = i * rowLength + j;
        GridEntry toBeAdded = new GridEntry(gridNumber, gridEntryPosition);
        System.Random random = new System.Random(System.Convert.ToInt32(System.Convert.ToDouble(gridNumber) / System.Convert.ToDouble(seed) * 10000000));
        /// Determine random selected object
        float randomObjectPick = random.Next(0, spawnChances[4].spawnChance);
        GameObject toBeCopied = spawnChances.Aggregate((x, y) => System.Math.Abs(x.spawnChance - randomObjectPick) < System.Math.Abs(y.spawnChance - randomObjectPick) ? x : y).gameObject;

        /// Add necessary spawning info
        int amountToRotate = random.Next(0, 4);
        int rotation = amountToRotate * 90;

        /// Spawn object 
        GameObject copied = Instantiate(toBeCopied, gridEntryPosition, Quaternion.Euler(0, rotation, 0), gameObject.transform);

        /// Rotate edges in object
        RandomizedObject randomizedObject = copied.GetComponent<RandomizedObject>();
        randomizedObject.rotateEdges(amountToRotate);

        /// Determine randomized addons
        List<GameObject> environmentAddons = randomizedObject.environmentAddons;
        int amountOfAddons = random.Next(0, environmentAddons.Count());
        for(int k = 0; k < amountOfAddons; k++)
        {
            environmentAddons[random.Next(0, environmentAddons.Count())].SetActive(true);
        }

        /// Set object in GridEntry
        toBeAdded.randomizedObject = copied.GetComponent<RandomizedObject>();
        return toBeAdded;
    }

    /// <summary>
    ///  For each location of the grid, place a yellow and red gizmo
    /// </summary>
    public void OnDrawGizmos()
    {
        Vector3 rowPosition = transform.position - new Vector3(gridEntrySize*(rowLength - 1f) / 2, 0, gridEntrySize * (amountOfRows - 1f) / 2);
        for(int i = 0; i < amountOfRows; i++)
        {
            Vector3 boxPosition = rowPosition;
            for (int j = 0; j < rowLength; j++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(boxPosition, new Vector3(gridEntrySize, 1, gridEntrySize));
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(boxPosition, new Vector3(gridEntrySize + 2.01f, 1, gridEntrySize + 2.01f));
                boxPosition += new Vector3(gridEntrySize, 0, 0);
            }
            rowPosition += new Vector3(0, 0, gridEntrySize);
        }
    }
}
