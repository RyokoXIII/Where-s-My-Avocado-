using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelID;

    public float playerPosX, playerPosY;
    public float npcPosX, npcPosY;
    public float npcParticlePosX, npcParticlePosY;

    public float starPosX_1, starPosY_1;
    public float starPosX_2, starPosY_2;
    public float starPosX_3, starPosY_3;

    public float roundLogPosX, roundLogPosY;
    public float roundLogScaleX, roundLogScaleY, roundLogScaleZ;

    public int woodNum;

    public float woodPosX1, woodPosX2, woodPosX3, woodPosX4, woodPosX5, woodPosX6;
    public float woodPosY1, woodPosY2, woodPosY3, woodPosY4, woodPosY5, woodPosY6;

    public int seesawNum;

    public float seesawPosX1, seesawPosX2;
    public float seesawPosY1, seesawPosY2;
    public float seesawScaleX, seesawScaleY;

    public float deadZonePosX, deadZonePosY;
    public float deadZoneScaleX, deadZoneScaleY;

    public bool roundLog, deadZone, wood, seesaw;
    public bool beach_background;
    public bool sunset_background;
    public bool sunset_music;
    public bool beach_music;

    public bool tutorial;
}
