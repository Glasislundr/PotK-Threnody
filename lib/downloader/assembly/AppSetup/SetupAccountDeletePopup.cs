// Decompiled with JetBrains decompiler
// Type: AppSetup.SetupAccountDeletePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using Gsc.Device;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace AppSetup
{
  public class SetupAccountDeletePopup : MonoBehaviour
  {
    [SerializeField]
    private UIButton cancelButton;
    [SerializeField]
    private UIButton nextButton;

    public void IbtnCancel() => Singleton<PopupManager>.GetInstance().onDismiss();

    public void IbtnAccountDeleteNext()
    {
      string helpContactAddress = Consts.GetInstance().HELP_CONTACT_ADDRESS;
      string str1 = Consts.Format(Consts.GetInstance().ACCOUNT_DELETE_TITLE, (IDictionary) new Dictionary<string, string>()
      {
        {
          "short_id",
          Persist.userInfo.Data.userId
        }
      });
      string str2 = string.Format("{0}/{1}/{2}/{3}/{4}", (object) DeviceInfo.DeviceModel, (object) DeviceInfo.DeviceVendor, (object) DeviceInfo.OperatingSystem, (object) DeviceInfo.ProcessorType, (object) DeviceInfo.SystemMemorySize);
      string str3 = Consts.Format(Consts.GetInstance().ACCOUNT_DELETE_MAIL_BODY, (IDictionary) new Dictionary<string, string>()
      {
        {
          "ver",
          Application.version
        },
        {
          "short_id",
          Persist.userInfo.Data.userId
        },
        {
          "agent",
          str2
        }
      }).Replace("\n", "%0A");
      App.LaunchMailer(helpContactAddress, str1, str3);
    }
  }
}
