// Decompiled with JetBrains decompiler
// Type: Story009132ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story009132ScrollParts : MonoBehaviour
{
  [SerializeField]
  private UISprite IbtnChapterSprite;
  [SerializeField]
  private UIButton IbtnChapter;
  [SerializeField]
  private UISprite SlcNew;
  [SerializeField]
  private UISprite SlcClear;
  [SerializeField]
  private UILabel TxtChapter;
  [SerializeField]
  private GameObject SlcLastPlay;
  private StoryPlaybackSeaDetail story;
  private NGMenuBase menu;
  private List<Story0093Scene.ContinuousData> continuousDataList;

  public void onClickEpisodeButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Story0093Scene.changeScene(true, this.story.script_id, new bool?(true), continuousDataList: this.continuousDataList);
  }

  public void Init(
    Story009132Menu menu,
    StoryPlaybackSeaDetail story,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    this.menu = (NGMenuBase) menu;
    this.story = story;
    this.continuousDataList = continuousDataList;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickEpisodeButton));
    ((Component) this.SlcNew).gameObject.SetActive(false);
    ((Component) this.SlcClear).gameObject.SetActive(false);
    this.TxtChapter.SetTextLocalize(story.name);
    if (PlayerPrefs.GetInt("SeaLastScriptId", -1) != story.script_id)
      return;
    this.SlcLastPlay.SetActive(true);
  }
}
