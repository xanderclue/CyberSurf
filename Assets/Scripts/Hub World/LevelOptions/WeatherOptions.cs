using UnityEngine;
using TMPro;
public class WeatherOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro weatherText = null;
    [SerializeField] private WeatherScript.Weather defaultWeather = WeatherScript.Weather.Sunny;
    private WeatherScript.Weather tempWeather;
    private void OnEnable()
    {
        leftButton.OnButtonPressed += ButtonLeftFunction;
        rightButton.OnButtonPressed += ButtonRightFunction;
    }
    private void OnDisable()
    {
        leftButton.OnButtonPressed -= ButtonLeftFunction;
        rightButton.OnButtonPressed -= ButtonRightFunction;
    }
    public override void EnableGroup()
    {
        base.EnableGroup();
        leftButton.enabled = true;
        rightButton.enabled = true;
    }
    public override void DisableGroup()
    {
        base.DisableGroup();
        leftButton.enabled = false;
        rightButton.enabled = false;
    }
    private void ButtonLeftFunction()
    {
        if (0 == tempWeather)
            tempWeather = WeatherScript.Weather.NumWeathers - 1;
        else
            --tempWeather;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        ++tempWeather;
        if (WeatherScript.Weather.NumWeathers == tempWeather)
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
        WeatherScript.currentWeather = tempWeather;
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
        tempWeather = WeatherScript.currentWeather;
        UpdateDisplay();
    }
}