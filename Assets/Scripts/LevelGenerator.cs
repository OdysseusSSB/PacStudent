using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] OldObjects;
    [SerializeField]
    private GameObject PowerUpPrefab;

    private int[,] levelMap =
        {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };

    [SerializeField]
    private GameObject tilePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < OldObjects.Length; i++)
        {
            Destroy(OldObjects[i]);
        }

        levelMap = MirrorLevelMap(levelMap);
        Debug.Log(levelMap.GetLength(0) + " " + levelMap.GetLength(1));
        int[,] rotations = GetRotations(levelMap);
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        // Place Tiles
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Debug.Log("Tile at (" + i + ", " + j + "): Value = " + levelMap[i, j] + ", Rotation = " + rotations[i, j]);
                GameObject tile = Instantiate(tilePrefab, new Vector3(j, -i, 0), Quaternion.Euler(0, 0, rotations[i, j] * 90));
                tile.GetComponent<Animator>().SetInteger("Type", levelMap[i, j]);
                if (levelMap[i, j] == 6)
                {
                    Instantiate(PowerUpPrefab, new Vector3(j, -i, 0), Quaternion.identity);
                }
            }
        }
    }

    int[,] MirrorLevelMap(int[,] originalMap)
    {
        int rows = originalMap.GetLength(0);
        int cols = originalMap.GetLength(1);
        int[,] mirroredMap = new int[rows * 2 - 1, cols * 2];

        for (int i = 0; i < rows; i++)
        {
            for (int j = cols; j < cols * 2 - 1; j++)
            {
                mirroredMap[i, j] = originalMap[i, cols - 1 - (j - cols)];
            }
        }
        for (int i = rows; i < rows * 2 - 1; i++)
        {
            for (int j = 0; j < cols * 2 - 1; j++)
            {
                mirroredMap[i, j] = mirroredMap[rows * 2 - 2 - i, j];
            }
        }
        return mirroredMap;
    }

    int[,] GetRotations(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        int[,] rotations = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int tileValue = map[i, j];
                // Rule for Corner Walls
                if (tileValue == 1 || tileValue == 3)
                {
                    if (i - 1 >= 0 && IsWall(map[i - 1, j]))
                    // There is a wall above
                    {
                        rotations[i, j] = 1;
                        if (j + 1 < cols && IsWall(map[i, j + 1]))
                        // There is a wall to the right
                        {
                            rotations[i, j] = 0;
                        }
                        else
                        // There is a wall to the left
                        {
                            rotations[i, j] = 1;
                        }
                    }
                    else
                    // There is a wall below
                    {
                        rotations[i, j] = 3;
                        if (j + 1 < cols && IsWall(map[i, j + 1]))
                        // There is a wall to the right
                        {
                            rotations[i, j] = 3;
                        }
                        else
                        // There is a wall to the left
                        {
                            rotations[i, j] = 2;
                        }
                    }
                }
                else if (tileValue == 2 || tileValue == 4 || tileValue == 8)
                // Rule for Outside Straight Wall
                {
                    if (i - 1 >= 0 && IsWall(map[i - 1, j]) && i + 1 < rows && IsWall(map[i + 1, j]))
                    // There is a wall above and below
                    {
                        rotations[i, j] = 0;
                    }
                    else
                    // There is a wall beside
                    {
                        rotations[i, j] = 1;
                    }
                }
                else if (tileValue == 7)
                // Rule for Junction
                {
                    if (i - 1 < 0 || !IsWall(map[i - 1, j]))
                    // There is no wall above
                    {
                        rotations[i, j] = 0;
                    }
                    else if (i + 1 >= rows || !IsWall(map[i + 1, j]))
                    // There is no wall below
                    {
                        rotations[i, j] = 2;
                    }
                    else if (j - 1 < 0 || !IsWall(map[i, j - 1]))
                    // There is no wall to the left
                    {
                        rotations[i, j] = 3;
                    }
                    else
                    // There is a wall to the right
                    {
                        rotations[i, j] = 1;
                    }
                }
            }
                
        }
        return rotations;
    }

    bool IsWall(int tileValue)
    {
        return tileValue == 1 || tileValue == 2 || tileValue == 3 || tileValue == 4 || tileValue == 7 || tileValue == 8;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
