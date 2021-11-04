using Assets.Scripts;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    public Board board;
    public AudioSource theme;
    public AudioSource lineClear;
    public AudioSource pieceLocked;

    private const int UPDATE_INTERVAL = 20;
    private int currentUpdateFrame = 0;

    private void Start()
    {
        board = new Board(20, 10);
        theme.Play();
    }

    private void Update()
    {
        if (!board.Lost)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                board.HardDrop();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                board.Rotate(false);
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                board.Rotate(true);
            }

            
        }
    }

    private void FixedUpdate()
    {
        if (!board.Lost)
        {
            if (currentUpdateFrame % 3 == 0)
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

            if (board.ClearedLines)
            {
                lineClear.Play();
                board.ClearedLines = false;
                Debug.Log(board.Points);
            }
            else if (board.PieceLocked)
            {
                pieceLocked.Play();
                board.PieceLocked = false;
            }

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
        else
        {
            theme.Pause();
        }
    }
}
