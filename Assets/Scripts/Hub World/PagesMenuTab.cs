using UnityEngine;
public class PagesMenuTab : MenuTab
{
    [SerializeField]
    private GameObject[] pages = null;
    [SerializeField]
    private EventSelectedObject previousButton = null, nextButton = null;
    private int currPage = 0;
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
    }
    private void OnDisable()
    {
        previousButton.OnSelectSuccess -= PrevPage;
        nextButton.OnSelectSuccess -= NextPage;
    }
    private void NextPage()
    {
        pages[currPage].SetActive(false);
        ++currPage;
        if (currPage >= pages.Length)
            currPage = 0;
        pages[currPage].SetActive(true);
    }
    private void PrevPage()
    {
        pages[currPage].SetActive(false);
        --currPage;
        if (currPage < 0)
            currPage = pages.Length - 1;
        pages[currPage].SetActive(true);
    }
}