﻿// Decompiled with JetBrains decompiler
// Type: Celeste.Achievements
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

//using Steamworks; // STEAM SHIT!

namespace Celeste
{
    public static class Achievements
    {
        public static string ID(Achievement achievement) => achievement.ToString();

        public static bool Has(Achievement achievement)
        {
            bool pbAchieved;
            //return SteamUserStats.GetAchievement(Achievements.ID(achievement), out pbAchieved) & pbAchieved;  // STEAM SHIT!
            return false;
        }

        public static void Register(Achievement achievement)
        {
            if (Achievements.Has(achievement))
                return;
            // SteamUserStats.SetAchievement(Achievements.ID(achievement));  // STEAM SHIT!
            Stats.Store();
        }
    }
}