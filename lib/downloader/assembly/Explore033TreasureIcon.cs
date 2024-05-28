// Decompiled with JetBrains decompiler
// Type: Explore033TreasureIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033TreasureIcon : MonoBehaviour
{
  public Camera mTargetCamera;
  private MeshRenderer mMeshRenderer;

  public IEnumerator InitAsync(GameCore.Reward reward)
  {
    switch (reward.Type)
    {
      case MasterDataTable.CommonRewardType.supply:
        SupplySupply supply = (SupplySupply) null;
        if (!MasterData.SupplySupply.TryGetValue(reward.Id, out supply))
          break;
        yield return (object) this.initAync(supply);
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        GearGear gear = (GearGear) null;
        if (!MasterData.GearGear.TryGetValue(reward.Id, out gear))
          break;
        yield return (object) this.initAync(gear);
        break;
      case MasterDataTable.CommonRewardType.material_unit:
        UnitUnit unit = (UnitUnit) null;
        if (!MasterData.UnitUnit.TryGetValue(reward.Id, out unit))
          break;
        yield return (object) this.initAync(unit);
        break;
    }
  }

  private IEnumerator initAync(GearGear gear)
  {
    Explore033TreasureIcon explore033TreasureIcon = this;
    explore033TreasureIcon.mMeshRenderer = ((Component) explore033TreasureIcon).GetComponentInChildren<MeshRenderer>();
    Future<Sprite> loader = gear.LoadSpriteThumbnail();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Renderer) explore033TreasureIcon.mMeshRenderer).material.mainTexture = (Texture) loader.Result.texture;
  }

  private IEnumerator initAync(SupplySupply supply)
  {
    Explore033TreasureIcon explore033TreasureIcon = this;
    explore033TreasureIcon.mMeshRenderer = ((Component) explore033TreasureIcon).GetComponentInChildren<MeshRenderer>();
    Future<Sprite> loader = supply.LoadSpriteThumbnail();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Renderer) explore033TreasureIcon.mMeshRenderer).material.mainTexture = (Texture) loader.Result.texture;
  }

  private IEnumerator initAync(UnitUnit unit)
  {
    Explore033TreasureIcon explore033TreasureIcon = this;
    explore033TreasureIcon.mMeshRenderer = ((Component) explore033TreasureIcon).GetComponentInChildren<MeshRenderer>();
    if (!unit.IsMaterialUnit)
    {
      Debug.LogError((object) "ExploreTreasureIcon Support Only MaterialUnit");
    }
    else
    {
      Future<Sprite> loader = unit.LoadSpriteMedium();
      IEnumerator e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Renderer) explore033TreasureIcon.mMeshRenderer).material.mainTexture = (Texture) loader.Result.texture;
    }
  }

  private void Update()
  {
    Vector3 position = ((Component) this.mTargetCamera).transform.position;
    position.y = ((Component) this).transform.position.y;
    ((Component) this).transform.LookAt(position);
  }
}
