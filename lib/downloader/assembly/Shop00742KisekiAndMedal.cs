// Decompiled with JetBrains decompiler
// Type: Shop00742KisekiAndMedal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00742KisekiAndMedal : MonoBehaviour
{
  [SerializeField]
  protected UILabel TxtFlavor;
  [SerializeField]
  protected UILabel TxtName;
  [SerializeField]
  protected UI2DSprite SlcTarget;

  public IEnumerator Init(MasterDataTable.CommonRewardType type)
  {
    switch (type)
    {
      case MasterDataTable.CommonRewardType.coin:
        yield return (object) this.doCoin();
        break;
      case MasterDataTable.CommonRewardType.medal:
        yield return (object) this.doMedal();
        break;
      case MasterDataTable.CommonRewardType.battle_medal:
        yield return (object) this.doBattleMedal();
        break;
    }
  }

  private IEnumerator doCoin()
  {
    this.TxtFlavor.SetTextLocalize("マナが結晶化した特殊な鉱石。マスターの中のバイブスに反応して、キラーズを持つキル姫を呼び寄せることができるという伝承がある。非常に貴重な物で一般には流通しない。願いに応じてその手の中に現れ、その力を発揮すると言われている。");
    this.TxtName.SetTextLocalize("姫石");
    Future<Sprite> r = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/Detail/kiseki_basic");
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }

  private IEnumerator doMedal()
  {
    this.TxtFlavor.SetTextLocalize("レアメダルショップでアイテムを購入出来る。またレアメダルスロットを回すことが出来る。");
    this.TxtName.SetTextLocalize("レアメダル");
    Future<Sprite> r = Singleton<ResourceManager>.GetInstance().Load<Sprite>("AssetBundle/Resources/ItemDetails/ItemDetails_medal");
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }

  private IEnumerator doBattleMedal()
  {
    this.TxtFlavor.SetTextLocalize("有効期限ありと有効期限なしの二種類があり、ファンキルメダルショップでアイテムを購入出来る。");
    this.TxtName.SetTextLocalize("ファンキルメダル");
    Future<Sprite> r = Singleton<ResourceManager>.GetInstance().Load<Sprite>("AssetBundle/Resources/ItemDetails/ItemDetails_battle_medal");
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SlcTarget.sprite2D = r.Result;
  }
}
