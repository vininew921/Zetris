using UnityEngine;
using UnityEngine.Networking.Types;

namespace Assets.Scripts
{
    public class Board
    {
        public int[][] Positions;
        public int Rows;
        public int Columns;
        string formatedMatrix;

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

            formatedMatrix = FormatMatrix();

            if (!ValidPosition())
            {
                currentPiece.Y--;
                FreezeBoard();
                int randomIndex = Random.Range(0, 7);
                currentPiece = Tetrominoe.GetTetrominoe(randomIndex);
            }
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
                        if (y + currentPiece.Y >= Rows || Positions[y + currentPiece.Y][x + currentPiece.X] != 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private string FormatMatrix(int pad = 10)
        {
            string result = "";

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    int currentSquare = Positions[y][x];
                    result += currentSquare.ToString().PadLeft(pad);
                }

                result += "\n";
            }

            return result;
        }
    }
}
