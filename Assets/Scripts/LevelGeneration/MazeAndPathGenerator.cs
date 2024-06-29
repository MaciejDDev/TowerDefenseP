using System.Collections.Generic;
using UnityEngine;
namespace LevelGeneration
{

    public class MazeAndPathGenerator
    {

        public Grid GenerateGridByDFS(int width, int height, int cellSize)
        {
            Grid levelGrid = new Grid(width, height, cellSize);
            Cell tempCell, currentCell;
            Stack<Cell> cellStack = new Stack<Cell>();

            bool[,] visitedCells = new bool[width, height];
            ResetVisitedCells(visitedCells, width, height);

            //The creation starts in a random cell
            int randomX = Random.Range(0, width);
            int randomY = Random.Range(0, height);
            tempCell = levelGrid.GetCell(randomX, randomY);
            //Push the starting cell onto the stack
            cellStack.Push(tempCell);
            //While the stack is not empty 
            while (cellStack.Count > 0)
            {
                //Pop the current cell and mark as visited
                currentCell = cellStack.Pop();
                SetCellVisited(visitedCells, currentCell.IndexX, currentCell.IndexY);

                List<Cell> neighboursToVisit = GetNotVisitedNeighbours(levelGrid, currentCell, visitedCells);
                if (neighboursToVisit.Count > 0)
                {
                    //Push again the last visited cell onto the stack for backtracking later
                    cellStack.Push(currentCell);

                    //Get the next cell to visit
                    tempCell = neighboursToVisit[Random.Range(0, neighboursToVisit.Count)];

                    //Break the walls between the cells
                    levelGrid.RemoveWallsInBetween(currentCell, tempCell);

                    //Push the next cell onto the stack
                    cellStack.Push(tempCell);
                }
            }
            return levelGrid;
        }

        public List<Cell> FindPathByDFS(Grid grid, Cell start, Cell end)
        {
            
            List<Cell> tempCellList = new List<Cell>();
            bool[,] visitedCells = new bool[grid.Width, grid.Height];
            //Set All Cells to Unvisited
            ResetVisitedCells(visitedCells, grid.Width, grid.Height);
            //Store the start and end cells
            Cell endCell = grid.GetCell(end.IndexX, end.IndexY);
            Cell currentCell = grid.GetCell(start.IndexX, start.IndexY);

            //Push start cell onto the stack
            Stack<Cell> cellStack = new Stack<Cell>();
            cellStack.Push(currentCell);

            while ((cellStack.Count > 0) && (currentCell != endCell))
            {
                //Pop the currentCell and visit it
                currentCell = cellStack.Pop();
                SetCellVisited(visitedCells, currentCell.IndexX, currentCell.IndexY);


                //Get the connected neighbours
                tempCellList = RemoveVisitedNeighbours(grid.GetConnectedNeighbours(currentCell), visitedCells);
                if (tempCellList.Count > 0)
                {
                    //Push the visited cell again for backtracking later
                    cellStack.Push(currentCell);

                    if (currentCell != endCell)
                    {
                        //Get the next cell from the connected neighbours
                        Cell tempCell = tempCellList[Random.Range(0, tempCellList.Count)];
                        //Push the next cell onto the stack
                        cellStack.Push(tempCell);

                    }

                }
            }
            //After the while, the path is stored in the stack
            //Store the path in a list
            List<Cell> path = new List<Cell>();
            while (cellStack.Count > 0)
            {
                path.Add(cellStack.Pop());
            }

            return path;
        }

        public List<Cell> FindPathByAStar(Grid grid, Cell start, Cell end)
        {
            List<Cell> openNodes = new List<Cell>();
            List<Cell> closedNodes = new List<Cell>();
            List<Cell> finalPathNodes = new List<Cell>();

            openNodes.Add(start);
            while (openNodes.Count > 0)
            {
                Cell currentCell = GetMostPromisingOpenNode(openNodes);

                if (currentCell == end) 
                {
                    finalPathNodes = CalculateFinalPath(start, end);
                    return finalPathNodes;
                }
                List<Cell> neighbours = grid.GetConnectedNeighbours(currentCell);
                foreach (var neighbour in neighbours)
                {
                    if (closedNodes.Contains(neighbour))
                        continue;
                    if (!openNodes.Contains(neighbour))
                    {
                        neighbour.iG = currentCell.iG + 1;
                        neighbour.iH = CalculateManhattanDistance(neighbour, end);
                        neighbour.ParentCell = currentCell;
                        openNodes.Add(neighbour);
                    }
                    else
                    {
                        if (neighbour.iG > currentCell.iG)
                        {
                            neighbour.iG = currentCell.iG + 1;
                            neighbour.ParentCell = currentCell;
                        }
                    }
                }
                openNodes.Remove(currentCell);
                closedNodes.Add(currentCell);
            }
            return finalPathNodes;
        }

        private List<Cell> CalculateFinalPath(Cell start, Cell end)
        {
            List<Cell> finalPath = new List<Cell>();
            Cell currentCell = end;
            finalPath.Add(currentCell);
            while (currentCell != start)
            {
                currentCell = currentCell.ParentCell;
                finalPath.Add(currentCell);
            }
            finalPath.Reverse();
            return finalPath;
            
        }

        private int CalculateManhattanDistance(Cell neighbour, Cell end)
        {
            int manhattanDist = 0;
            manhattanDist += System.Math.Abs(neighbour.IndexX - end.IndexX);
            manhattanDist += System.Math.Abs(neighbour.IndexY - end.IndexY);

            return manhattanDist;
        }

        private Cell GetMostPromisingOpenNode(List<Cell> openNodes)
        {
            if (openNodes.Count == 0)
            {
                Debug.LogWarning("[A*] :: Open Nodes Count = 0");
                return null;
            }
            Cell bestCell = openNodes[0];
            foreach (Cell cell in openNodes) 
            { 
                if (cell.IF < bestCell.IF)
                    bestCell = cell;
            }
            return bestCell;
        }

        private void ResetVisitedCells(bool[,] visitedCells, int width, int height)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    visitedCells[i, j] = false;
        }

        private void SetCellVisited(bool[,] visitedCells, int width, int height)
        {
            visitedCells[width, height] = true;
        }

        private List<Cell> GetNotVisitedNeighbours(Grid grid, Cell cell, bool[,] visitedCells)
        {
            List<Cell> neighbours = grid.GetNeighbours(cell);
            List<Cell> notVisitedNeighbours = new List<Cell>();
            foreach (var neighbour in neighbours)
            {
                if (!visitedCells[neighbour.IndexX, neighbour.IndexY])
                    notVisitedNeighbours.Add(neighbour);
            }
            return notVisitedNeighbours;
        }

        private List<Cell> RemoveVisitedNeighbours(List<Cell> neighbours, bool[,] visitedCells)
        {
            List<Cell> notVisitedNeighbours = new List<Cell>();
            foreach (var neighbour in neighbours)
            {
                if (!visitedCells[neighbour.IndexX, neighbour.IndexY])
                    notVisitedNeighbours.Add(neighbour);
            }
            return notVisitedNeighbours;
        }

    }
}