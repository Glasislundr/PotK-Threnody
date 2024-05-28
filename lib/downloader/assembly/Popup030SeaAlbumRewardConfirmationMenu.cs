// Decompiled with JetBrains decompiler
// Type: Popup030SeaAlbumRewardConfirmationMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup030SeaAlbumRewardConfirmationMenu : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite rewardEmblem;
  private Action callback;

  public IEnumerator Init(int reward_group_id, Action callback)
  {
    List<SeaAlbumRewardGroup> list = ((IEnumerable<SeaAlbumRewardGroup>) MasterData.SeaAlbumRewardGroupList).Where<SeaAlbumRewardGroup>((Func<SeaAlbumRewardGroup, bool>) (x => x.reward_group_id == reward_group_id)).ToList<SeaAlbumRewardGroup>();
    this.callback = callback;
    foreach (SeaAlbumRewardGroup albumRewardGroup in list)
    {
      if (albumRewardGroup.reward_type_id == MasterDataTable.CommonRewardType.emblem)
      {
        IEnumerator e = this.CreateEmblem(albumRewardGroup.reward_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator CreateEmblem(int reward_id)
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(reward_id);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.rewardEmblem.sprite2D = sprF.Result;
  }

  public void IbtnPopupOk()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.callback();
  }

  public override void onBackButton() => this.IbtnPopupOk();
}
