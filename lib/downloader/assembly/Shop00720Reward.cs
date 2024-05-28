// Decompiled with JetBrains decompiler
// Type: Shop00720Reward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00720Reward : MonoBehaviour
{
  [SerializeField]
  private GameObject Thumbnail;
  [SerializeField]
  private UILabel Label;

  public IEnumerator Init(MasterDataTable.CommonRewardType rewardType, int rewardID, int quantity, string txt)
  {
    this.Label.SetTextLocalize(txt);
    IEnumerator e = this.Thumbnail.GetOrAddComponent<CreateIconObject>().CreateThumbnail(rewardType, rewardID, quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
