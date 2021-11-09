using Assets.Scripts;
using UnityEngine;

public class GLBehaviour : MonoBehaviour
{
    public Material drawMaterial;
    public TetrisController tetrisController;
    public Vector3 drawOffset;
    public Vector3 nextPiecePosition;
    public float squareScale = 0.5f;

    private bool gameStarted = false;
    private Color[] colors;

    private void Start()
    {
        colors = new Color[] { Color.black, Color.cyan, Color.yellow, Color.magenta, Color.green, Color.red, Color.blue, new Color(1, 165f / 255f, 0) };
    }

    public void StartGame()
    {
        gameStarted = true;
        tetrisController.StartGame();
    }

    private void OnPostRender()
    {
        if (gameStarted)
        {
            StartOpenGLOperations();

            RenderGame();

            EndOpenGLOperations();
        }
    }

    private void RenderGame()
    {
        GL.Begin(GL.QUADS);

        for(int y = 0; y < tetrisController.board.Rows; y++)
        {
            for(int x = 0; x < tetrisController.board.Columns; x++)
            {
                int currentSquare = tetrisController.board.Positions[y][x];
                GL.Color(colors[currentSquare]);
                DrawSquare(x, tetrisController.board.Rows - y);
            }
        }

        Tetrominoe phantomPiece = tetrisController.board.phantomPiece;

        for (int y = 0; y < phantomPiece.Shape.Length; y++)
        {
            for (int x = 0; x < phantomPiece.Shape[y].Length; x++)
            {
                if (phantomPiece.Shape[y][x] != 0)
                {
                    GL.Color(Color.gray);
                    DrawSquare(x + phantomPiece.X, tetrisController.board.Rows - phantomPiece.Y - y);
                }
            }
        }

        Tetrominoe currentPiece = tetrisController.board.currentPiece;

        for (int y = 0; y < currentPiece.Shape.Length; y++)
        {
            for (int x = 0; x < currentPiece.Shape[y].Length; x++)
            {
                if (currentPiece.Shape[y][x] != 0)
                {
                    GL.Color(colors[currentPiece.Shape[y][x]]);
                    DrawSquare(x + currentPiece.X, tetrisController.board.Rows - currentPiece.Y - y);
                }
            }
        }

        Tetrominoe nextPiece = tetrisController.board.nextPiece;

        for (int y = 0; y < nextPiece.Shape.Length; y++)
        {
            for (int x = 0; x < nextPiece.Shape[y].Length; x++)
            {
                if (nextPiece.Shape[y][x] != 0)
                {
                    GL.Color(colors[nextPiece.Shape[y][x]]);
                    DrawSquare(x + (int)nextPiecePosition.x, tetrisController.board.Rows - (int)nextPiecePosition.y - y);
                }
            }
        }

        Tetrominoe switchPiece = tetrisController.board.switchPiece;
        if(switchPiece != null)
        {
            for (int y = 0; y < switchPiece.Shape.Length; y++)
            {
                for (int x = 0; x < switchPiece.Shape[y].Length; x++)
                {
                    if (switchPiece.Shape[y][x] != 0)
                    {
                        GL.Color(colors[switchPiece.Shape[y][x]]);
                        DrawSquare(x + (int)nextPiecePosition.x, tetrisController.board.Rows - (int)nextPiecePosition.y - y - 5);
                    }
                }
            }
        }

        GL.End();
    }

    private void StartOpenGLOperations()
    {
        GL.PushMatrix();
        drawMaterial.SetPass(0);
    }

    private void EndOpenGLOperations()
    {
        GL.PopMatrix();
    }

    private void DrawSquare(int x, int y)
    {
        Vector3 bottomLeft = new Vector3(x, y) + (0.9f * drawOffset);
        Vector3 topLeft = bottomLeft + (0.9f * Vector3.up);
        Vector3 topRight = topLeft + (0.9f * Vector3.right);
        Vector3 bottomRight = topRight + (0.9f * Vector3.down);
        GL.Vertex(bottomLeft * squareScale);
        GL.Vertex(topLeft * squareScale);
        GL.Vertex(topRight * squareScale);
        GL.Vertex(bottomRight * squareScale);
    }
}
