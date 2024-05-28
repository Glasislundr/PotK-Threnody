// Decompiled with JetBrains decompiler
// Type: SafeAreaBandRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SafeAreaBandRoot : MonoBehaviour
{
  private static SafeAreaBandRoot instance;
  [SerializeField]
  private GameObject bandPanel;

  private void Awake()
  {
    if (Object.op_Inequality((Object) SafeAreaBandRoot.instance, (Object) null))
    {
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      SafeAreaBandRoot.instance = this;
      Object.DontDestroyOnLoad((Object) ((Component) this).gameObject);
      int num = ModalWindow.setupRootPanel(((Component) this).GetComponent<UIRoot>());
      ((Component) this).GetComponent<UIRoot>().manualHeight = num;
    }
  }

  public static void ShowSafeAreaBand()
  {
    if (!Object.op_Inequality((Object) SafeAreaBandRoot.instance, (Object) null))
      return;
    SafeAreaBandRoot.instance.bandPanel.SetActive(true);
  }

  public static void HideSafeAreaBand()
  {
    if (!Object.op_Inequality((Object) SafeAreaBandRoot.instance, (Object) null))
      return;
    SafeAreaBandRoot.instance.bandPanel.SetActive(false);
  }

  public static void DestroySafeAreaBand()
  {
    if (!Object.op_Inequality((Object) SafeAreaBandRoot.instance, (Object) null))
      return;
    Object.Destroy((Object) ((Component) SafeAreaBandRoot.instance).gameObject);
    SafeAreaBandRoot.instance = (SafeAreaBandRoot) null;
  }

  private void Start()
  {
  }

  private void Update()
  {
  }
}
