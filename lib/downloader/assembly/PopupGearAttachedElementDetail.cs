// Decompiled with JetBrains decompiler
// Type: PopupGearAttachedElementDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupGearAttachedElementDetail : PopupAutoCloseOnAnyTap
{
  [SerializeField]
  [Tooltip("属性アイコン")]
  private UI2DSprite iconElement_;
  [SerializeField]
  [Tooltip("属性名")]
  private UILabel txtName_;
  [SerializeField]
  [Tooltip("説明文")]
  private UILabel txtDescription_;
  private BattleskillSkill skill_;

  public static Future<GameObject> createPrefabLoader()
  {
    return new ResourceObject("Prefabs/unit004_4_3/popup_ElementClass_detail").Load<GameObject>();
  }

  public static IEnumerator show(GameObject prefab, GearGear gear)
  {
    if (gear != null)
    {
      Singleton<PopupManager>.GetInstance().open(prefab, isViewBack: false).GetComponent<PopupGearAttachedElementDetail>().initialize(gear);
      while (Singleton<PopupManager>.GetInstance().isOpen)
        yield return (object) null;
    }
  }

  private void initialize(GearGear gear)
  {
    this.setEventOnAnyTap();
    this.skill_ = gear.attachedElementSkill;
    this.txtName_.SetTextLocalize(this.skill_.name);
    this.txtDescription_.SetTextLocalize(this.skill_.description);
  }

  private IEnumerator Start()
  {
    Future<GameObject> loader = Res.Icons.CommonElementIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    GameObject result = loader.Result;
    loader = (Future<GameObject>) null;
    this.iconElement_.sprite2D = result.GetComponent<CommonElementIcon>().getIcon(this.skill_.element);
  }
}
