using Assets.Scripts;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    public Board board;

    private const int UPDATE_INTERVAL = 20;
    private int currentUpdateFrame = 0;

    private void Start()
    {
        board = new Board(20, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            board.HardDrop();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            board.Rotate(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            board.Rotate(true);
        }
    }

    private void FixedUpdate()
    {
        if (currentUpdateFrame == 0)
        {
            board.Update();
        }

        if(currentUpdateFrame % 2 == 0)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                board.Move(-1, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                board.Move(1, 0);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                board.Move(0, 1);
            }
        }

        currentUpdateFrame++;

        if (currentUpdateFrame == UPDATE_INTERVAL)
        {
            currentUpdateFrame = 0;
        }
    }
}
