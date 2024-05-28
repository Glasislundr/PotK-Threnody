// Decompiled with JetBrains decompiler
// Type: Startup00017Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using System;
using UnityEngine;

#nullable disable
public class Startup00017Menu : NGMenuBase
{
  [SerializeField]
  private UIInput inputID;
  [SerializeField]
  private UIInput inputPassword;
  [SerializeField]
  private UIButton DecideBtn;
  [SerializeField]
  private GameObject SuccesPopup;
  [SerializeField]
  private GameObject CodeError;
  [SerializeField]
  private GameObject InputBlock;
  [SerializeField]
  private GameObject SameTerminal;
  [SerializeField]
  private GameObject Unknown;
  private const int CODE_LENGTH = 8;

  private void Start()
  {
    this.inputID.caretColor = Color.black;
    ((UIButtonColor) this.DecideBtn).isEnabled = false;
  }

  public void InitDataCode()
  {
    this.inputID.keyboardType = (UIInput.KeyboardType) 1;
    this.inputID.value = "";
  }

  public void OnCahnge()
  {
    if (string.IsNullOrEmpty(this.inputID.value) || !string.IsNullOrEmpty(this.inputID.value) && this.inputID.value.Length < 1 || string.IsNullOrEmpty(this.inputPassword.value) || !string.IsNullOrEmpty(this.inputPassword.value) && this.inputPassword.value.Length < 8)
      ((UIButtonColor) this.DecideBtn).isEnabled = false;
    else
      ((UIButtonColor) this.DecideBtn).isEnabled = true;
  }

  public void MigrateAPI()
  {
    if (!string.IsNullOrEmpty(this.inputID.value) && !string.IsNullOrEmpty(this.inputPassword.value))
    {
      WebAPI.AuthAddDevice(this.inputID.value, this.inputPassword.value, (Action) (() =>
      {
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
        this.SuccesPopup.SetActive(true);
        this.ClearGuildData();
        this.DeleteUserInfoData();
        this.DeleteDeckInfoData();
        this.DeleteEarthData();
        this.EndTutorial();
      }), (Action<AddDeviceWithEmailAddressAndPasswordResult>) (error => this.ErrorPopup(error)));
    }
    else
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
      this.CodeError.SetActive(true);
    }
  }

  private void ErrorPopup(AddDeviceWithEmailAddressAndPasswordResult error)
  {
    GameObject gameObject;
    switch (error.ResultCode)
    {
      case AddDeviceWithEmailAddressAndPasswordResultCode.MissingDeviceId:
        gameObject = this.SameTerminal;
        break;
      case AddDeviceWithEmailAddressAndPasswordResultCode.MissingEmailOrPassword:
        gameObject = this.CodeError;
        break;
      case AddDeviceWithEmailAddressAndPasswordResultCode.Locked:
        gameObject = this.InputBlock;
        DateTime blockTime = DateTime.Now.AddSeconds((double) error.LockedExpiresIn);
        gameObject.GetComponent<Transfer012811Menu>().ChangeDescription(blockTime, false);
        break;
      default:
        gameObject = this.Unknown;
        break;
    }
    gameObject.SetActive(true);
  }

  private void ClearGuildData()
  {
    if (!Persist.guildSetting.Exists)
      return;
    Persist.guildSetting.Data.reset();
    Persist.guildSetting.Flush();
  }

  private void DeleteUserInfoData()
  {
    if (!Persist.userInfo.Exists)
      return;
    Persist.userInfo.Delete();
  }

  private void DeleteDeckInfoData()
  {
    Persist.customDeckTutorial.DeleteAndClear();
    Persist.deckOrganized.DeleteAndClear();
    Persist.seaDeckOrganized.DeleteAndClear();
    Persist.colosseumDeckOrganized.DeleteAndClear();
    Persist.versusDeckOrganized.DeleteAndClear();
    Persist.guildRaidLastSortie.DeleteAndClear();
  }

  private void DeleteEarthData()
  {
    if (Persist.earthData.Exists)
      Persist.earthData.Delete();
    if (!Persist.earthBattleEnvironment.Exists)
      return;
    Persist.earthBattleEnvironment.Delete();
  }

  private void EndTutorial() => Persist.EndTutorial();
}
