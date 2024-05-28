// Decompiled with JetBrains decompiler
// Type: SwitcherSceneChanging
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SwitcherSceneChanging : MonoBehaviour
{
  private bool reset_;
  private bool lastChanging_;
  [SerializeField]
  private bool passFirstReset_;
  [SerializeField]
  private bool offChanging_ = true;
  [SerializeField]
  private GameObject[] objs_;

  private void Awake()
  {
    this.reset_ = !this.passFirstReset_;
    this.lastChanging_ = false;
  }

  private void Update()
  {
    bool flag1 = !Singleton<NGSceneManager>.GetInstance().isSceneInitialized | Singleton<PopupManager>.GetInstance().isOpen | Singleton<CommonRoot>.GetInstance().isLoading | Singleton<CommonRoot>.GetInstance().isActiveHomeMenu;
    if (!this.reset_ && this.lastChanging_ == flag1)
      return;
    this.reset_ = false;
    this.lastChanging_ = flag1;
    bool flag2 = this.offChanging_ ? !flag1 : flag1;
    foreach (GameObject gameObject in this.objs_)
      gameObject.SetActive(flag2);
  }
}
