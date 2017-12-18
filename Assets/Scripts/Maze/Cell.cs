using System;
using UnityEngine;

namespace Scripts.Maze
{
    public class Cell : MonoBehaviour
    {
        private GameObject[] walls;

        public bool IsVisited { get; private set; }

        private void Start()
        {
            walls = new GameObject[4];
            int i = 0;
            foreach (Transform child in transform)
            {
                if (child.tag == Tags.wall)
                {
                    walls[i] = child.gameObject;
                    i++;
                }
            }
        }

        public void DeleteWall(Vector2Int dir, float wait = 0)
        {
            IsVisited = true;
            if (Vector2Int.up == dir)
                Destroy(walls[0], wait);
            else if (Vector2Int.down == dir)
                Destroy(walls[1], wait);
            else if (Vector2Int.right == dir)
                Destroy(walls[2], wait);
            else if (Vector2Int.left == dir)
                Destroy(walls[3], wait);
            else
                throw new Exception("Wrong vector values");
        }
    }
}
