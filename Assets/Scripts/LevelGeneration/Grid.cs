using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{

    public class Grid
    {

        #region Fields
        int _height, _width;

        private Cell[,] _grid;
        #endregion


        public int Height => _height;
        public int Width => _width;
        public Cell[,] GetGrid => _grid;
        public Cell GetCell(int X, int Y) => _grid[X, Y];

        #region Methods
        public Grid(int width, int height, int cellSize)
        {
            _width = width;
            _height = height;
            _grid = new Cell[width, height];

            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    _grid[i, j] = new Cell(i, j, cellSize);

        }
        public List<Cell> GetNeighbours(Cell cell)
        {
            List<Cell> neighbours = new List<Cell>();

            if (IsValid(cell.IndexX, cell.IndexY + 1)) //Top
                neighbours.Add(_grid[cell.IndexX, cell.IndexY + 1]);
            if (IsValid(cell.IndexX + 1, cell.IndexY)) //Right
                neighbours.Add(_grid[cell.IndexX + 1, cell.IndexY]);
            if (IsValid(cell.IndexX, cell.IndexY - 1)) //Down
                neighbours.Add(_grid[cell.IndexX, cell.IndexY - 1]);
            if (IsValid(cell.IndexX - 1, cell.IndexY)) //Left
                neighbours.Add(_grid[cell.IndexX - 1, cell.IndexY]);
            return neighbours;
        }
        public void RemoveWallsInBetween(Cell c1, Cell c2)
        {
            if (c1.IndexX == c2.IndexX) // Same Column
            {
                if (c1.IndexY > c2.IndexY) // c1 on top
                {
                    c1.BottomWall = false;
                    c2.TopWall = false;
                }
                else //c2 on top
                {
                    c1.TopWall = false;
                    c2.BottomWall = false;
                }

            }
            if (c1.IndexY == c2.IndexY) // Same Row
            {
                if (c1.IndexX > c2.IndexX) // c1 on the right
                {
                    c1.LeftWall = false;
                    c2.RightWall = false;
                }
                else //c2 on the right
                {
                    c1.RightWall = false;
                    c2.LeftWall = false;
                }
            }
        }
        public List<Cell> GetConnectedNeighbours(Cell cell)
        {
            List<Cell> connectedNeighbours = new List<Cell>();
            int x = cell.IndexX;
            int y = cell.IndexY;
            if (!cell.TopWall && IsValid(x, y + 1)) //TOP
                connectedNeighbours.Add(_grid[x, y + 1]);
            if (!cell.RightWall && IsValid(x + 1, y)) //RIGHT
                connectedNeighbours.Add(_grid[x + 1, y]);
            if (!cell.BottomWall && IsValid(x, y - 1)) //BOTTOM
                connectedNeighbours.Add(_grid[x, y - 1]);
            if (!cell.LeftWall && IsValid(x - 1, y)) //LEFT
                connectedNeighbours.Add(_grid[x - 1, y]);
            
            return connectedNeighbours;
        }
        public void DrawGizmos()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _grid[i, j].DrawGizmos(_width, _height);
                }
            }
        }
        public void SetCellAsPath(Cell cell)
        {
            _grid[cell.IndexX, cell.IndexY].IsPath = true;
        }
        
        public Cell GetClosestCellToWorldPosition(Vector3 position)
        {
            float minDist = Mathf.Infinity;
            Cell closestCell = null;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    float tempDist = Vector3.Distance(position, _grid[i, j].GetCenterWorldSpace(_width, _height));
                    if (tempDist < minDist)
                    {
                        minDist = tempDist;
                        closestCell = _grid[i, j];
                    }
                }
            }
            return closestCell;
        }
        private bool IsValid(int x, int y)
        {
            return (x >= 0 && x < _width) && (y >= 0 && y < _height);
        }


        #endregion
    }
}