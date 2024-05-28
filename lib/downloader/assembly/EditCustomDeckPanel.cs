// Decompiled with JetBrains decompiler
// Type: EditCustomDeckPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/DeckPanel")]
public class EditCustomDeckPanel : MonoBehaviour
{
  [SerializeField]
  private EditCustomDeckUnitPanel[] unitPanels_;
  private PlayerCustomDeck deck_;
  private Dictionary<int, PlayerUnit[]> storedLastPlayerUnits_ = new Dictionary<int, PlayerUnit[]>();

  public EditCustomDeckMenu menu { get; private set; }

  public void setLinks(EditCustomDeckMenu m)
  {
    this.menu = m;
    for (int index = 0; index < this.unitPanels_.Length; ++index)
    {
      this.unitPanels_[index].panel = this;
      this.unitPanels_[index].index = index;
    }
  }

  public void setBlanks()
  {
    this.storeLastPlayerUnits();
    this.deck_ = (PlayerCustomDeck) null;
    for (int index = 0; index < this.unitPanels_.Length; ++index)
      this.unitPanels_[index].setBlank();
  }

  public IEnumerator doInitialize(PlayerCustomDeck customDeck)
  {
    this.storeLastPlayerUnits();
    this.deck_ = customDeck;
    this.restoreLastPlayerUnits();
    PlayerCustomDeckUnit_parameter_list[] units = customDeck.unit_parameter_list;
    int n;
    for (n = 0; n < this.unitPanels_.Length && n < units.Length; ++n)
    {
      IEnumerator e = this.unitPanels_[n].doInitialize(units[n]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    for (; n < this.unitPanels_.Length; ++n)
      this.unitPanels_[n].setBlank();
  }

  private void storeLastPlayerUnits()
  {
    if (this.deck_ == null)
      return;
    this.storedLastPlayerUnits_[this.deck_.deck_number] = ((IEnumerable<EditCustomDeckUnitPanel>) this.unitPanels_).Select<EditCustomDeckUnitPanel, PlayerUnit>((Func<EditCustomDeckUnitPanel, PlayerUnit>) (x => x.lastPlayerUnit)).ToArray<PlayerUnit>();
  }

  private void restoreLastPlayerUnits()
  {
    PlayerUnit[] playerUnitArray;
    if (this.deck_ == null || !this.storedLastPlayerUnits_.TryGetValue(this.deck_.deck_number, out playerUnitArray))
      return;
    for (int index = 0; index < playerUnitArray.Length; ++index)
      this.unitPanels_[index].lastPlayerUnit = playerUnitArray[index];
  }

  public GameObject[] objIcons
  {
    get
    {
      if (this.deck_ == null)
        return (GameObject[]) null;
      GameObject[] objIcons = new GameObject[this.unitPanels_.Length];
      for (int index = 0; index < objIcons.Length; ++index)
        objIcons[index] = this.unitPanels_[index].objIcon;
      return objIcons;
    }
  }

  public IEnumerator doUpdatedJob(PlayerCustomDeckUnit_parameter_list param)
  {
    IEnumerator e = this.unitPanels_[param.index].doUpdateJob(param.createPlayerUnit(this.menu.playerUnits, this.menu.playerGears));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void callbackSwapPosition(int aIndex, int bIndex)
  {
    PlayerUnit lastPlayerUnit = this.unitPanels_[aIndex].lastPlayerUnit;
    this.unitPanels_[aIndex].lastPlayerUnit = this.unitPanels_[bIndex].lastPlayerUnit;
    this.unitPanels_[bIndex].lastPlayerUnit = lastPlayerUnit;
  }
}
