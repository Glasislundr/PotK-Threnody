// Decompiled with JetBrains decompiler
// Type: Shop00742CommonPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Shop00742CommonPoint : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtCount;
  [SerializeField]
  protected UILabel TxtName;

  public void Init(MasterDataTable.CommonRewardType type, int count)
  {
    switch (type)
    {
      case MasterDataTable.CommonRewardType.money:
        this.doMoney(count);
        break;
      case MasterDataTable.CommonRewardType.player_exp:
        this.doPlayerExp(count);
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        this.doFriendPoint(count);
        break;
    }
  }

  private void doMoney(int count)
  {
    this.TxtName.text = "ゼニー";
    this.TxtCount.text = "×" + count.ToString();
  }

  private void doPlayerExp(int count)
  {
    this.TxtName.text = "プレイヤー経験値";
    this.TxtCount.text = "×" + count.ToString();
  }

  private void doFriendPoint(int count)
  {
    this.TxtName.text = "マナポイント";
    this.TxtCount.text = "×" + count.ToString();
  }
}
