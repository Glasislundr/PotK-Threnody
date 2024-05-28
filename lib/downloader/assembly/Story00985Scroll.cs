// Decompiled with JetBrains decompiler
// Type: Story00985Scroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story00985Scroll : MonoBehaviour
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
  private int script_id;
  private NGMenuBase menu;
  private List<Story0093Scene.ContinuousData> continuousDataList;

  public void onClickEpisodeButton()
  {
    if (this.menu.IsPushAndSet())
      return;
    Story0093Scene.changeScene009_3(true, this.script_id, this.continuousDataList, true);
  }

  public void Init(
    string name,
    int script_id,
    NGMenuBase menu,
    List<Story0093Scene.ContinuousData> continuousDataList)
  {
    this.script_id = script_id;
    this.menu = menu;
    this.continuousDataList = continuousDataList;
    EventDelegate.Add(this.IbtnChapter.onClick, new EventDelegate.Callback(this.onClickEpisodeButton));
    this.TxtChapter.SetTextLocalize(name);
    if (PlayerPrefs.GetInt("ExtraLastScriptId", -1) != script_id)
      return;
    this.SlcLastPlay.SetActive(true);
  }
}
