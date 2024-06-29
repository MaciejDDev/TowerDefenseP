using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelGeneration
{

    [CreateAssetMenu(fileName ="LevelConfig",menuName ="Config/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Map Dimensions")]
        [SerializeField] int _width;
        [SerializeField] int _height;

        [Header("Cell Size")]
        [SerializeField] int _cellSize;


        Grid _levelGrid;
        MazeAndPathGenerator _levelGenerator;
        Cell _baseCell;
        public Grid LevelGrid => _levelGrid;
        public Cell BaseCell => _baseCell;
        public void Init()
        {
            _levelGenerator = new MazeAndPathGenerator();
        }
        
        
        private void SetBaseCell()
        {
            _baseCell = _levelGrid.GetCell(_width / 2, _height / 2);
            //Remove walls around base cell
            List<Cell> baseNeighbours = _levelGrid.GetNeighbours(_baseCell);
            foreach (var neighbour in baseNeighbours)
            {
                _levelGrid.RemoveWallsInBetween(_baseCell, neighbour);
            }
        
        }
        public void GenerateNewMap()
        {
            _levelGrid = _levelGenerator.GenerateGridByDFS( _width, _height, _cellSize);
            SetBaseCell();
        }
        public List<Vector3> GeneratePath(Cell start, Cell end)
        {
            List<Cell> path = _levelGenerator.FindPathByAStar(_levelGrid, start, end);
            List<Vector3> positionsPath = new List<Vector3>();
            foreach (Cell cell in path)
            {
                _levelGrid.SetCellAsPath(cell);
                positionsPath.Add(cell.GetCenterWorldSpace(_width, _height));
            }
            return positionsPath;
        }



    }
}