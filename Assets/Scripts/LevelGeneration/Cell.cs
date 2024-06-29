using System;
using UnityEngine;

namespace LevelGeneration
{
    [Serializable]
    public class Cell
    {
        #region Fields
        int _indexX;
        int _indexY;
        bool _isPath;
        private bool[] _walls;
        private int _cellSize;
        #endregion

        //A* data
        public Cell ParentCell;
        public int iG;
        public int iH;
        #region Properties


        public int IF => iG + iH;

        public bool TopWall { get { return _walls[0]; } set { _walls[0] = value; } }
        public bool RightWall { get { return _walls[1]; } set { _walls[1] = value; } }
        public bool BottomWall { get { return _walls[2]; } set { _walls[2] = value; } }
        public bool LeftWall { get { return _walls[3]; } set { _walls[3] = value; } }

        public int IndexX => _indexX;
        public int IndexY => _indexY;
        public int cellSize => _cellSize;

        public bool IsPath { get { return _isPath; } set { _isPath = value; } }

        #endregion

        #region Methods
        public Cell(int x, int y, int cellSize)
        {
            _indexX = x;
            _indexY = y;

            _walls = new bool[] { true, true, true, true };
            _cellSize = cellSize;
        }

        public void DrawGizmos(int gridWidth, int gridHeight)
        {
            float cellHalfSize = _cellSize / 2f;
            float horizontalOffset = (gridWidth - 1) * _cellSize / 2f;
            float verticalOffset = (gridHeight - 1) * _cellSize / 2f;
            float cellCenterWorldX = _cellSize * _indexX - horizontalOffset;
            float cellCenterWorldY = _cellSize * _indexY - verticalOffset;
            Gizmos.color = Color.green;
            if (_walls[0]) // TOP
                Gizmos.DrawLine(new Vector3(cellCenterWorldX - cellHalfSize, 0, cellCenterWorldY + cellHalfSize), new Vector3(cellCenterWorldX  + cellHalfSize, 0, cellCenterWorldY + cellHalfSize));
            if (_walls[1]) // RIGHT
                Gizmos.DrawLine(new Vector3(cellCenterWorldX + cellHalfSize, 0, cellCenterWorldY - cellHalfSize), new Vector3(cellCenterWorldX  + cellHalfSize, 0, cellCenterWorldY + cellHalfSize));
            if (_walls[2]) // DOWN
                Gizmos.DrawLine(new Vector3(cellCenterWorldX - cellHalfSize, 0, cellCenterWorldY - cellHalfSize), new Vector3(cellCenterWorldX + cellHalfSize, 0, cellCenterWorldY - cellHalfSize));
            if (_walls[3]) // LEFT
                Gizmos.DrawLine(new Vector3(cellCenterWorldX - cellHalfSize, 0, cellCenterWorldY - cellHalfSize), new Vector3(cellCenterWorldX - cellHalfSize, 0, cellCenterWorldY + cellHalfSize));
            /*
            Gizmos.color = Color.red;
            if (_isPath)
                Gizmos.DrawCube(GetCenterWorldSpace(gridWidth,gridHeight),_cellSize * 0.5f * Vector3.one);
            */
        }

        public override string ToString()
        {
            string cellString = "";
            cellString = string.Format("<{0},{1}>", this._indexX, this._indexY);
            return cellString;
        }


        public Vector3 GetCenterWorldSpace(int gridWidth, int gridHeight)
        {
            float horizontalOffset = (gridWidth - 1) * _cellSize / 2f;
            float verticalOffset = (gridHeight - 1) * _cellSize / 2f;
            float xCoord = _cellSize * _indexX - horizontalOffset;
            float zCoord = _cellSize * _indexY - verticalOffset;

            return new Vector3(xCoord, 0f, zCoord);
        }

        public override bool Equals(System.Object obj)
        {
            Cell cell = obj as Cell;
            if (cell == null) 
                return false;

            if (cell.IndexX == this._indexX &&  cell.IndexY == this._indexY)
                return true;

            return false;
        }
        #endregion
    }
}