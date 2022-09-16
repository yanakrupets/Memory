using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static int CardPairs { get; set; } = 9;

    public static int PlayersCount { get; set; } = 2;

    public static List<string> PlayersNames = new List<string>();
}
