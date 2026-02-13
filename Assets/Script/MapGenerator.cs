using UnityEngine;
using Unity.AI.Navigation; // Needed for NavMesh

public class MapGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public GameObject wallPrefab;
    public NavMeshSurface navMeshSurface;

    [Tooltip("How many blocks wide the map is")]
    public int width = 20;
    public int depth = 20;

    [Tooltip("Size of your wall cube (usually 2)")]
    public float cellSize = 2f;

    [Range(0f, 1f)]
    [Tooltip("0 = Empty Field, 1 = Full Blocks")]
    public float wallDensity = 0.3f; // 30% of the map will be walls

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // 1. Loop through the Grid (X and Z axis)
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int z = -depth / 2; z < depth / 2; z++)
            {
                // Calculate the exact position for this block
                Vector3 pos = new Vector3(x * cellSize, 1f, z * cellSize);

                // 2. SAFETY CHECK: Keep the center empty!
                // If the block is too close to (0,0,0), skip it so the player isn't stuck.
                if (Vector3.Distance(pos, Vector3.zero) < 4f)
                {
                    continue;
                }

                // 3. Dice Roll: Should we place a wall here?
                if (Random.value < wallDensity)
                {
                    // Spawn the wall
                    GameObject newWall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);

                    // Randomize rotation slightly for visual variety (Optional)
                    // newWall.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                }
            }
        }

        // 4. Update the AI Pathfinding
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}