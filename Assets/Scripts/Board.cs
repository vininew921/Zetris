using UnityEngine;
using UnityEngine.Networking.Types;

namespace Assets.Scripts
{
    public class Board
    {
        public int[][] Positions;
        public int Rows;
        public int Columns;

        public Tetrominoe currentPiece;

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            Positions = new int[rows][];
            for (int y = 0; y < rows; y++)
            {
                Positions[y] = new int[columns];
                for (int x = 0; x < columns; x++)
                {
                    Positions[y][x] = 0;
                }
            }

            int randomIndex = Random.Range(0, 7);
            currentPiece = Tetrominoe.GetTetrominoe(randomIndex);
        }

        public void Update()
        {
            currentPiece.Y++;

            if (!ValidPosition())
            {
                currentPiece.Y--;
                FreezeBoard();
                int randomIndex = Random.Range(0, 7);
                currentPiece = Tetrominoe.GetTetrominoe(randomIndex);
            }
        }

        public void Move(int x, int y)
        {
            currentPiece.X += x;
            currentPiece.Y += y;

            if (!ValidPosition())
            {
                currentPiece.X -= x;
                currentPiece.Y -= y;

                if (!currentPiece.Active)
                {
                    FreezeBoard();
                    int randomIndex = Random.Range(0, 7);
                    currentPiece = Tetrominoe.GetTetrominoe(randomIndex);
                }
            }
        }

        public void HardDrop()
        {
            while (ValidPosition())
            {
                currentPiece.Y++;
            }

            currentPiece.Y--;
            FreezeBoard();
            int randomIndex = Random.Range(0, 7);
            currentPiece = Tetrominoe.GetTetrominoe(randomIndex);
        }

        private void FreezeBoard()
        {
            for (int y = 0; y < currentPiece.Shape.Length; y++)
            {
                for (int x = 0; x < currentPiece.Shape[y].Length; x++)
                {
                    if (currentPiece.Shape[y][x] != 0)
                    {
                        Positions[y + currentPiece.Y][x + currentPiece.X] = currentPiece.Shape[y][x];
                    }
                }
            }
        }

        private bool ValidPosition()
        {
            for (int y = 0; y < currentPiece.Shape.Length; y++)
            {
                for (int x = 0; x < currentPiece.Shape[y].Length; x++)
                {
                    if (currentPiece.Shape[y][x] != 0)
                    {
                        if(x + currentPiece.X < 0 || x + currentPiece.X >= Columns)
                        {
                            return false;
                        }

                        if (y + currentPiece.Y >= Rows || Positions[y + currentPiece.Y][x + currentPiece.X] != 0)
                        {
                            currentPiece.Active = false;
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
