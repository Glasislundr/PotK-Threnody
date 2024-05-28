// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollView04
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class DetailMenuScrollView04 : DetailMenuScrollViewBase
{
  [SerializeField]
  protected UILabel txt_Bust;
  [SerializeField]
  protected UILabel txt_Hip;
  [SerializeField]
  protected UILabel txt_Waist;
  [SerializeField]
  protected UILabel txt_Hight;
  [SerializeField]
  protected UILabel txt_Weight;
  [SerializeField]
  protected UILabel txt_Birthday;
  [SerializeField]
  protected UILabel txt_Birthplace;
  [SerializeField]
  protected UILabel txt_Bloodgroup;
  [SerializeField]
  protected UILabel txt_Interest;
  [SerializeField]
  protected UILabel txt_Favorite;
  [SerializeField]
  protected UILabel txt_Constellation;

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    this.Set(playerUnit.unit.character);
    return true;
  }

  public void Set(UnitCharacter character)
  {
    int result;
    string text;
    if (character.birthday.Length == 4 && int.TryParse(character.birthday, out result))
    {
      string unit0042Birthday = Consts.GetInstance().unit004_2_birthday;
      Hashtable args = new Hashtable();
      int num = result / 100;
      args.Add((object) "month", (object) num.ToString());
      num = result % 100;
      args.Add((object) "date", (object) num.ToString());
      text = Consts.Format(unit0042Birthday, (IDictionary) args);
    }
    else
      text = character.birthday;
    this.txt_Bust.SetTextLocalize(character.bust);
    this.txt_Hip.SetTextLocalize(character.hip);
    this.txt_Waist.SetTextLocalize(character.waist);
    this.txt_Hight.SetTextLocalize(character.height);
    this.txt_Weight.SetTextLocalize(character.weight);
    this.txt_Birthday.SetTextLocalize(text);
    this.txt_Birthplace.SetTextLocalize(character.origin);
    this.txt_Bloodgroup.SetTextLocalize(character.blood_type);
    this.txt_Interest.SetTextLocalize(character.hobby);
    this.txt_Favorite.SetTextLocalize(character.favorite);
    this.txt_Constellation.SetTextLocalize(character.zodiac_sign);
  }
}
