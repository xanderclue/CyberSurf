using UnityEngine;
public class BoardSelector : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] boards = null;
    public void SelectBoard(BoardType board)
    {
        for (int i = 0; i < boards.Length; ++i)
            boards[i].enabled = i == (int)board;
    }
    public Transform CurrentBoard
    {
        get
        {
            foreach (MeshRenderer board in boards)
                if (board.enabled)
                    return board.transform;
            return transform;
        }
    }
}