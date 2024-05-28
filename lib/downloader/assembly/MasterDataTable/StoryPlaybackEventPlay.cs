// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackEventPlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackEventPlay
  {
    public int ID;
    public string scene_name;
    public int? arg1;
    public int script_id;
    public DateTime start_at;
    public DateTime end_at;

    public static StoryPlaybackEventPlay Parse(MasterDataReader reader)
    {
      return new StoryPlaybackEventPlay()
      {
        ID = reader.ReadInt(),
        scene_name = reader.ReadStringOrNull(true),
        arg1 = reader.ReadIntOrNull(),
        script_id = reader.ReadInt(),
        start_at = reader.ReadDateTime(),
        end_at = reader.ReadDateTime()
      };
    }

    public static int[] GetPlayIDList(DateTime serverTime, string sceneName)
    {
      IOrderedEnumerable<StoryPlaybackEventPlay> orderedEnumerable = ((IEnumerable<StoryPlaybackEventPlay>) MasterData.StoryPlaybackEventPlayList).Where<StoryPlaybackEventPlay>((Func<StoryPlaybackEventPlay, bool>) (x => x.scene_name == sceneName & x.start_at <= serverTime && x.end_at >= serverTime)).OrderBy<StoryPlaybackEventPlay, DateTime>((Func<StoryPlaybackEventPlay, DateTime>) (x => x.start_at));
      List<int> intList = new List<int>();
      foreach (StoryPlaybackEventPlay playbackEventPlay in (IEnumerable<StoryPlaybackEventPlay>) orderedEnumerable)
        intList.Add(playbackEventPlay.ID);
      return intList.ToArray();
    }
  }
}
