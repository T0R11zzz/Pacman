using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    public GameObject emptyPrefab;
    public GameObject outerCornerPrefab; 
    public GameObject outerWallPrefab;   
    public GameObject innerCornerPrefab;  
    public GameObject innerWallPrefab;  
    public GameObject pelletPrefab;      
    public GameObject powerPelletPrefab;  
    public GameObject tJunctionPrefab;

    int[,] levelMap =
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
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    int width;
    int height;
    Vector2 baseOffset;

    // Start is called before the first frame update
    void Start()
    {
        width = levelMap.GetLength(1);
        height = levelMap.GetLength(0);
        baseOffset = new Vector2(-width / 2f, height / 2f);

        GameObject existingLevel = GameObject.Find("Level01");
        if (existingLevel != null)
        {
            Destroy(existingLevel);
        }
        GenerateNewLevel();
        MirrorLevel();
        MainCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNewLevel()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap[y, x];
                GameObject tilePrefab = GetTilePrefab(tileType);
                Quaternion rotation = GetTileRotation(tileType, x, y);

                if (tilePrefab != null)
                {
                    Vector2 position = new Vector2(x,-y) + baseOffset;
                    Instantiate(tilePrefab, position, rotation, transform);
                }
            }
        }

    }

    GameObject GetTilePrefab(int tileType)
    {
        switch (tileType)
        {
            case 0: return emptyPrefab;
            case 1: return outerCornerPrefab;
            case 2: return outerWallPrefab;
            case 3: return innerCornerPrefab;
            case 4: return innerWallPrefab;
            case 5: return pelletPrefab;
            case 6: return powerPelletPrefab;
            case 7: return tJunctionPrefab;
            default: return null;
        }
    }

    Quaternion GetTileRotation(int tileType, int x, int y)
    {
        if (tileType == 1 || tileType == 2)
        {
            return GetOutsideTileRotation(tileType, x, y);
        }
        else if (tileType == 3 || tileType == 4)
        {
            return GetInsideTileRotation(tileType, x, y);
        }
        return Quaternion.identity;
    }

    Quaternion GetOutsideTileRotation(int tileType,  int x, int y)
    {
        bool left = (x > 0 && (levelMap[y, x - 1] == 1 || levelMap[y, x - 1] == 2));
        bool right = (x < levelMap.GetLength(1) - 1 && (levelMap[y, x + 1] == 1 || levelMap[y, x + 1] == 2));
        bool up = (y > 0 && (levelMap[y - 1, x] == 1 || levelMap[y - 1, x] == 2));
        bool down = (y < levelMap.GetLength(0) - 1 && (levelMap[y + 1, x] == 1 || levelMap[y + 1, x] == 2));

        switch (tileType)
        {
            case 1: // Outside Corner
                if (up && right && !left && !down)
                {
                    return Quaternion.Euler(0, 0, -90);
                }
                else if (right && down && !left && !up)
                {
                    return Quaternion.Euler(0, 0, 180);
                }
                else if (down && left && !right && !up)
                {
                    return Quaternion.Euler(0, 0, 90);
                }
                return Quaternion.identity;

                case 2: // Outside Wall
                if (left && right && (!up|| !down))
                {
                    return Quaternion.identity;
                }
                else if (up && down && (!left || !right))
                {
                    return Quaternion.Euler(0, 0, 90);
                }
                return Quaternion.identity;
        }
        return Quaternion.identity;
    }

    Quaternion GetInsideTileRotation(int tileType, int x, int y)
    {
        bool left = (x > 0 && (levelMap[y, x - 1] == 3 || levelMap[y, x - 1] == 4)); 
        bool right = (x < levelMap.GetLength(1) - 1 && (levelMap[y, x + 1] == 3 || levelMap[y, x + 1] == 4)); 
        bool up = (y > 0 && (levelMap[y - 1, x] == 3 || levelMap[y - 1, x] == 4)); 
        bool down = (y < levelMap.GetLength(0) - 1 && (levelMap[y + 1, x] == 3 || levelMap[y + 1, x] == 4));

        switch (tileType)
        {
            case 3: // Inside Corner 
                if (up && right && !left && !down)
                {
                    return Quaternion.Euler(0, 0, -90);
                }
                else if (right && down && !left && !up)
                {
                    return Quaternion.Euler(0, 0, 180);
                }
                else if (down && left && !right && !up)
                {
                    return Quaternion.Euler(0, 0, 90);
                }
                return Quaternion.identity;

            case 4: // Inside Wall 
                if (left && right && (!up || !down))
                {
                    return Quaternion.identity;
                }
                else if (up && down && (!left || !right))
                {
                    return Quaternion.Euler(0, 0, 90);
                }
                return Quaternion.identity;
        }
        return Quaternion.identity;

    }

    Quaternion AdjustRotation(Quaternion originalRotation, string mirrorType)
    {
        float angle = originalRotation.eulerAngles.z;

        switch (mirrorType)
        {
            case "Horizontal":
                angle = -angle;
                break;
            case "Vertical":
                angle = 180 - angle;
                break;
            case "Both":
                angle = 180 + angle;
                break;
            default:
                break;
        }

        angle = (angle + 360) % 360;

        return Quaternion.Euler(0, 0, angle);
    }

    void MirrorLevel()
    {
        //TopRightQuadrant
        Vector2 offsetTR = baseOffset + new Vector2 (width, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap[y, x];
                GameObject tilePrefab = GetTilePrefab(tileType);
                Quaternion rotation = GetTileRotation(tileType, x, y);
                Quaternion mirroredRotation = AdjustRotation(rotation, "Horizontal");

                int mirroredX = width - 1 - x;

                if (tilePrefab != null)
                {
                    Vector2 positionTR = new Vector2(mirroredX, -y) + offsetTR;
                    Instantiate(tilePrefab, positionTR, mirroredRotation, transform);
                }
            }
        }

        // BottomLeftQuadrant
        Vector2 offsetBL = baseOffset + new Vector2(0, -height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap[y, x];
                GameObject tilePrefab = GetTilePrefab(tileType);
                Quaternion rotation = GetTileRotation(tileType, x, y);
                Quaternion mirroredRotation = AdjustRotation(rotation, "Vertical");

                int mirroredY = height - 1 - y;

                if (tilePrefab != null)
                {
                    Vector2 positionBL = new Vector2(x, -mirroredY) + offsetBL;
                    Instantiate(tilePrefab, positionBL, mirroredRotation, transform);
                }
            }
        }

        // BottomRightQuadrant
        Vector2 offsetBR = baseOffset + new Vector2(width, -height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int tileType = levelMap[y, x];
                GameObject tilePrefab = GetTilePrefab(tileType);
                Quaternion rotation = GetTileRotation(tileType, x, y);
                Quaternion mirroredRotation = AdjustRotation(rotation, "Both");

                int mirroredX = width - 1 - x;
                int mirroredY = height - 1 - y;

                if (tilePrefab != null)
                {
                    Vector2 positionBR = new Vector2(mirroredX, -mirroredY) + offsetBR;
                    Instantiate(tilePrefab, positionBR, mirroredRotation, transform);
                }
            }
        }
    }

    void MainCamera()
    {
        Camera mainCamera = Camera.main;
        int width = levelMap.GetLength(1);
        int height = levelMap.GetLength(0);

        float totalWidth = width * 2f;
        float totalHeight = height * 2f;        

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float cameraSize = totalHeight / 2f;

        if (totalWidth / totalHeight > aspectRatio)
        {
            cameraSize = (totalWidth / aspectRatio) / 2f;
        }

        mainCamera.orthographicSize = cameraSize;
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }
}
