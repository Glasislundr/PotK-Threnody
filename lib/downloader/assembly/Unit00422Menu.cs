// Decompiled with JetBrains decompiler
// Type: Unit00422Menu
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
public class Unit00422Menu : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname01;
  [SerializeField]
  protected UILabel TxtCharaname02;
  [SerializeField]
  protected UILabel TxtLv;
  [SerializeField]
  protected UILabel TxtOwn;
  [SerializeField]
  protected UILabel TxtOwnnumber;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtLvIcon;
  [SerializeField]
  public NGxScroll scroll;
  [SerializeField]
  protected NGxMaskSprite mask_Chara;
  [SerializeField]
  protected UI2DSprite link_Character;
  [SerializeField]
  protected UISprite lpivot_Friendly_Gauge;
  [SerializeField]
  private List<Unit00422Menu.CharacterIntimate> characterIntimates = new List<Unit00422Menu.CharacterIntimate>();
  [SerializeField]
  private List<UnitIcon> allUnitIcon = new List<UnitIcon>();
  private const int CHARACTER_ID_MAX = 7000;

  public virtual void Foreground()
  {
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void VScrollBar()
  {
  }

  public IEnumerator Init(PlayerUnit playerUnit)
  {
    ((UIWidget) this.lpivot_Friendly_Gauge).width = 0;
    IEnumerator e = this.InitPlayer(playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitPlayer(PlayerUnit playerUnit)
  {
    Unit00422Menu unit00422Menu = this;
    unit00422Menu.characterIntimates.Clear();
    IEnumerator e = unit00422Menu.SetMainCharacter(playerUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00422Menu.GenerateCharacterIntimate(playerUnit);
    Future<GameObject> futurePrefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = futurePrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00422Menu.scroll.Clear();
    int characterIntimateLength = unit00422Menu.characterIntimates.Count;
    for (int i = 0; i < characterIntimateLength; ++i)
    {
      Unit00422Menu.CharacterIntimate characterIntimate = unit00422Menu.characterIntimates[i];
      GameObject gameObject = Object.Instantiate<GameObject>(futurePrefab.Result);
      unit00422Menu.scroll.Add(gameObject);
      UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
      e = unitIcon.SetUnit(characterIntimate.unit, characterIntimate.unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      // ISSUE: reference to a compiler-generated method
      unitIcon.onClick = new Action<UnitIconBase>(unit00422Menu.\u003CInitPlayer\u003Eb__20_1);
      unit00422Menu.SetBottomMode(unitIcon);
      unitIcon.setLevelText(characterIntimate.level.ToString().ToConverter());
      unit00422Menu.allUnitIcon.Add(unitIcon);
      characterIntimate = (Unit00422Menu.CharacterIntimate) null;
      unitIcon = (UnitIcon) null;
    }
    unit00422Menu.TxtCharaname02.SetTextLocalize("");
    unit00422Menu.TxtLv.SetText("-");
    unit00422Menu.TxtLvIcon.SetText("-");
    ((UIWidget) unit00422Menu.lpivot_Friendly_Gauge).width = 0;
    unit00422Menu.scroll.ResolvePosition();
    unit00422Menu.SetPosessionText(characterIntimateLength, MasterData.UnitCharacter.Where<KeyValuePair<int, UnitCharacter>>((Func<KeyValuePair<int, UnitCharacter>, bool>) (uc => uc.Value.ID < 7000)).ToList<KeyValuePair<int, UnitCharacter>>().Count - 1);
  }

  protected virtual void SetBottomMode(UnitIcon icon)
  {
    icon.BottomModeValue = UnitIconBase.BottomMode.Friendly;
  }

  protected virtual void SetPosessionText(int value, int max)
  {
    this.TxtOwnnumber.SetTextLocalize(string.Format("{0}/{1}", (object) value, (object) max));
  }

  private void GenerateCharacterIntimate(PlayerUnit playerUnit)
  {
    int characterUnitCharacter = playerUnit.unit.character_UnitCharacter;
    foreach (PlayerCharacterIntimate pci in SMManager.Get<PlayerCharacterIntimate[]>())
    {
      if (characterUnitCharacter == pci._character || characterUnitCharacter == pci._target_character)
        this.characterIntimates.Add(new Unit00422Menu.CharacterIntimate(characterUnitCharacter, pci));
    }
    this.characterIntimates = this.characterIntimates.OrderBy<Unit00422Menu.CharacterIntimate, int>((Func<Unit00422Menu.CharacterIntimate, int>) (x => x.unit.character.ID)).ToList<Unit00422Menu.CharacterIntimate>();
  }

  private IEnumerator SetMainCharacter(PlayerUnit playerUnit)
  {
    this.TxtCharaname01.SetTextLocalize(playerUnit.unit.character.name);
    Future<Sprite> futureSprite = playerUnit.unit.LoadSpriteLarge();
    IEnumerator e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mask_Chara.sprite2D = futureSprite.Result;
    Future<Sprite> futureMask = Res.GUI._004_2_2_sozai.mask_Chara.Load<Sprite>();
    e = futureMask.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.mask_Chara).GetComponent<NGxMaskSpriteWithScale>().maskTexture = futureMask.Result.texture;
  }

  private void UpdateSelectCharacter(UnitIconBase unitIcon)
  {
    this.TxtCharaname02.SetTextLocalize(unitIcon.Unit.character.name);
    this.link_Character.sprite2D = unitIcon.icon.sprite2D;
    Unit00422Menu.CharacterIntimate characterIntimate = this.characterIntimates.Where<Unit00422Menu.CharacterIntimate>((Func<Unit00422Menu.CharacterIntimate, bool>) (x => x.unit.ID == unitIcon.Unit.ID)).First<Unit00422Menu.CharacterIntimate>();
    if (characterIntimate.level == characterIntimate.max_level)
      this.TxtLv.SetTextLocalize(Consts.GetInstance().UNIT_00422_MAX.ToConverter());
    else
      this.TxtLv.SetTextLocalize(characterIntimate.level.ToString().ToConverter());
    this.TxtLvIcon.SetText(characterIntimate.level.ToString().ToConverter());
    ((UIWidget) this.lpivot_Friendly_Gauge).width = characterIntimate.gauge;
    this.allUnitIcon.ForEach((Action<UnitIcon>) (icon => this.SetFriendlyEffect((UnitIconBase) icon, false)));
    this.SetFriendlyEffect(unitIcon, true);
  }

  protected virtual void SetFriendlyEffect(UnitIconBase icon, bool value)
  {
    icon.SetFriendlyEffect(value);
  }

  private class CharacterIntimate
  {
    private const int GAUGE_MAX = 194;
    public UnitUnit unit;
    public int exp;
    public int exp_next;
    public int level;
    public int max_level;
    public int total_exp;

    public float percentageNextLevel
    {
      get
      {
        if (this.level >= this.max_level)
          return 100f;
        if (this.exp < 0)
          return 0.0f;
        return this.exp_next < 0 ? 100f : (float) this.exp * 100f / (float) (this.exp + this.exp_next);
      }
    }

    public int gauge
    {
      get
      {
        if ((double) this.percentageNextLevel >= 100.0)
          return 194;
        return (double) this.percentageNextLevel <= 0.0 ? 0 : (int) (194.0 * (double) this.percentageNextLevel / 100.0);
      }
    }

    public CharacterIntimate(int characterId, PlayerCharacterIntimate pci)
    {
      UnitUnit unitUnit = (UnitUnit) null;
      if (pci.character.ID == characterId)
        unitUnit = pci.target_character.GetDefaultUnitUnit().resource_reference_unit_id;
      else if (pci.target_character.ID == characterId)
        unitUnit = pci.character.GetDefaultUnitUnit().resource_reference_unit_id;
      this.unit = unitUnit;
      this.exp = pci.exp;
      this.exp_next = pci.exp_next;
      this.level = pci.level;
      this.max_level = pci.max_level;
      this.total_exp = pci.total_exp;
    }
  }
}
