using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Board
    {
        public int[][] Positions;
        public int Rows;
        public int Columns;
        public int Points;
        public int Level;

        public Tetrominoe currentPiece;

        public bool Lost;
        public bool ClearedLines;
        public bool PieceLocked;

        private int totalClearedLines;

        private readonly int[] pointTable =
        {
            40,
            100,
            300,
            1200
        };

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Points = 0;
            Level = 0;

            Lost = false;
            ClearedLines = false;
            PieceLocked = false;

            totalClearedLines = 0;

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
                GenerateNextPiece();
            }
        }

        public void GenerateNextPiece()
        {
            FreezeBoard();
            PieceLocked = true;
            int randomIndex = Random.Range(0, 7);
            currentPiece = Tetrominoe.GetTetrominoe(randomIndex);

            int clearedLines = ClearLines();
            if(clearedLines > 0)
            {
                totalClearedLines += clearedLines;
                Points += pointTable[clearedLines - 1];
                ClearedLines = true;

                Level = totalClearedLines / 10;
            }

            if (!ValidPosition())
            {
                Lost = true;
            }
        }

        public int ClearLines()
        {
            int clearedLines = 0;

            for (int y = 0; y < Rows; y++)
            {
                bool clear = true;
                for (int x = 0; x < Columns; x++)
                {
                    if(Positions[y][x] == 0)
                    {
                        clear = false;
                        break;
                    }
                }

                if (clear)
                {
                    clearedLines++;
                    for(int clearY = y; clearY >= 0; clearY--)
                    {
                        for (int x = 0; x < Columns; x++)
                        {
                            if(clearY == 0)
                            {
                                Positions[0][x] = 0;
                            }
                            else
                            {
                                Positions[clearY][x] = Positions[clearY - 1][x];
                            }
                        }
                    }
                }
            }

            return clearedLines;
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
                    GenerateNextPiece();
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
            GenerateNextPiece();
        }

        public void Rotate(bool clockWise)
        {
            int[][] newShape = new int[currentPiece.Shape.Length][];

            if (clockWise)
            {
                //Transpose
                for (int y = 0; y < currentPiece.Shape.Length; y++)
                {
                    newShape[y] = new int[currentPiece.Shape[y].Length];
                    for (int x = 0; x < currentPiece.Shape[y].Length; x++)
                    {
                        newShape[y][x] = currentPiece.Shape[x][y];
                    }
                }

                //Reverse rows
                for (int y = 0; y < currentPiece.Shape.Length; y++)
                {
                    newShape[y] = newShape[y].Reverse().ToArray();
                }
            }
            else
            {
                int[][] reversed = new int[currentPiece.Shape.Length][];

                //Reverse rows
                for (int y = 0; y < currentPiece.Shape.Length; y++)
                {
                    reversed[y] = currentPiece.Shape[y].Reverse().ToArray();
                }

                //Transpose
                for (int y = 0; y < currentPiece.Shape.Length; y++)
                {
                    newShape[y] = new int[currentPiece.Shape[y].Length];
                    for (int x = 0; x < currentPiece.Shape[y].Length; x++)
                    {
                        newShape[y][x] = reversed[x][y];
                    }
                }
            }

            int[][] oldShape = currentPiece.Shape;
            currentPiece.Shape = newShape;

            if (!ValidPosition())
            {
                currentPiece.Shape = oldShape;
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
                        if (x + currentPiece.X < 0 || x + currentPiece.X >= Columns)
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
