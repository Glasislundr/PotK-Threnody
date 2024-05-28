// Decompiled with JetBrains decompiler
// Type: Shop007231Description
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
public class Shop007231Description : BackButtonMenuBase
{
  [SerializeField]
  private UILabel title;
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  private UILabel txtDescription_;
  [SerializeField]
  private UILabel txtNames_;
  private SM.SelectTicket ticket_;
  private Dictionary<int, SelectTicketSelectSample> sampleDic;

  public void initialize(SM.SelectTicket ticket) => this.ticket_ = ticket;

  private IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    Shop007231Description shop007231Description = this;
    if (num1 != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    UIPanel component = ((Component) shop007231Description.scroll_).GetComponent<UIPanel>();
    ((UIRect) component).ResetAnchors();
    ((UIRect) component).Update();
    List<UnitUnit> unitUnitList = (List<UnitUnit>) null;
    if (shop007231Description.ticket_.category_id == 1)
      unitUnitList = ((IEnumerable<SelectTicketChoices>) shop007231Description.ticket_.choices).Where<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (uc => MasterData.UnitUnit.ContainsKey(uc.reward_id))).Select<SelectTicketChoices, UnitUnit>((Func<SelectTicketChoices, UnitUnit>) (uc => MasterData.UnitUnit[uc.reward_id])).ToList<UnitUnit>();
    ((UIWidget) shop007231Description.txtDescription_).width = (int) component.width;
    if (!string.IsNullOrEmpty(shop007231Description.ticket_.detail))
    {
      shop007231Description.txtDescription_.maxLineCount = shop007231Description.ticket_.detail.Where<char>((Func<char, bool>) (c => c.Equals('\n'))).Count<char>() + 1;
      shop007231Description.txtDescription_.SetTextLocalize(shop007231Description.ticket_.detail);
    }
    else
      shop007231Description.txtDescription_.SetTextLocalize("");
    ((Component) shop007231Description.txtNames_).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) shop007231Description.txtNames_).transform.localPosition.x, (float) -((UIWidget) shop007231Description.txtDescription_).height));
    ((UIWidget) shop007231Description.txtNames_).width = (int) component.width;
    Consts instance = Consts.GetInstance();
    string text = "";
    if (shop007231Description.ticket_.category_id == 1)
    {
      shop007231Description.title.SetTextLocalize(instance.SHOP_007231_TITLE01);
      text = string.Format(instance.SHOP_007231_DESCRIPTION_INSERT1, shop007231Description.ticket_.unit_type_selectable ? (object) instance.SHOP_007231_DESCRIPTION_UNITTYPE_SELECT : (object) instance.SHOP_007231_DESCRIPTION_UNITTYPE_NOTSELECT);
      if (shop007231Description.ticket_.exchange_limit)
      {
        string descriptionFormatUnitCount = instance.SHOP_007231_DESCRIPTION_FORMAT_UNIT_COUNT;
        foreach (UnitUnit unitUnit in unitUnitList)
        {
          UnitUnit u = unitUnit;
          SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) shop007231Description.ticket_.choices).First<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (x => x.reward_id == u.ID));
          text += string.Format(descriptionFormatUnitCount, (object) u.name, (object) (u.rarity.index + 1), (object) selectTicketChoices.exchangeable_count);
        }
      }
      else
      {
        string descriptionFormatUnit = instance.SHOP_007231_DESCRIPTION_FORMAT_UNIT;
        foreach (UnitUnit unitUnit in unitUnitList)
          text += string.Format(descriptionFormatUnit, (object) unitUnit.name, (object) (unitUnit.rarity.index + 1));
      }
    }
    else if (shop007231Description.ticket_.category_id == 2)
    {
      shop007231Description.title.SetTextLocalize(instance.SHOP_007231_TITLE02);
      text = instance.SHOP_007231_DESCRIPTION_INSERT2;
      foreach (SelectTicketChoices choice in shop007231Description.ticket_.choices)
      {
        if (!MasterData.SelectTicketSelectSample[choice.id].deckID.HasValue)
          text = text + "\n " + MasterData.SelectTicketSelectSample[choice.id].reward_title + " × " + (object) MasterData.SelectTicketSelectSample[choice.id].reward_value;
      }
    }
    shop007231Description.txtNames_.SetTextLocalize(text);
    int num2 = shop007231Description.CountOf(shop007231Description.txtNames_.text, '\n'.ToString()) + 1;
    shop007231Description.txtNames_.maxLineCount = num2;
    shop007231Description.scroll_.ResetPosition();
    return false;
  }

  private int CountOf(string target, params string[] strArray)
  {
    int num = 0;
    foreach (string str in strArray)
    {
      for (int index = target.IndexOf(str, 0); index != -1; index = target.IndexOf(str, index + str.Length))
        ++num;
    }
    return num;
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnBack() => Singleton<PopupManager>.GetInstance().onDismiss();
}
