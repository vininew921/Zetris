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
        public Tetrominoe phantomPiece;

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
            phantomPiece = Tetrominoe.GetTetrominoe(randomIndex);
            SetPhantomPiece();
        }

        public void Update()
        {
            currentPiece.Y++;

            if (!ValidPosition(currentPiece))
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
            phantomPiece = Tetrominoe.GetTetrominoe(randomIndex);
            SetPhantomPiece();

            int clearedLines = ClearLines();
            if(clearedLines > 0)
            {
                totalClearedLines += clearedLines;
                Points += pointTable[clearedLines - 1];
                ClearedLines = true;

                Level = totalClearedLines / 10;
            }

            if (!ValidPosition(currentPiece))
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

            if (!ValidPosition(currentPiece))
            {
                currentPiece.X -= x;
                currentPiece.Y -= y;

                if (!currentPiece.Active)
                {
                    GenerateNextPiece();
                }
            }

            SetPhantomPiece();
        }

        public void HardDrop()
        {
            while (ValidPosition(currentPiece))
            {
                currentPiece.Y++;
            }

            currentPiece.Y--;
            GenerateNextPiece();
        }

        public void SetPhantomPiece()
        {
            phantomPiece.X = currentPiece.X;
            phantomPiece.Y = currentPiece.Y;

            while (ValidPosition(phantomPiece))
            {
                phantomPiece.Y++;
            }

            phantomPiece.Y--;
            phantomPiece.X = currentPiece.X;
        }

        public void Rotate(bool clockWise)
        {
            RotatePiece(currentPiece, clockWise);

            phantomPiece.X = currentPiece.X;
            phantomPiece.Y = currentPiece.Y;
            RotatePiece(phantomPiece, clockWise);
            SetPhantomPiece();
        }

        private void RotatePiece(Tetrominoe piece, bool clockWise)
        {
            int[][] newShape = new int[piece.Shape.Length][];

            if (clockWise)
            {
                //Transpose
                for (int y = 0; y < piece.Shape.Length; y++)
                {
                    newShape[y] = new int[piece.Shape[y].Length];
                    for (int x = 0; x < piece.Shape[y].Length; x++)
                    {
                        newShape[y][x] = piece.Shape[x][y];
                    }
                }

                //Reverse rows
                for (int y = 0; y < piece.Shape.Length; y++)
                {
                    newShape[y] = newShape[y].Reverse().ToArray();
                }
            }
            else
            {
                int[][] reversed = new int[piece.Shape.Length][];

                //Reverse rows
                for (int y = 0; y < piece.Shape.Length; y++)
                {
                    reversed[y] = piece.Shape[y].Reverse().ToArray();
                }

                //Transpose
                for (int y = 0; y < piece.Shape.Length; y++)
                {
                    newShape[y] = new int[piece.Shape[y].Length];
                    for (int x = 0; x < piece.Shape[y].Length; x++)
                    {
                        newShape[y][x] = reversed[x][y];
                    }
                }
            }

            int[][] oldShape = piece.Shape;
            piece.Shape = newShape;

            if (!ValidPosition(piece))
            {
                piece.Shape = oldShape;
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

        private bool ValidPosition(Tetrominoe piece)
        {
            for (int y = 0; y < piece.Shape.Length; y++)
            {
                for (int x = 0; x < piece.Shape[y].Length; x++)
                {
                    if (piece.Shape[y][x] != 0)
                    {
                        if (x + piece.X < 0 || x + piece.X >= Columns)
                        {
                            return false;
                        }

                        if (y + piece.Y >= Rows || Positions[y + piece.Y][x + piece.X] != 0)
                        {
                            piece.Active = false;
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
