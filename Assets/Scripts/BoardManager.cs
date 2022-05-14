using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Completed
{

    public class BoardManager : MonoBehaviour
    {

        private GameObject player;

        [SerializeField]
        private GameObject exit;
        [SerializeField]
        private GameObject[] floorTiles;
        [SerializeField]
        private GameObject[] wallTiles;
        [SerializeField]
        private GameObject[] foodTiles;
        [SerializeField]
        private GameObject[] enemyObjects;
        [SerializeField]
        private int deviationRate = 10;
        [SerializeField]
        private int roomRate = 15;
        [SerializeField]
        private int maxRouteLength = 30;
        [SerializeField]
        private int maxRoutes = 20;
        private int routeCount = 0;

        private int bossRoute = 0;
        private Vector2Int bossRoom = new Vector2Int(0,0);

        private Transform boardHolder;
        private List <Vector2Int> gridPositions = new List<Vector2Int>();
        private List <Vector2Int> wallPositions = new List<Vector2Int>();
        private List <Vector3> enemyPositions = new List<Vector3>();

        void InitialiseList()
        {
            gridPositions.Clear();
            wallPositions.Clear();
            enemyPositions.Clear();
            routeCount = 0;
            bossRoom = new Vector2Int(0,0);
        }

        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum, Vector2Int minPos, Vector2Int maxPos)
        {
            int objectCount = Random.Range(minimum, maximum +1);

            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y),0f);
                GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
                Instantiate (tileChoice, randomPosition, Quaternion.identity);
            }
        }

        void spawnEnemies(GameObject enemy, int minimum, int maximum, Vector2Int minPos, Vector2Int maxPos)
        {
            int objectCount = Random.Range(minimum, maximum +1);
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y),0f);
                if (!enemyPositions.Contains(randomPosition))
                {
                    enemyPositions.Add(randomPosition);
                    Instantiate(enemy, randomPosition, Quaternion.identity);
                }
            }
        }

        public void SetupScene(int level)
        {
            boardHolder = new GameObject ("Board").transform;
            InitialiseList();
            int enemyCount = (int)Math.Log(level,2f);
            int x = 0;
            int y = 0;
            int routeLength = 0;
            Vector2Int previousPos = new Vector2Int(x, y);
            GenerateSquare(x, y, 1);
            y += 3;
            GenerateSquare(x, y, 1);
            NewRoute(x, y, routeLength, previousPos);
            foreach (Vector2Int position in wallPositions)
            {
                if (Vector2Int.Distance(previousPos, position)>Vector2Int.Distance(previousPos, bossRoom))
                    bossRoom = position;
            }
            createBossRoom();
            FillWalls();
            //Instantiate(exit, new Vector3(previousPos.x,previousPos.y, 0F), Quaternion.identity);
        }

        private void FillWalls()
        {
            foreach (Vector2Int position in wallPositions)
            {
                GameObject toInstantiate = wallTiles[2];
                GameObject instance = Instantiate(toInstantiate, new Vector3 (position.x,position.y,0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }

        private void createBossRoom()
        {
            int x = bossRoom.x;
            int y = bossRoom.y;
            GenerateSquare(x, y, 1);
            if(Math.Abs(x ) > Math.Abs(y) )
            {
                int adder=(x>0)?3:-3;
                x+=adder;
                GenerateSquare(x, y, 1);
                x+=adder;
                GenerateSquare(x, y, 1);
                x+=adder*2;
            }
            else
            {
                int adder=(y>0)?3:-3;
                y+=adder;
                GenerateSquare(x, y, 1);
                y+=adder;
                GenerateSquare(x, y, 1);
                y+=adder*2;
            }
            GenerateSquare(x, y, 6);
            Instantiate(exit, new Vector3 (x, y, 0f), Quaternion.identity);
        }

        private void NewRoute(int x, int y, int routeLength, Vector2Int previousPos)
        {
            if (routeCount < maxRoutes)
            {
                routeCount++;
                while (++routeLength < maxRouteLength)
                {
                    //Initialize
                    bool routeUsed = false;
                    int xOffset = x - previousPos.x; //0
                    int yOffset = y - previousPos.y; //3
                    int roomSize = 1; //Hallway size
                    if (Random.Range(1, 100) <= roomRate)
                        roomSize = Random.Range(3, 6);
                    previousPos = new Vector2Int(x, y);

                    //Go Straight
                    if (Random.Range(1, 100) <= deviationRate)
                    {
                        if (routeUsed)
                        {
                            GenerateSquare(previousPos.x + xOffset, previousPos.y + yOffset, roomSize);
                            NewRoute(previousPos.x + xOffset, previousPos.y + yOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                        }
                        else
                        {
                            x = previousPos.x + xOffset;
                            y = previousPos.y + yOffset;
                            GenerateSquare(x, y, roomSize);
                            routeUsed = true;
                        }
                    }

                    //Go left
                    if (Random.Range(1, 100) <= deviationRate)
                    {
                        if (routeUsed)
                        {
                            GenerateSquare(previousPos.x - yOffset, previousPos.y + xOffset, roomSize);
                            NewRoute(previousPos.x - yOffset, previousPos.y + xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                        }
                        else
                        {
                            y = previousPos.y + xOffset;
                            x = previousPos.x - yOffset;
                            GenerateSquare(x, y, roomSize);
                            routeUsed = true;
                        }
                    }
                    //Go right
                    if (Random.Range(1, 100) <= deviationRate)
                    {
                        if (routeUsed)
                        {
                            GenerateSquare(previousPos.x + yOffset, previousPos.y - xOffset, roomSize);
                            NewRoute(previousPos.x + yOffset, previousPos.y - xOffset, Random.Range(routeLength, maxRouteLength), previousPos);
                        }
                        else
                        {
                            y = previousPos.y - xOffset;
                            x = previousPos.x + yOffset;
                            GenerateSquare(x, y, roomSize);
                            routeUsed = true;
                        }
                    }

                    if (!routeUsed)
                    {
                        x = previousPos.x + xOffset;
                        y = previousPos.y + yOffset;
                        GenerateSquare(x, y, roomSize);
                    }
                }
            }
        }

        private void GenerateSquare(int x, int y, int radius)
        {
            Vector2Int pos;
            int minX = x -radius;
            int minY = y -radius;
            int maxX = x +radius;
            int maxY = y +radius;

            for (int tileX = minX; tileX <= maxX; tileX++)
            {
                for (int tileY = minY; tileY <= maxY; tileY++)
                {
                    GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3 (tileX,tileY,0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    pos = new Vector2Int (tileX,tileY);
                    gridPositions.Add(pos);
                    if (wallPositions.Contains(pos))
                    {
                        wallPositions.RemoveAll(pos.Equals);
                    }
                }
            }

            for (int tileX =minX; tileX <= maxX; tileX++)
            {
                pos=new Vector2Int( maxX+1, minY + (tileX -minX));
                if (!gridPositions.Contains(pos))
                {
                    wallPositions.Add(pos);
                }

                pos = new Vector2Int( minX-1, minY + (tileX -minX));
                if (!gridPositions.Contains(pos))
                {
                    wallPositions.Add(pos);
                }

                pos = new Vector2Int(tileX, maxY+1 );
                if (!gridPositions.Contains(pos))
                {
                    wallPositions.Add(pos);
                }
                pos = new Vector2Int(tileX,minY-1);
                if (!gridPositions.Contains(pos))
                {
                    wallPositions.Add(pos);
                }
            }

            pos = new Vector2Int( minX -1,minY-1);
            if (!gridPositions.Contains(pos))
            {
                wallPositions.Add(pos);
            }
            pos = new Vector2Int( maxX +1,minY-1);
            if (!gridPositions.Contains(pos))
            {
                wallPositions.Add(pos);
            }
            pos = new Vector2Int( minX -1,maxY+1);
            if (!gridPositions.Contains(pos))
            {
                wallPositions.Add(pos);
            }
            pos = new Vector2Int( maxX +1,maxY+1);
            if (!gridPositions.Contains(pos))
            {
                wallPositions.Add(pos);
            }
            if (radius>1)
            {                

                int randomEnemyIndex = Random.Range(0, enemyObjects.Length);
                spawnEnemies(enemyObjects[randomEnemyIndex], radius, radius * 2, new Vector2Int(minX, minY), new Vector2Int(maxX, maxY));
            }                
        }
    }
}

