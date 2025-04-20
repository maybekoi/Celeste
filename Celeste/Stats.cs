// Decompiled with JetBrains decompiler
// Type: Celeste.Stats
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using Steamworks;
using System.Collections.Generic;

#nullable disable
namespace Celeste;

public static class Stats
{
  private static Dictionary<Stat, string> statToString = new Dictionary<Stat, string>();
  private static bool ready;

  public static void MakeRequest()
  {
    Stats.ready = SteamUserStats.RequestCurrentStats();
    SteamUserStats.RequestGlobalStats(0);
  }

  public static bool Has() => Stats.ready;

  public static void Increment(Stat stat, int increment = 1)
  {
    if (!Stats.ready)
      return;
    string pchName = (string) null;
    if (!Stats.statToString.TryGetValue(stat, out pchName))
      Stats.statToString.Add(stat, pchName = stat.ToString());
    int pData;
    if (!SteamUserStats.GetStat(pchName, out pData))
      return;
    SteamUserStats.SetStat(pchName, pData + increment);
  }

  public static int Local(Stat stat)
  {
    int pData = 0;
    if (Stats.ready)
    {
      string pchName = (string) null;
      if (!Stats.statToString.TryGetValue(stat, out pchName))
        Stats.statToString.Add(stat, pchName = stat.ToString());
      SteamUserStats.GetStat(pchName, out pData);
    }
    return pData;
  }

  public static long Global(Stat stat)
  {
    long pData = 0;
    if (Stats.ready)
    {
      string pchStatName = (string) null;
      if (!Stats.statToString.TryGetValue(stat, out pchStatName))
        Stats.statToString.Add(stat, pchStatName = stat.ToString());
      SteamUserStats.GetGlobalStat(pchStatName, out pData);
    }
    return pData;
  }

  public static void Store()
  {
    if (!Stats.ready)
      return;
    SteamUserStats.StoreStats();
  }

  public static string Name(Stat stat) => Dialog.Clean("STAT_" + stat.ToString());
}
