// Decompiled with JetBrains decompiler
// Type: Unit004JobAfter2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004JobAfter2 : MonoBehaviour
{
  [SerializeField]
  private UILabel txtBonus1;
  [SerializeField]
  private UILabel txtBonusNum1;
  [SerializeField]
  private UILabel txtBonus2;
  [SerializeField]
  private UILabel txtBonusNum2;
  [SerializeField]
  private UILabel txtBonus3;
  [SerializeField]
  private UILabel txtBonusNum3;

  public IEnumerator Init(PlayerUnitJob_abilities jobAbility)
  {
    JobCharacteristics master = jobAbility.master;
    string levelmaxBonusTypeText1 = jobAbility.levelmaxBonusTypeText1;
    if (levelmaxBonusTypeText1 != null)
    {
      this.txtBonus1.SetTextLocalize(levelmaxBonusTypeText1);
      this.txtBonusNum1.SetTextLocalize("+" + master.levelmax_bonus_value.ToString());
      ((Component) this.txtBonus1).gameObject.SetActive(true);
      ((Component) this.txtBonusNum1).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.txtBonus1).gameObject.SetActive(false);
      ((Component) this.txtBonusNum1).gameObject.SetActive(false);
    }
    string levelmaxBonusTypeText2 = jobAbility.levelmaxBonusTypeText2;
    if (levelmaxBonusTypeText2 != null)
    {
      this.txtBonus2.SetTextLocalize(levelmaxBonusTypeText2);
      this.txtBonusNum2.SetTextLocalize("+" + master.levelmax_bonus_value2.ToString());
      ((Component) this.txtBonus2).gameObject.SetActive(true);
      ((Component) this.txtBonusNum2).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.txtBonus2).gameObject.SetActive(false);
      ((Component) this.txtBonusNum2).gameObject.SetActive(false);
    }
    string levelmaxBonusTypeText3 = jobAbility.levelmaxBonusTypeText3;
    if (levelmaxBonusTypeText3 != null)
    {
      this.txtBonus3.SetTextLocalize(levelmaxBonusTypeText3);
      this.txtBonusNum3.SetTextLocalize("+" + master.levelmax_bonus_value3.ToString());
      ((Component) this.txtBonus3).gameObject.SetActive(true);
      ((Component) this.txtBonusNum3).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.txtBonus3).gameObject.SetActive(false);
      ((Component) this.txtBonusNum3).gameObject.SetActive(false);
    }
    yield return (object) null;
  }
}
