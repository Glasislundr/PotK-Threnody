// Decompiled with JetBrains decompiler
// Type: BattleUI05AlbumCompleteSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUI05AlbumCompleteSetting : MonoBehaviour
{
  [SerializeField]
  private UILabel txtReward;
  [SerializeField]
  private UILabel txtAlbumName;
  [SerializeField]
  private CreateIconObject rewardIcon;
  [SerializeField]
  private UI2DSprite Illustration;
  [SerializeField]
  private UI2DSprite IllustrationEff;

  public IEnumerator Init(MasterDataTable.SeaAlbum seaAlbum, List<SeaAlbumRewardGroup> rewardList)
  {
    IEnumerator e = this.CreateRewardIcon(seaAlbum, rewardList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadIllustration(seaAlbum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtAlbumName.SetTextLocalize(seaAlbum.name);
    foreach (SeaAlbumRewardGroup reward in rewardList)
    {
      if (reward.reward_type_id == MasterDataTable.CommonRewardType.emblem)
        this.txtReward.SetTextLocalize(reward.reward_title);
    }
  }

  private IEnumerator CreateRewardIcon(MasterDataTable.SeaAlbum seaAlbum, List<SeaAlbumRewardGroup> rewardList)
  {
    IEnumerator e;
    foreach (SeaAlbumRewardGroup reward in rewardList)
    {
      if (reward.reward_type_id == MasterDataTable.CommonRewardType.emblem)
      {
        e = this.rewardIcon.CreateThumbnail(reward.reward_type_id, reward.reward_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    e = this.LoadIllustration(seaAlbum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
  }

  private IEnumerator LoadIllustration(MasterDataTable.SeaAlbum seaAlbum)
  {
    Future<Sprite> imageF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("Album/{0}/slc_album_s", (object) seaAlbum.ID));
    IEnumerator e = imageF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = imageF.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      this.Illustration.sprite2D = result;
      this.IllustrationEff.sprite2D = result;
    }
  }
}
