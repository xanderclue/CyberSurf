using UnityEngine;
using TMPro;
public class WeatherOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField]
    private TextMeshPro weatherText = null;
    private enum Weather { Sunny, Rainy, Snowy, NumWeathers }
    [SerializeField]
    private Weather defaultWeather = Weather.Sunny;
    private Weather tempWeather;
    private static Weather ActualWeather { get; set; } // replace with game's value
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing WeatherOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing WeatherOptions.rightButton");
        if (null == weatherText)
            Debug.LogWarning("Missing WeatherOptions.weatherText");
    }
    new private void OnEnable()
    {
        base.OnEnable();
        leftButton.OnButtonPressed += ButtonLeftFunction;
        rightButton.OnButtonPressed += ButtonRightFunction;
    }
    private void OnDisable()
    {
        leftButton.OnButtonPressed -= ButtonLeftFunction;
        rightButton.OnButtonPressed -= ButtonRightFunction;
    }
    private void ButtonLeftFunction()
    {
        if (0 == tempWeather)
            tempWeather = Weather.NumWeathers - 1;
        else
            --tempWeather;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        ++tempWeather;
        if (Weather.NumWeathers == tempWeather)
            tempWeather = 0;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        weatherText.SetText(tempWeather.ToString());
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        ActualWeather = tempWeather;
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempWeather = defaultWeather;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempWeather = ActualWeather;
        UpdateDisplay();
    }
}