using UnityEngine;
using TMPro;
public class PagesMenuTab : MenuTab
{
    [SerializeField] private GameObject[] pages = null;
    [SerializeField] private EventSelectedObject previousButton = null;
    [SerializeField] private EventSelectedObject nextButton = null;
    [SerializeField, LabelOverride("Page Number")] private TextMeshPro pageText = null;
    [SerializeField] private bool dontLoop = false;
    private int currPage = 0;
    public delegate void PageChangeEvent();
    public event PageChangeEvent OnPageChanged;
    new private void Awake()
    {
        base.Awake();
        if (null == pages)
            pages = new GameObject[1];
        for (int i = 0; i < pages.Length; ++i)
            if (null == pages[i])
                pages[i] = new GameObject();
    }
    private void OnEnable()
    {
        currPage = 0;
        pages[0].SetActive(true);
        for (int i = 1; i < pages.Length; ++i)
            pages[i].SetActive(false);
        previousButton.OnSelectSuccess += PrevPage;
        nextButton.OnSelectSuccess += NextPage;
        OnPageChanged += UpdateText;
        OnPageChanged?.Invoke();
    }
    private void OnDisable()
    {
        previousButton.OnSelectSuccess -= PrevPage;
        nextButton.OnSelectSuccess -= NextPage;
        OnPageChanged -= UpdateText;
    }
    private void UpdateText()
    {
        if (null != pageText)
            pageText.SetText((currPage + 1).ToString() + "/" + pages.Length.ToString());
    }
    private void NextPage()
    {
        pages[currPage].SetActive(false);
        ++currPage;
        if (currPage >= pages.Length)
            currPage = dontLoop ? pages.Length - 1 : 0;
        pages[currPage].SetActive(true);
        OnPageChanged?.Invoke();
    }
    private void PrevPage()
    {
        pages[currPage].SetActive(false);
        --currPage;
        if (currPage < 0)
            currPage = dontLoop ? 0 : pages.Length - 1;
        pages[currPage].SetActive(true);
        OnPageChanged?.Invoke();
    }
}