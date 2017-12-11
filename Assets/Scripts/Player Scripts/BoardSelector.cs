using UnityEngine;
public class BoardSelector : MonoBehaviour
{
    [SerializeField] GameObject[] boards = null;
    public void SelectBoard(BoardType board)
    {
        for (int i = 0; i < boards.Length; ++i)
            boards[i].SetActive(i == (int)board);
    }
    public GameObject CurrentBoard
    {
        get
        {
            foreach (GameObject board in boards)
                if (board.activeSelf)
                    return board;
            return null;
        }
    }
}