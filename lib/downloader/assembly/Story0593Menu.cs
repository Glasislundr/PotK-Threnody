// Decompiled with JetBrains decompiler
// Type: Story0593Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Story0593Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel mTitleLabel;
  [SerializeField]
  private UIGrid mGrid;
  [SerializeField]
  private UIScrollView mScrollView;
  [SerializeField]
  private GameObject scrollMainPanel;
  private GameObject ScrollParts;

  public IEnumerator InitAsync()
  {
    Future<GameObject> ScrollPartsF = Res.Prefabs.story059_3.story0593scroll.Load<GameObject>();
    IEnumerator e = ScrollPartsF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollParts = ScrollPartsF.Result;
  }

  public IEnumerator StartAsync(EarthQuestChapter chapter, List<Story059ItemData> itemList)
  {
    this.mTitleLabel.SetTextLocalize(chapter.chapter_name);
    foreach (Story059ItemData story059ItemData in itemList)
      this.ScrollParts.CloneAndGetComponent<Story0593ScrollItem>(((Component) this.mGrid).transform).Init(story059ItemData);
    this.mGrid.repositionNow = true;
    this.mScrollView.ResetPosition();
    yield break;
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void activeScrollMainPanel(bool active) => this.scrollMainPanel.SetActive(active);
}
