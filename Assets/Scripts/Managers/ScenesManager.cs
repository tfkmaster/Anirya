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
}

public struct tuto_002
{
    public bool TrunkIsBroken;
    public bool XAttackDisplay;
}

public struct tuto_003
{
    public bool FirstWolfSlayed;
}

public class ScenesManager : MonoBehaviour
{
    public PlayerData player_data;
    public tuto_001 tuto_001;
    public tuto_002 tuto_002;
    public tuto_003 tuto_003;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Player datas
        player_data.AlimMet = false;

        //tuto_001 datas
        tuto_001.ButterflyCinematic = true;
        tuto_001.TutorialGuide = true;

        //tuto_002
        tuto_002.TrunkIsBroken = false;
        tuto_002.XAttackDisplay = false;

        //tuto_003
        tuto_003.FirstWolfSlayed = false;
    }
}
