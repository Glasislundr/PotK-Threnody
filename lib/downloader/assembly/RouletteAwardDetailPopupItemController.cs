// Decompiled with JetBrains decompiler
// Type: RouletteAwardDetailPopupItemController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class RouletteAwardDetailPopupItemController : MonoBehaviour
{
  [SerializeField]
  private GameObject awardIconContainer;
  [SerializeField]
  private UILabel awardDescription;

  public IEnumerator Init(RouletteR001FreeDeckEntity awardData)
  {
    IEnumerator e = this.awardIconContainer.GetOrAddComponent<CreateIconObject>().CreateThumbnail(awardData.reward_type_id, awardData.reward_id, awardData.reward_quantity ?? 1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.awardDescription.SetTextLocalize(awardData.reward_title);
  }
}
