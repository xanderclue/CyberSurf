using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextElementControllerScript : MonoBehaviour {
    GameObject fpsText;
    GameObject timerText;
    GameObject bonusTimeText;
    GameObject scoreText;
    GameObject scoreMulti;
    [SerializeField] GameObject[] arrow;
    GameObject ringCountText;
    GameObject speedText;
    GameObject speedBar;
    //need to add
    GameObject bestlap_time;
    GameObject checkpoint_time;
    GameObject time_difference;
    GameObject player_list;
    GameObject current_lap_time;
    GameObject position_text;
    GameObject lap_text;
   // GameObject altimeterText;
    //GameObject debugGUI;

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
        //
       // public bool altimeterBool;
        public bool debugGUIBool;
        public bool overAllBool;
    }
    public hudElementsBools hudElementsControl;

    public void setTimer(bool isOn) { hudElementsControl.timerBool = isOn; PlayerPrefs.SetInt("HudTimerBool", isOn ? 1 : 0); }
    public void setScore(bool isOn) { hudElementsControl.scoreBool = isOn; PlayerPrefs.SetInt("HudScoreBool", isOn ? 1 : 0); }
    public void setScoreMulti(bool isOn) { hudElementsControl.scoremultiBool = isOn; PlayerPrefs.SetInt("HudScoremultiBool", isOn ? 1 : 0); }
    public void setFPS(bool isOn) { hudElementsControl.fpsBool = isOn; PlayerPrefs.SetInt("HudFpsBool", isOn ? 1 : 0); }
    public void setArrow(bool isOn) { hudElementsControl.arrowBool = isOn; PlayerPrefs.SetInt("HudArrowBool", isOn ? 1 : 0); }
    public void setRingCount(bool isOn) { hudElementsControl.ringCountBool = isOn; PlayerPrefs.SetInt("HudRingCountBool", isOn ? 1 : 0); }
    public void setSpeed(bool isOn) { hudElementsControl.speedBool = isOn; PlayerPrefs.SetInt("HudSpeedBool", isOn ? 1 : 0); }
    public void setSpeedBar(bool isOn) { hudElementsControl.speedBarBool = isOn; PlayerPrefs.SetInt("HudSpeedBarBool", isOn ? 1 : 0); }
    //need add
    public void setBestLap(bool isOn) { hudElementsControl.bestlapBool = isOn; PlayerPrefs.SetInt("BestLapBool", isOn ? 1 : 0); }
    public void setCheckpoint_time(bool isOn) { hudElementsControl.checkpoint_timeBool = isOn; PlayerPrefs.SetInt("CheckPointTimeBool", isOn ? 1 : 0); }
    public void setTimeDifference(bool isOn) { hudElementsControl.time_differenceBool = isOn; PlayerPrefs.SetInt("TimeDifferenceBool", isOn ? 1 : 0); }
    public void setPlayerList(bool isOn) { hudElementsControl.player_listBool = isOn; PlayerPrefs.SetInt("PlayerListBool", isOn ? 1 : 0); }
    public void setCurrentLapTime(bool isOn) { hudElementsControl.current_lap_timeBool = isOn; PlayerPrefs.SetInt("CurrentLapTimeBool", isOn ? 1 : 0); }
    public void setPositionText(bool isOn) { hudElementsControl.positionBool = isOn; PlayerPrefs.SetInt("PositionBool", isOn ? 1 : 0); }
    public void setLapText(bool isOn) { hudElementsControl.lapBool = isOn; PlayerPrefs.SetInt("LapBool", isOn ? 1 : 0); }
    //public void setAltimeter(bool isOn) { hudElementsControl.altimeterBool = isOn; PlayerPrefs.SetInt("HudAltimeterBool", isOn ? 1 : 0); }
    public void setDebugGUI(bool isOn) { hudElementsControl.debugGUIBool = isOn; PlayerPrefs.SetInt("HudDebugGUIBool", isOn ? 1 : 0); }
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
        //setAltimeter(isOn);
        setDebugGUI(isOn);
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
        bestlap_time.SetActive(hudElementsControl.bestlapBool);
        checkpoint_time.SetActive(hudElementsControl.checkpoint_timeBool);
        time_difference.SetActive(hudElementsControl.time_differenceBool);
        player_list.SetActive(hudElementsControl.player_listBool);
        current_lap_time.SetActive(hudElementsControl.current_lap_timeBool);
        position_text.SetActive(hudElementsControl.positionBool);
        lap_text.SetActive(hudElementsControl.lapBool);
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
        if (bestlap_time.activeSelf)
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
        }
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
       /* if (altimeterText.activeSelf)
        {
            altimeterText.SetActive(false);
        }*/
        /*if (debugGUI.activeSelf)
        {
            debugGUI.SetActive(false);
        }*/
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
      
        bestlap_time = GetComponentInChildren<Best_Lap_Text_script>().gameObject;
        checkpoint_time = GetComponentInChildren<Checkpoint_Time_Text_Script>().gameObject;
        time_difference = GetComponentInChildren<Time_Gap_Text_Script>().gameObject;
        player_list = GetComponentInChildren<Player_List_Text_script>().gameObject;
        current_lap_time = GetComponentInChildren<Current_Lap_Time_Text>().gameObject;
        position_text = GetComponentInChildren<Position_Text_script>().gameObject;
        lap_text = GetComponentInChildren<Lap_Text_script>().gameObject;


        //arrow = GetComponentInChildren<arrowPointAtUpdater>().get;
        //ringCountText = GetComponentInChildren<RingCountTextUpdate>().gameObject;
        speedText = GetComponentInChildren<SpeedUpdate>().gameObject;
        speedBar = GetComponentInChildren<speedBarUpdater>().gameObject;
        //altimeterText = GetComponentInChildren<altimeterTextUpdater>().gameObject;
        //debugGUI = GameObject.Find("GUI");

        GetPlayerPrefs();

        EventManager.OnToggleHud += setHud;
    }

    private void OnDisable()
    {
        EventManager.OnToggleHud -= setHud;
    }
    private void GetPlayerPrefs()
    {
        hudElementsControl.timerBool = (0 != PlayerPrefs.GetInt("HudTimerBool", 1));
        hudElementsControl.scoreBool = (0 != PlayerPrefs.GetInt("HudScoreBool", 1));
        hudElementsControl.scoremultiBool = (0 != PlayerPrefs.GetInt("HudScoremultiBool", 1));
        hudElementsControl.fpsBool = (0 != PlayerPrefs.GetInt("HudFpsBool", 1));
        hudElementsControl.arrowBool = (0 != PlayerPrefs.GetInt("HudArrowBool", 1));
        hudElementsControl.ringCountBool = (0 != PlayerPrefs.GetInt("HudRingCountBool", 1));
        hudElementsControl.speedBool = (0 != PlayerPrefs.GetInt("HudSpeedBool", 1));
        hudElementsControl.speedBarBool = (0 != PlayerPrefs.GetInt("HudSpeedBarBool", 1));
        //hudElementsControl.altimeterBool = (0 != PlayerPrefs.GetInt("HudAltimeterBool", 1));
        //hudElementsControl.debugGUIBool = (0 != PlayerPrefs.GetInt("HudDebugGUIBool", 1));
        hudElementsControl.bestlapBool = (0 != PlayerPrefs.GetInt("BestLapBool", 1));
        hudElementsControl.time_differenceBool = (0 != PlayerPrefs.GetInt("TimeDifferenceBool", 1));
        hudElementsControl.player_listBool = (0 != PlayerPrefs.GetInt("PlayerListBool",1));
        hudElementsControl.current_lap_timeBool = (0 != PlayerPrefs.GetInt("CurrentLapTimeBool", 1));
        hudElementsControl.positionBool = (0 != PlayerPrefs.GetInt("PositionBool", 1));
        hudElementsControl.checkpoint_timeBool = (0 != PlayerPrefs.GetInt("CheckPointTimeBool", 1));
        hudElementsControl.lapBool = (0 != PlayerPrefs.GetInt("LapBool", 1));
        hudElementsControl.overAllBool = (0 != PlayerPrefs.GetInt("HudOverAllBool", 1));
        PlayerPrefs.SetInt("HudTimerBool", hudElementsControl.timerBool ? 1 : 0);
        PlayerPrefs.SetInt("HudScoreBool", hudElementsControl.scoreBool ? 1 : 0);
        PlayerPrefs.SetInt("HudScoremultiBool", hudElementsControl.scoremultiBool ? 1 : 0);
        PlayerPrefs.SetInt("HudFpsBool", hudElementsControl.fpsBool ? 1 : 0);
        PlayerPrefs.SetInt("HudArrowBool", hudElementsControl.arrowBool ? 1 : 0);
        PlayerPrefs.SetInt("HudRingCountBool", hudElementsControl.ringCountBool ? 1 : 0);
        PlayerPrefs.SetInt("HudSpeedBool", hudElementsControl.speedBool ? 1 : 0);
        PlayerPrefs.SetInt("HudSpeedBarBool", hudElementsControl.speedBarBool ? 1 : 0);
        PlayerPrefs.SetInt("BestLapBool", hudElementsControl.bestlapBool ? 1 : 0);
        PlayerPrefs.SetInt("CheckPointTimeBool", hudElementsControl.checkpoint_timeBool ? 1 : 0);
        PlayerPrefs.SetInt("TimeDifferenceBool", hudElementsControl.time_differenceBool ? 1 : 0);
        PlayerPrefs.SetInt("PlayerListBool", hudElementsControl.player_listBool ? 1 : 0);
        PlayerPrefs.SetInt("CurrentLapTimeBool", hudElementsControl.current_lap_timeBool ? 1 : 0);
        PlayerPrefs.SetInt("PositionBool", hudElementsControl.positionBool ? 1 : 0);
        PlayerPrefs.SetInt("LapBool", hudElementsControl.lapBool ? 1 : 0);

        //PlayerPrefs.SetInt("HudAltimeterBool", hudElementsControl.altimeterBool ? 1 : 0);
        //PlayerPrefs.SetInt("HudDebugGUIBool", hudElementsControl.debugGUIBool ? 1 : 0);

        PlayerPrefs.SetInt("HudOverAllBool", hudElementsControl.overAllBool ? 1 : 0);
        PlayerPrefs.Save();
    }
}
