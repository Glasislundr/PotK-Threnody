// Decompiled with JetBrains decompiler
// Type: Unit0042FloatingTerraiAbilityDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042FloatingTerraiAbilityDialog")]
public class Unit0042FloatingTerraiAbilityDialog : Unit0042FloatingDialogBase
{
  [Header("TerraiAbility")]
  [SerializeField]
  private UILabel txt_AbilityName;
  [SerializeField]
  private UILabel txt_Description;
  [SerializeField]
  private GameObject[] dynSkillGenreIcons;
  private BattleskillSkill skill;

  public void SetData(BattleskillSkill skill)
  {
    this.skill = skill;
    this.txt_AbilityName.text = skill.name;
    this.txt_Description.text = skill.description;
  }

  public new void Show()
  {
    base.Show();
    this.StartCoroutine(this.CreateSkillGenereIcons());
  }

  private IEnumerator CreateSkillGenereIcons()
  {
    Future<GameObject> prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    result.Clone(this.dynSkillGenreIcons[0].transform).GetComponent<SkillGenreIcon>().Init(this.skill.genre1);
    result.Clone(this.dynSkillGenreIcons[1].transform).GetComponent<SkillGenreIcon>().Init(this.skill.genre2);
  }
}
