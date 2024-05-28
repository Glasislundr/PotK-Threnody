// Decompiled with JetBrains decompiler
// Type: Gacha99951aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Gacha99951aMenu : Quest99951Menu
{
  [SerializeField]
  protected UILabel TxtPopupdescripton04;
  [SerializeField]
  protected UILabel TxtPopupPossessionAmount;
  [SerializeField]
  protected UILabel TxtPopupStorageAmount;

  public override void SetText(int haveUnit, int max_haveUnit, Player player_data)
  {
    this.player_data_ = player_data;
    this.TxtPopupdescripton01.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION01));
    this.TxtPopupdescripton02.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION02));
    this.TxtPopupdescripton03.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION05));
    this.TxtPopupPossessionAmount.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION07, (IDictionary) new Hashtable()
    {
      {
        (object) "haveunit",
        (object) haveUnit.ToString().ToConverter()
      },
      {
        (object) "maxhaveunit",
        (object) max_haveUnit.ToString().ToConverter()
      }
    }));
    ((Component) this.TxtPopupdescripton04).gameObject.SetActive(false);
    ((Component) this.TxtPopupStorageAmount).gameObject.SetActive(false);
    this.TxtTitle.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION04));
  }

  public void SetText(
    int haveUnit,
    int max_haveUnit,
    int haveUnitReserves,
    int max_haveUnitReserves,
    Player player_data)
  {
    this.player_data_ = player_data;
    this.TxtPopupdescripton01.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION01));
    this.TxtPopupdescripton02.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION02));
    this.TxtPopupdescripton03.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION05));
    this.TxtPopupPossessionAmount.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION07, (IDictionary) new Hashtable()
    {
      {
        (object) "haveunit",
        (object) haveUnit.ToString().ToConverter()
      },
      {
        (object) "maxhaveunit",
        (object) max_haveUnit.ToString().ToConverter()
      }
    }));
    this.TxtPopupdescripton04.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION06));
    this.TxtPopupStorageAmount.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION07, (IDictionary) new Hashtable()
    {
      {
        (object) "haveunit",
        (object) haveUnitReserves.ToString().ToConverter()
      },
      {
        (object) "maxhaveunit",
        (object) max_haveUnitReserves.ToString().ToConverter()
      }
    }));
    this.TxtTitle.SetText(Consts.Format(Consts.GetInstance().GACHA_99951MENU_DESCRIPTION04));
    if (!Object.op_Inequality((Object) this.btnCom, (Object) null))
      return;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (!Object.op_Inequality((Object) instance, (Object) null) || !(instance.sceneName == "unit004_training"))
      return;
    ((UIButtonColor) this.btnCom).isEnabled = false;
  }

  public override void onBackButton() => this.IbtnNo();
}
