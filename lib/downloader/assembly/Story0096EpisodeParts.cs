// Decompiled with JetBrains decompiler
// Type: Story0096EpisodeParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story0096EpisodeParts : MonoBehaviour
{
  [SerializeField]
  private GameObject DirEpisodenum10;
  [SerializeField]
  private GameObject DirEpisodenum01;
  [SerializeField]
  private GameObject DirEpisodenum1;
  [SerializeField]
  private UIButton IbtnEpisode;
  [SerializeField]
  private UILabel TxtEpisodetitle;
  [SerializeField]
  private GameObject SlcClear;
  [SerializeField]
  private GameObject SlcNew;

  public void setData(
    PlayerCharacterQuestS quest,
    StoryPlaybackCharacterDetail playbackStory,
    NGMenuBase menu,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    int num1 = quest.quest_character_s.priority / 10;
    int num2 = quest.quest_character_s.priority % 10;
    if (num1 <= 0)
      this.DirEpisodenum10.SetActive(false);
    else
      this.DirEpisodenum10.GetComponent<UISprite>().spriteName = "slc_Episode0" + (object) num1 + ".png__GUI__009-6_sozai__009-6_sozai_prefab";
    this.DirEpisodenum01.GetComponent<UISprite>().spriteName = "slc_Episode0" + (object) num2 + ".png__GUI__009-6_sozai__009-6_sozai_prefab";
    EventDelegate.Set(this.IbtnEpisode.onClick, (EventDelegate.Callback) (() =>
    {
      if (menu.IsPushAndSet())
        return;
      Story0093Scene.changeScene009_3(true, playbackStory.script_id, continuousDataList);
    }));
    this.TxtEpisodetitle.SetTextLocalize(playbackStory.name);
    this.SlcClear.SetActive(false);
    this.SlcNew.SetActive(false);
  }

  public void setData(
    PlayerHarmonyQuestS quest,
    StoryPlaybackHarmonyDetail playbackStory,
    NGMenuBase menu,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    int num1 = quest.quest_harmony_s.priority / 10;
    int num2 = quest.quest_harmony_s.priority % 10;
    if (num1 <= 0)
    {
      this.DirEpisodenum10.SetActive(false);
      this.DirEpisodenum01.SetActive(false);
      this.DirEpisodenum1.SetActive(true);
      this.DirEpisodenum1.GetComponent<UISprite>().spriteName = "slc_Episode0" + (object) num2 + ".png__GUI__009-6_sozai__009-6_sozai_prefab";
    }
    else
    {
      this.DirEpisodenum10.SetActive(true);
      this.DirEpisodenum01.SetActive(true);
      this.DirEpisodenum1.SetActive(false);
      this.DirEpisodenum10.GetComponent<UISprite>().spriteName = "slc_Episode0" + (object) num1 + ".png__GUI__009-6_sozai__009-6_sozai_prefab";
      this.DirEpisodenum01.GetComponent<UISprite>().spriteName = "slc_Episode0" + (object) num2 + ".png__GUI__009-6_sozai__009-6_sozai_prefab";
    }
    EventDelegate.Set(this.IbtnEpisode.onClick, (EventDelegate.Callback) (() =>
    {
      if (menu.IsPushAndSet())
        return;
      Story0093Scene.changeScene009_3(true, playbackStory.script_id, continuousDataList);
    }));
    this.TxtEpisodetitle.SetTextLocalize(playbackStory.name);
    this.SlcClear.SetActive(false);
    this.SlcNew.SetActive(false);
  }
}
