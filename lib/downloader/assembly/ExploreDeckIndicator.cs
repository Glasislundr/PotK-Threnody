// Decompiled with JetBrains decompiler
// Type: ExploreDeckIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ExploreDeckIndicator : MonoBehaviour
{
  [SerializeField]
  private Transform[] mLinkCharacters;
  [SerializeField]
  private UILabel mTotalCombat;
  [SerializeField]
  private UILabel mWinRate;
  [SerializeField]
  private NGxMaskSprite mMaskLeaderChara;
  [SerializeField]
  private UI2DSprite mSprLeaderGearKind;
  [SerializeField]
  private UILabel mTxtLeaderUnitLv;
  private GameObject unitIconPrefab;

  public IEnumerator InitializeAsync(PlayerDeck deck, int winRate = 999)
  {
    IEnumerator e = this.InitializeDeckInfo(deck.player_units);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mTotalCombat.SetTextLocalize(deck.total_combat);
    this.mWinRate.SetTextLocalize(winRate);
  }

  public IEnumerator InitializeDeckInfo(PlayerUnit[] deckUnits)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    for (int i = 1; i < deckUnits.Length; ++i)
    {
      e = this.createUnitIcon(deckUnits[i], i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    PlayerUnit leaderUnit = deckUnits[0];
    Future<Sprite> futureSprite = leaderUnit.unit.LoadSpriteLarge(leaderUnit.job_id, 1f);
    e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mMaskLeaderChara.sprite2D = futureSprite.Result;
    Future<Sprite> futureMask = new ResourceObject("GUI/explore_other/slc_unit_mask_Edit").Load<Sprite>();
    e = futureMask.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.mMaskLeaderChara).GetComponent<NGxMaskSpriteWithScale>().maskTexture = futureMask.Result.texture;
    this.mSprLeaderGearKind.sprite2D = ExploreDeckIndicator.LoadGearKindSprite((GearKindEnum) leaderUnit.unit.kind_GearKind, leaderUnit.GetElement());
    this.mTxtLeaderUnitLv.SetTextLocalize(leaderUnit.total_level);
  }

  private IEnumerator createUnitIcon(PlayerUnit unit, int index)
  {
    UnitIcon iconScript = this.unitIconPrefab.CloneAndGetComponent<UnitIcon>(this.mLinkCharacters[index]);
    IEnumerator e = iconScript.setSimpleUnit(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    iconScript.setLevelText(unit);
    iconScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    if (unit != (PlayerUnit) null)
    {
      iconScript.onLongPress = (Action<UnitIconBase>) null;
      iconScript.BreakWeapon = unit.IsBrokenEquippedGear;
      iconScript.SpecialIcon = false;
    }
    else
      iconScript.SetEmpty();
    iconScript.onClick = (Action<UnitIconBase>) null;
    ((UIButtonColor) iconScript.Button).isEnabled = false;
    iconScript.Favorite = false;
    iconScript.SetupDeckStatusBlink();
  }

  public static Sprite LoadGearKindSprite(GearKindEnum kind, CommonElement element)
  {
    string empty = string.Empty;
    return Resources.Load<Sprite>(!Singleton<NGGameDataManager>.GetInstance().IsSea ? string.Format("Icons/Materials/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()) : string.Format("Icons/Materials/Sea/GearKind_Element_Icon/slc_{0}_{1}_34_30", (object) kind.ToString(), (object) element.ToString()));
  }
}
