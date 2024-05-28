// Decompiled with JetBrains decompiler
// Type: GuildChatBBSViewerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildChatBBSViewerController : MonoBehaviour
{
  [SerializeField]
  private UILabel textMessage;
  [SerializeField]
  private UIButton OKButton;

  public void InitializeBBSViewerDialog()
  {
    this.textMessage.SetTextLocalize(PlayerAffiliation.Current.guild.private_message);
  }

  public void OnBBSViewerOKButtonClicked() => Singleton<PopupManager>.GetInstance().dismiss();

  public void OnBBSViewerEditButtonClicked()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartCoroutine(this.OpenBBSEditorDialog());
  }

  private IEnumerator OpenBBSEditorDialog()
  {
    GuildChatDetailedListController detailedListController = Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController;
    while (detailedListController.bbsEditorDialogPrefab == null)
      yield return (object) null;
    GameObject prefab = Singleton<CommonRoot>.GetInstance().guildChatManager.detailedListController.bbsEditorDialogPrefab.Result.Clone();
    prefab.GetComponent<GuildChatBBSEditorController>().InitializeBBSEditorDialog();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }
}
