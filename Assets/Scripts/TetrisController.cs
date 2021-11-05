using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TetrisController : MonoBehaviour
{
    public Board board;
    public AudioSource theme;
    public AudioSource lineClear;
    public AudioSource pieceLocked;
    public Text pointsText;
    public Text levelsText;

    private int updateInterval = 20;
    private int currentUpdateFrame = 0;

    private readonly int[] levelTable =
    {
        //Frames per level
        48, //Level - 0
        43, //Level - 1
        38, //Level - 2
        33, //Level - 3
        28, //Level - 4
        23, //Level - 5
        18, //Level - 6
        13, //Level - 7
        8, //Level - 8
        6, //Level - 9
        5, //Level - 10
        5, //Level - 11
        5, //Level - 12
        4, //Level - 13
        4, //Level - 14
        4, //Level - 15
        3, //Level - 16
        3, //Level - 17
        3, //Level - 18
        2, //Level - 19
        2, //Level - 20
        2, //Level - 21
        2, //Level - 22
        2, //Level - 23
        2, //Level - 24
        2, //Level - 25
        2, //Level - 26
        2, //Level - 27
        2, //Level - 28
        1 //Level - 29+
    };

    private void Start()
    {
        board = new Board(20, 10);
        updateInterval = levelTable[board.Level];
        levelsText.text = "Levels: " + board.Level.ToString();
        pointsText.text = "Points: " + board.Points.ToString();
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
                updateInterval = levelTable[Math.Min(board.Level, 29)];
                currentUpdateFrame = 0;
                levelsText.text = "Levels: " + board.Level.ToString();
                pointsText.text = "Points: " + board.Points.ToString();
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

            if (currentUpdateFrame == updateInterval)
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
