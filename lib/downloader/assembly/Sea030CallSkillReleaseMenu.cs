// Decompiled with JetBrains decompiler
// Type: Sea030CallSkillReleaseMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030CallSkillReleaseMenu : BackButtonMenuBase
{
  [SerializeField]
  private Transform popupPosition;
  private GameObject popupObj;
  private GameObject popupPrefab;

  public IEnumerator LoadPrefab()
  {
    Future<GameObject> prefab = new ResourceObject("Animations/Call_Skill_Release/CallSkill_Effect").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupPrefab = prefab.Result;
  }

  public void CreatePopup(CallCharacter master)
  {
    this.popupObj = this.popupPrefab.Clone(this.popupPosition);
    this.popupObj.GetComponent<Sea030CallSkillEffectPopup>().SetData(master);
  }

  public override void onBackButton()
  {
  }
}
