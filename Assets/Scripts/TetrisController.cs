using Assets.Scripts;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    public Board board;

    private const int UPDATE_INTERVAL = 8;
    private int currentUpdateFrame = 0;

    private void Start()
    {
        board = new Board(20, 10);
    }

    private void FixedUpdate()
    {
        if (currentUpdateFrame == 0)
        {
            board.Update();
        }

        currentUpdateFrame++;

        if (currentUpdateFrame == UPDATE_INTERVAL)
        {
            currentUpdateFrame = 0;
        }
    }
}
