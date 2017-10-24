using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextElementControllerScript : MonoBehaviour
{
    private GameObject fpsText;
    private GameObject timerText;
    private GameObject bonusTimeText;
    private GameObject scoreText;
    private GameObject scoreMulti;
    [SerializeField] private GameObject[] arrow;
    private GameObject ringCountText;
    private GameObject speedText;
    private GameObject speedBar;
    //need to add
    private GameObject bestlap_time;
    private GameObject checkpoint_time;
    private GameObject time_difference;
    private GameObject player_list;
    private GameObject current_lap_time;
    private GameObject position_text;
    private GameObject lap_text;
    public Color HUD_Color;

    public struct hudElementsBools
    {
        public bool timerBool;
        public bool scoreBool;
        public bool scoremultiBool;
        public bool fpsBool;
        public bool arrowBool;
        public bool ringCountBool;
        public bool speedBool;
        public bool speedBarBool;
        //need add
        public bool bestlapBool;
        public bool checkpoint_timeBool;
        public bool time_differenceBool;
        public bool player_listBool;
        public bool current_lap_timeBool;
        public bool positionBool;
        public bool lapBool;
        public bool overAllBool;
    }
    public hudElementsBools hudElementsControl;

    public void setTimer(bool isOn) { hudElementsControl.timerBool = isOn; GameSettings.SetBool("HudTimer", isOn); }
    public void setScore(bool isOn) { hudElementsControl.scoreBool = isOn; GameSettings.SetBool("HudScore", isOn); }
    public void setScoreMulti(bool isOn) { hudElementsControl.scoremultiBool = isOn; GameSettings.SetBool("HudScoremulti", isOn); }
    public void setFPS(bool isOn) { hudElementsControl.fpsBool = isOn; GameSettings.SetBool("HudFps", isOn); }
    public void setArrow(bool isOn) { hudElementsControl.arrowBool = isOn; GameSettings.SetBool("HudArrow", isOn); }
    public void setRingCount(bool isOn) { hudElementsControl.ringCountBool = isOn; GameSettings.SetBool("HudRingCount", isOn); }
    public void setSpeed(bool isOn) { hudElementsControl.speedBool = isOn; GameSettings.SetBool("HudSpeed", isOn); }
    public void setSpeedBar(bool isOn) { hudElementsControl.speedBarBool = isOn; GameSettings.SetBool("HudSpeedBar", isOn); }
    //need add
    public void setBestLap(bool isOn) { hudElementsControl.bestlapBool = isOn; GameSettings.SetBool("BestLap", isOn); }
    public void setCheckpoint_time(bool isOn) { hudElementsControl.checkpoint_timeBool = isOn; GameSettings.SetBool("CheckPointTime", isOn); }
    public void setTimeDifference(bool isOn) { hudElementsControl.time_differenceBool = isOn; GameSettings.SetBool("TimeDifference", isOn); }
    public void setPlayerList(bool isOn) { hudElementsControl.player_listBool = isOn; GameSettings.SetBool("PlayerList", isOn); }
    public void setCurrentLapTime(bool isOn) { hudElementsControl.current_lap_timeBool = isOn; GameSettings.SetBool("CurrentLapTime", isOn); }
    public void setPositionText(bool isOn) { hudElementsControl.positionBool = isOn; GameSettings.SetBool("Position", isOn); }
    public void setLapText(bool isOn) { hudElementsControl.lapBool = isOn; GameSettings.SetBool("Lap", isOn); }
    public void setAll(bool isOn)
    {
        setTimer(isOn);
        setScore(isOn);
        setScoreMulti(isOn);
        setFPS(isOn);
        setArrow(isOn);
        setRingCount(isOn);
        setSpeed(isOn);
        setBestLap(isOn);
        setCheckpoint_time(isOn);
        setTimeDifference(isOn);
        setPlayerList(isOn);
        setCurrentLapTime(isOn);
        setPositionText(isOn);
        setLapText(isOn);
        setSpeedBar(isOn);
        hudElementsControl.overAllBool = isOn;
    }

    //For level use
    public void gameStart()
    {
        timerText.SetActive(hudElementsControl.timerBool);
        scoreText.SetActive(hudElementsControl.scoreBool);
        scoreMulti.SetActive(hudElementsControl.scoremultiBool);
        fpsText.SetActive(hudElementsControl.fpsBool);
        for (int i = 0; i < arrow.Length; i++)
        {
            arrow[i].SetActive(hudElementsControl.arrowBool);
        }
        /* bestlap_time.SetActive(hudElementsControl.bestlapBool);
         checkpoint_time.SetActive(hudElementsControl.checkpoint_timeBool);
         time_difference.SetActive(hudElementsControl.time_differenceBool);
         player_list.SetActive(hudElementsControl.player_listBool);
         current_lap_time.SetActive(hudElementsControl.current_lap_timeBool);
         position_text.SetActive(hudElementsControl.positionBool);
         lap_text.SetActive(hudElementsControl.lapBool);*/

        //ringCountText.SetActive(hudElementsControl.ringCountBool);
        speedText.SetActive(hudElementsControl.speedBool);
        speedBar.SetActive(hudElementsControl.speedBarBool);
        // altimeterText.SetActive(hudElementsControl.altimeterBool);
        //debugGUI.SetActive(hudElementsControl.debugGUIBool);


        switch (GameManager.instance.gameMode.currentMode)
        {
            case GameModes.Cursed:
                bonusTimeText.SetActive(hudElementsControl.timerBool);
                break;
            case GameModes.Continuous:
            case GameModes.Free:
                bonusTimeText.SetActive(false);
                break;
            default:
                Debug.LogWarning("Missing case: \"" + GameManager.instance.gameMode.currentMode.ToString("F") + "\"");
                break;
        }

    }

    //For menu's use
    public void menuStart()
    {
        if (fpsText.activeSelf)
        {
            fpsText.SetActive(false);
        }
        if (timerText.activeSelf)
        {
            timerText.SetActive(false);
        }
        if (bonusTimeText.activeSelf)
        {
            bonusTimeText.SetActive(false);
        }
        if (scoreText.activeSelf)
        {
            scoreText.SetActive(false);
        }
        if (scoreMulti.activeSelf)
        {
            scoreMulti.SetActive(false);
        }
        /* if (bestlap_time.activeSelf)
         {
             bestlap_time.SetActive(false);
         }
         if (checkpoint_time.activeSelf)
         {
             checkpoint_time.SetActive(false);
         }
         if (time_difference.activeSelf)
         {
             time_difference.SetActive(false);
         }
         if (player_list.activeSelf)
         {
             player_list.SetActive(false);
         }
         if (current_lap_time.activeSelf)
         {
             current_lap_time.SetActive(false);
         }
         if (position_text.activeSelf)
         {
             position_text.SetActive(false);
         }
         if (lap_text.activeSelf)
         {
             lap_text.SetActive(false);
         }*/
        for (int i = 0; i < arrow.Length; i++)
        {
            if (arrow[i].activeSelf)
            {
                arrow[i].SetActive(false);
            }
        }
        /*if (ringCountText.activeSelf)
         {
             ringCountText.SetActive(false);
         }*/
        if (speedText.activeSelf)
        {
            speedText.SetActive(false);
        }
        if (speedBar.activeSelf)
        {
            speedBar.SetActive(false);
        }
    }


    void setHud(bool isOn)
    {
        if (isOn)
            gameStart();
        else
            menuStart();
    }

    private void OnEnable()
    {
        fpsText = GetComponentInChildren<FPSTextUpdateScript>().gameObject;
        timerText = GetComponentInChildren<TimerTextUpdateScript>().gameObject;
        bonusTimeText = GetComponentInChildren<bonusTimeTextUpdater>().gameObject;
        scoreText = GetComponentInChildren<ScoreTextUpdateScript>().gameObject;
        scoreMulti = GetComponentInChildren<Score_Multi_Script>().gameObject;

        /* bestlap_time = GetComponentInChildren<Best_Lap_Text_script>().gameObject;
         checkpoint_time = GetComponentInChildren<Checkpoint_Time_Text_Script>().gameObject;
         time_difference = GetComponentInChildren<Time_Gap_Text_Script>().gameObject;
         player_list = GetComponentInChildren<Player_List_Text_script>().gameObject;
         current_lap_time = GetComponentInChildren<Current_Lap_Time_Text>().gameObject;
         position_text = GetComponentInChildren<Position_Text_script>().gameObject;
         lap_text = GetComponentInChildren<Lap_Text_script>().gameObject;*/


        //arrow = GetComponentInChildren<arrowPointAtUpdater>().get;
        //ringCountText = GetComponentInChildren<RingCountTextUpdate>().gameObject;
        speedText = GetComponentInChildren<SpeedUpdate>().gameObject;
        speedBar = GetComponentInChildren<speedBarUpdater>().gameObject;

        GetPlayerPrefs();

        EventManager.OnToggleHud += setHud;
    }

    private void OnDisable()
    {
        EventManager.OnToggleHud -= setHud;
    }
    private void GetPlayerPrefs()
    {
        hudElementsControl.timerBool = GameSettings.GetBool("HudTimer", true);
        hudElementsControl.scoreBool = GameSettings.GetBool("HudScore", true);
        hudElementsControl.scoremultiBool = GameSettings.GetBool("HudScoremulti", true);
        hudElementsControl.fpsBool = GameSettings.GetBool("HudFps", true);
        hudElementsControl.arrowBool = GameSettings.GetBool("HudArrow", true);
        hudElementsControl.ringCountBool = GameSettings.GetBool("HudRingCount", true);
        hudElementsControl.speedBool = GameSettings.GetBool("HudSpeed", true);
        hudElementsControl.speedBarBool = GameSettings.GetBool("HudSpeedBar", true);
        hudElementsControl.bestlapBool = GameSettings.GetBool("BestLap", true);
        hudElementsControl.time_differenceBool = GameSettings.GetBool("TimeDifference", true);
        hudElementsControl.player_listBool = GameSettings.GetBool("PlayerList", true);
        hudElementsControl.current_lap_timeBool = GameSettings.GetBool("CurrentLapTime", true);
        hudElementsControl.positionBool = GameSettings.GetBool("Position", true);
        hudElementsControl.checkpoint_timeBool = GameSettings.GetBool("CheckPointTime", true);
        hudElementsControl.lapBool = GameSettings.GetBool("Lap", true);
        hudElementsControl.overAllBool = GameSettings.GetBool("HudOverAll", true);
    }
}