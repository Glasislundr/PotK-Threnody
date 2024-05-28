// Decompiled with JetBrains decompiler
// Type: Story00913Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Story00913Scene : NGSceneBase
{
  public Story00913Menu menu;
  public UILabel TxtTitle;
  [SerializeField]
  private NGxScroll ScrollContainer;

  public IEnumerator onStartSceneAsync()
  {
    PlayerSeaQuestS[] PlayerSeaQuests = SMManager.Get<PlayerSeaQuestS[]>();
    IEnumerator e1;
    if (PlayerSeaQuests == null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.QuestProgressSea> ft = WebAPI.QuestProgressSea((Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (ft.Result == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        yield break;
      }
      else
      {
        PlayerSeaQuests = SMManager.Get<PlayerSeaQuestS[]>();
        ft = (Future<WebAPI.Response.QuestProgressSea>) null;
      }
    }
    this.ScrollContainer.Clear();
    if (PlayerSeaQuests == null || PlayerSeaQuests.Length == 0)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
    else
    {
      e1 = this.menu.InitChapterButton(((IEnumerable<QuestSeaXL>) MasterData.QuestSeaXLList).Select<QuestSeaXL, int>((Func<QuestSeaXL, int>) (x => x.ID)).OrderBy<int, int>((Func<int, int>) (i => i)).SelectMany<int, PlayerSeaQuestS>((Func<int, IEnumerable<PlayerSeaQuestS>>) (i => (IEnumerable<PlayerSeaQuestS>) PlayerSeaQuests.DisplayScrollM(i))).ToArray<PlayerSeaQuestS>());
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.ScrollContainer.ResolvePosition();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
  }
}
