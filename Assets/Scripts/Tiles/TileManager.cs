using System;
using UnityEngine;
using LevelGeneration;
using System.Collections.Generic;
using Enemies;
using Utilities;
namespace TerrainGeneration
{
    public class TileManager : MonoBehaviour
    {
        [Header("Level Configuration")]
        [SerializeField] LevelConfig _levelConfig;
        
        
        [Header("Tiles Configuration")]
        [SerializeField] FactoryConfig _tileConfig;
        Factory _tileFactory;


        [Header("Enemy Spawners")]
        [SerializeField] List<EnemySpawner> _enemySpawns;

        bool levelGenerated = false;

        public void Init()
        {
            _tileFactory = new Factory(_tileConfig);
            _levelConfig.Init();
            
        }

        public void CreateNewLevel()
        {
            _levelConfig.GenerateNewMap();
            levelGenerated = true;
            foreach (var enemySpawner in _enemySpawns)
            {
                enemySpawner.SetStartCell(_levelConfig.LevelGrid.GetClosestCellToWorldPosition(enemySpawner.transform.position));
                enemySpawner.SetEnemiesPath(_levelConfig.GeneratePath(enemySpawner.GetStartCell, _levelConfig.BaseCell));
                enemySpawner.ResetTimer();
            }
            SpawnTilesOnMap(_levelConfig.LevelGrid);
        }
        public void RecycleAllTiles()
        {
            _tileFactory.RecycleAllPools();
        }

        public void SpawnTilesOnMap(LevelGeneration.Grid levelGrid)
        {
            string id = "";
            for (int i = 0; i < levelGrid.Width; i++)
            {
                for (int j = 0; j < levelGrid.Height; j++)
                {
                    if (levelGrid.GetGrid[i, j].IsPath)
                        id = "Path";
                    else
                        id = "Floor";
                    //Hacer una clase para que traduzca de índices de la matriz a coordenadas del mundo
                    Vector3 tileRotation = new Vector3(0f, 0f, 0f);
                    _tileFactory.Create(id, levelGrid.GetGrid[i, j].GetCenterWorldSpace(levelGrid.Width, levelGrid.Height), tileRotation);
                    SpawnCellWalls(levelGrid, levelGrid.GetGrid[i, j]);
                }
            }
        }

        private void SpawnCellWalls(LevelGeneration.Grid levelGrid, Cell cell)
        {
            Vector3 wallTileRot = new Vector3(0f, 0f, 0f);
            Vector3 leftWallPos = cell.GetCenterWorldSpace(levelGrid.Width, levelGrid.Height) - new Vector3(cell.cellSize / 2f, 0f, 0f);
            if (cell.IndexX > 0)
            {
                string id = "";
                if (cell.LeftWall)      
                    id = "Build";
                else
                {
                    if (cell.IsPath && levelGrid.GetCell(cell.IndexX - 1, cell.IndexY).IsPath)
                        id = "Path";
                    else if ((cell.IsPath && !levelGrid.GetCell(cell.IndexX - 1, cell.IndexY).IsPath) || (!cell.IsPath && levelGrid.GetCell(cell.IndexX - 1, cell.IndexY).IsPath))
                        id = "Build";
                    else
                        id = "Floor";
                }
                //Spawn Left Wall
                _tileFactory.Create(id, leftWallPos, wallTileRot);
                if ( cell.IndexY > 0)
                {
                    Vector3 cornerTilePos = cell.GetCenterWorldSpace(levelGrid.Width, levelGrid.Height) - new Vector3(cell.cellSize / 2f, 0f, cell.cellSize / 2f);
                    _tileFactory.Create("Build", cornerTilePos, wallTileRot);
                }

            }


            if (cell.IndexY > 0)
            {
                string id = "";
                //Spawn bottom wall
                Vector3 bottomWallPos = cell.GetCenterWorldSpace(levelGrid.Width, levelGrid.Height) - new Vector3(0f, 0f, cell.cellSize / 2f);
                if (cell.BottomWall)
                {
                    id = "Build";
                    
                    //_tileFactory.Create("Wall", bottomWallPos, wallTileRot);
                }
                else
                {
                    if (cell.IsPath && levelGrid.GetCell(cell.IndexX, cell.IndexY - 1).IsPath)
                        id = "Path";
                    else if ((cell.IsPath && !levelGrid.GetCell(cell.IndexX, cell.IndexY - 1).IsPath) || (!cell.IsPath && levelGrid.GetCell(cell.IndexX, cell.IndexY - 1).IsPath))
                        id = "Build";
                    else
                        id = "Floor";
                }
                _tileFactory.Create(id, bottomWallPos, wallTileRot);


            }







        }
        private void OnDrawGizmos()
        {
            if (levelGenerated)
                _levelConfig.LevelGrid.DrawGizmos();
        }



    }
    
}
