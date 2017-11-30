using UnityEngine;
public class Name : MonoBehaviour
{
    [SerializeField] private Letter[] letters = null;
    public string NameString
    {
        get
        {
            string retVal = "";
            foreach (Letter letter in letters)
                retVal += letter.CharValue;
            return retVal;
        }
        set
        {
            int len = System.Math.Min(letters.Length, value.Length);
            for (int i = 0; i < len; ++i)
                letters[i].CharValue = value[i];
        }
    }
    public delegate void NameChangedEvent();
    public event NameChangedEvent OnNameChanged;
    private void NameChanged() => OnNameChanged?.Invoke();
    private void OnEnable()
    {
        for (int i = 0; i < letters.Length; ++i)
        {
            letters[i].CharValue = GameSettings.GetChar("Name_" + i, letters[i].CharValue);
            letters[i].OnLetterChanged += NameChanged;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < letters.Length; ++i)
        {
            letters[i].OnLetterChanged -= NameChanged;
            GameSettings.SetChar("Name_" + i, letters[i].CharValue);
        }
    }
}