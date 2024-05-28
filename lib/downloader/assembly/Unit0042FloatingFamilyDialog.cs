// Decompiled with JetBrains decompiler
// Type: Unit0042FloatingFamilyDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042FloatingFamilyDialog")]
public class Unit0042FloatingFamilyDialog : Unit0042FloatingDialogBase
{
  [SerializeField]
  protected GameObject dyn_FamilyIcon;
  [SerializeField]
  protected UILabel txt_Name;
  [SerializeField]
  protected UILabel txt_Description;
  private GameObject familyIconPrefab;
  private UnitFamilyValue familyValue;

  public new void Show()
  {
    if (this.DialogConteiner.activeInHierarchy && this.isShow)
      return;
    base.Show();
    this.dir_SpecialPoint.SetActive(false);
    this.dir_FamilyType.SetActive(true);
    this.StopCoroutine(this.setIcon());
    this.StartCoroutine(this.setIcon());
  }

  public void setData(GameObject familyIconPrefab, UnitFamilyValue familyValue)
  {
    this.familyIconPrefab = familyIconPrefab;
    this.familyValue = familyValue;
    this.txt_Name.SetText(familyValue.name);
    this.txt_Description.SetText(familyValue.flavor);
  }

  private IEnumerator setIcon()
  {
    GameObject gameObject = this.familyIconPrefab.Clone();
    gameObject.gameObject.SetParent(this.dyn_FamilyIcon);
    gameObject.GetComponentInChildren<SkillfullnessIcon>().InitKindId((UnitFamily) this.familyValue.ID);
    yield break;
  }
}
