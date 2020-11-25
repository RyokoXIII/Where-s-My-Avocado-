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

    public float seesawPosX1, seesawPosX2, seesawPosX3, seesawPosX4;
    public float seesawPosY1, seesawPosY2, seesawPosY3, seesawPosY4;
    public float seesawScaleX, seesawScaleY;
    public float seesawRotateZ1, seesawRotateZ2, seesawRotateZ3, seesawRotateZ4;

    public int bigWoodNum, woodNestNum;

    public float bigWoodPosX1, bigWoodPosX2, bigWoodPosX3, bigWoodPosX4, bigWoodPosX5, bigWoodPosX6, bigWoodPosX7, bigWoodPosX8;
    public float bigWoodPosY1, bigWoodPosY2, bigWoodPosY3, bigWoodPosY4, bigWoodPosY5, bigWoodPosY6, bigWoodPosY7, bigWoodPosY8;
    public float bigWoodScaleX1, bigWoodScaleX2, bigWoodScaleX3, bigWoodScaleX4, bigWoodScaleX5, bigWoodScaleX6, bigWoodScaleX7, bigWoodScaleX8;
    public float bigWoodScaleY1, bigWoodScaleY2, bigWoodScaleY3, bigWoodScaleY4, bigWoodScaleY5, bigWoodScaleY6, bigWoodScaleY7, bigWoodScaleY8;
    public float bigWoodRotateZ1, bigWoodRotateZ2, bigWoodRotateZ3, bigWoodRotateZ4, bigWoodRotateZ5, bigWoodRotateZ6, bigWoodRotateZ7, bigWoodRotateZ8;

    public float deadZonePosX, deadZonePosY;
    public float deadZoneScaleX, deadZoneScaleY;

    public bool rotatePlayer, rotateBoss, rotateEnemy;
    public bool roundLog, deadZone, wood, seesaw, bigWood, woodNest;
    //public bool sunset_music;
    //public bool beach_music;

    public bool tutorial;
}
