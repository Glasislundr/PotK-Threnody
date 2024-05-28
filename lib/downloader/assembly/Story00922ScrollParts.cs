// Decompiled with JetBrains decompiler
// Type: Story00922ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story00922ScrollParts : MonoBehaviour
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
  private StoryPlaybackStoryDetail story;
  private NGMenuBase menu;
  private List<Story0093Scene.ContinuousData> continuousDataList;
  private int XL;

  public void onClickEpisodeButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Story0093Scene.changeScene009_3(true, this.story.script_id, this.continuousDataList, this.XL);
  }

  public void Init(
    Story00922Menu menu,
    StoryPlaybackStoryDetail story,
    List<Story0093Scene.ContinuousData> continuousDataList,
    int XL)
  {
    this.menu = (NGMenuBase) menu;
    this.story = story;
    this.XL = XL;
    this.continuousDataList = continuousDataList;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickEpisodeButton));
    ((Component) this.SlcNew).gameObject.SetActive(false);
    ((Component) this.SlcClear).gameObject.SetActive(false);
    this.TxtChapter.SetTextLocalize(story.name);
    int num = -1;
    switch (XL)
    {
      case 1:
        num = PlayerPrefs.GetInt("HeavenLastScriptId", -1);
        break;
      case 4:
        num = PlayerPrefs.GetInt("LostLastScriptId", -1);
        break;
      case 6:
        num = PlayerPrefs.GetInt("IntegralLastScriptId", -1);
        break;
      case 7:
        num = PlayerPrefs.GetInt("EverafterLastScriptId", -1);
        break;
    }
    if (num != story.script_id)
      return;
    this.SlcLastPlay.SetActive(true);
  }
}
