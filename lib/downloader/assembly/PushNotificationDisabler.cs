// Decompiled with JetBrains decompiler
// Type: PushNotificationDisabler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PushNotificationDisabler : MonoBehaviour
{
  [SerializeField]
  private GameObject m_pushNotificationConfig;
  [SerializeField]
  private GameObject m_masterNameChange;
  [SerializeField]
  private GameObject m_userCommentEdit;

  private void Start()
  {
    if (!Application.platform.Equals((object) (RuntimePlatform) 2) && !Application.platform.Equals((object) (RuntimePlatform) 1))
      return;
    this.SortObjects();
    this.DisablePushNotification();
  }

  private void SortObjects()
  {
    this.m_masterNameChange.transform.position = this.m_pushNotificationConfig.transform.position;
    this.m_userCommentEdit.transform.position = this.m_masterNameChange.transform.position;
  }

  private void DisablePushNotification() => this.m_pushNotificationConfig.SetActive(false);
}
