using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public bool AlimMet;
}

public struct tuto_001
{
    public bool ButterflyCinematic;
    public bool TutorialGuide;
    public bool TrunkIsBroken;
}

public struct tuto_002
{
    public bool TrunkIsBroken;
    public bool XAttackDisplay;
}

public struct tuto_005
{
    public bool FirstWolfSlayed;
    public bool FirstWolfEncountered;
}

public struct hub
{
    
}

public struct chap001_004
{
    public bool Visited;
}

public class ScenesManager : MonoBehaviour
{
    public PlayerData player_data;
    public tuto_001 tuto_001;
    public tuto_002 tuto_002;
    public tuto_005 tuto_005;
    public hub hub;
    public chap001_004 chap001_004;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Player datas
        player_data.AlimMet = false;

        //tuto_001 datas
        tuto_001.ButterflyCinematic = true;
        tuto_001.TutorialGuide = true;
        tuto_001.TrunkIsBroken = false;

        //tuto_002
        tuto_002.TrunkIsBroken = false;
        tuto_002.XAttackDisplay = false;

        //tuto_005
        tuto_005.FirstWolfSlayed = false;
        tuto_005.FirstWolfEncountered = false;

        //chap001_004
        chap001_004.Visited = false;
    }
}
