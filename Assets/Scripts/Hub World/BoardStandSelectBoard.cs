using UnityEngine;

public class BoardStandSelectBoard : SelectedObject
{
    [SerializeField] private Color emissionColor = Color.black;
    [SerializeField] private Transform boardCopy = null;
    private static Transform playerBoard = null;
    private BoardStandProperties selectionVariables = null;
    private Material renderMat = null;
    private bool animationRunning = false;
    private float tVal = 0.0f, invAnimationTime = 1.0f;
    private Vector3 startPosition, startScale;
    private Quaternion startRotation, ogLocalRot;
    new private void Start()
    {
        base.Start();
        selectionVariables = GetComponentInParent<BoardStandProperties>();
        renderMat = GetComponent<MeshRenderer>().material;
        renderMat.SetColor("_EmissionColor", Color.black);
        ogLocalRot = boardCopy.localRotation;
        if (null == playerBoard)
            playerBoard = GameManager.player.GetComponentsInChildren<BoardRollEffect>()[0].transform;
    }
    protected override void SelectedFunction()
    {
        base.SelectedFunction();
        renderMat.SetColor("_EmissionColor", emissionColor);
    }
    protected override void DeselectedFunction()
    {
        base.DeselectedFunction();
        renderMat.SetColor("_EmissionColor", Color.black);
    }
    public override void SuccessFunction()
    {
        StartAnimation();
    }
    private void StartAnimation()
    {
        boardCopy.localPosition = Vector3.zero;
        boardCopy.localRotation = Quaternion.Euler(-90.0f, 0.0f, 180.0f);
        boardCopy.localScale = Vector3.one;
        boardCopy.parent = null;
        startPosition = boardCopy.position;
        startRotation = boardCopy.rotation;
        startScale = boardCopy.lossyScale;
        boardCopy.gameObject.SetActive(true);
        invAnimationTime = 1.0f / WaitTime;
        tVal = 0.0f;
        animationRunning = true;
    }
    private void OnEndAnimation()
    {
        animationRunning = false;
        boardCopy.parent = transform;
        boardCopy.gameObject.SetActive(false);
        GameManager.instance.boardScript.BoardSelect(selectionVariables.boardType);
        EventManager.OnCallBoardMenuEffects();
    }
    new private void Update()
    {
        base.Update();
        if (animationRunning)
        {
            boardCopy.position = Vector3.Lerp(startPosition, playerBoard.position, tVal);
            boardCopy.rotation = Quaternion.Slerp(startRotation, playerBoard.rotation, tVal);
            boardCopy.localScale = Vector3.Lerp(startScale, playerBoard.lossyScale, tVal);
            tVal += Time.deltaTime * invAnimationTime;
            if (tVal >= 1.0f)
                OnEndAnimation();
        }
    }
}