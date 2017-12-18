using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        private Cell[,] _cells;
        private System.Random random;
        [SerializeField] private int _seed;
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GameObject _cellPrefab;

        private void Start()
        {
            random = new System.Random(_seed);
            StartCoroutine(GenerateMaze());
        }

        private List<Vector2Int> stack;

        private IEnumerator GenerateMaze()
        {
            var size = _cellPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size;
            _cells = new Cell[_width, _height];
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var newCell = Instantiate(_cellPrefab, new Vector3(x * size.x, 0, y * size.z), new Quaternion(), transform);
                    _cells[x, y] = newCell.GetComponent<Cell>();
                }
            }
            yield return null;//wait for cells to initialize

            stack = new List<Vector2Int>();

            var currPos = new Vector2Int(0, 0);
            int visitedCount = 0;
            for (int i = 0; i < _cells.Length - 1; i++)
            {
                var c = AllUn_VisitedAdjacentTiles(currPos, false);
                if (c.Count != 0)
                {
                    var newPos = c[random.Next(0, c.Count)];

                    VisitCell(currPos, newPos);
                    currPos = newPos;
                    AllUn_VisitedAdjacentTiles(currPos, false).ForEach(item => stack.Add(item));
                }
                else
                {
                    var newPos = stack[0];
                    var cc = AllUn_VisitedAdjacentTiles(newPos, true);
                    var currPosAnother = cc[random.Next(0, cc.Count)];

                    VisitCell(currPosAnother, newPos);
                    currPos = newPos;
                }
                var toDel = new List<Vector2Int>();
                foreach (var item in stack)
                {
                    if (_cells[item.x, item.y].IsVisited == true)
                    {
                        toDel.Add(item);
                    }
                }
                foreach (var item in toDel)
                {
                    stack.Remove(item);
                }
                visitedCount++;
                yield return null;
            }
        }

        private void VisitCell(Vector2Int currPos, Vector2Int posMovingTo)
        {
            var vecDir = posMovingTo - currPos;

            _cells[currPos.x, currPos.y].DeleteWall(vecDir);
            _cells[posMovingTo.x, posMovingTo.y].DeleteWall(vecDir * -1);
        }

        private List<Vector2Int> AllUn_VisitedAdjacentTiles(Vector2Int pos, bool returnVisited)
        {
            var notVisited = new List<Vector2Int>();
            try
            {
                if (_cells[pos.x + 1, pos.y].IsVisited == returnVisited)
                {
                    notVisited.Add(new Vector2Int(pos.x + 1, pos.y));
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (_cells[pos.x - 1, pos.y].IsVisited == returnVisited)
                {
                    notVisited.Add(new Vector2Int(pos.x - 1, pos.y));
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (_cells[pos.x, pos.y + 1].IsVisited == returnVisited)
                {
                    notVisited.Add(new Vector2Int(pos.x, pos.y + 1));
                }
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (_cells[pos.x, pos.y - 1].IsVisited == returnVisited)
                {
                    notVisited.Add(new Vector2Int(pos.x, pos.y - 1));
                }
            }
            catch (IndexOutOfRangeException) { }

            return notVisited;
        }
    }
}
