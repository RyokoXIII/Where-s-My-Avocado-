using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource beachMusicBackground, selectFX, backFX, enemySlashFX, bossSlashFX,
        victoryFX, loseFX, titleMusic, showStarFX, waterSplashFX;
}
