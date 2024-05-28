// Decompiled with JetBrains decompiler
// Type: PopupRecommendPartCallSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Recommend/CallSkill")]
public class PopupRecommendPartCallSkill : PopupRecommendPart
{
  [SerializeField]
  private UILabel skillNameLabel;
  [SerializeField]
  private UILabel skillDescriptionLabel;
  [SerializeField]
  private Transform genre1;
  [SerializeField]
  private Transform genre2;
  [SerializeField]
  private int genreDepth;
  private GameObject genrePrefab;
  private GameObject popupCallSkillDetailsPrefab;
  private UnitUnit unit;
  private BattleskillSkill battleSkill;

  public override IEnumerator doInitialize(PlayerUnit playerUnit, UnitUnit target)
  {
    this.unit = target;
    CallCharacter[] callCharacterList = MasterData.CallCharacterList;
    CallCharacter callChara = (CallCharacter) null;
    foreach (CallCharacter callCharacter in callCharacterList)
    {
      if (callCharacter.same_character_id == playerUnit.unit.same_character_id)
      {
        callChara = callCharacter;
        break;
      }
    }
    if (callChara != null)
    {
      Future<GameObject> prefabF;
      if (Object.op_Equality((Object) this.popupCallSkillDetailsPrefab, (Object) null))
      {
        prefabF = new ResourceObject("Prefabs/UnitGUIs/Popup_CallSkillDetails").Load<GameObject>();
        IEnumerator e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.popupCallSkillDetailsPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      this.battleSkill = MasterData.BattleskillSkill[callChara.call_skill_id];
      this.skillNameLabel.SetText(this.battleSkill.name);
      this.skillDescriptionLabel.SetText(this.battleSkill.description);
      if (Object.op_Equality((Object) this.genrePrefab, (Object) null))
      {
        prefabF = Res.Icons.SkillGenreIcon.Load<GameObject>();
        yield return (object) prefabF.Wait();
        this.genrePrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      BattleskillGenre? genre1 = this.battleSkill.genre1;
      BattleskillGenre? genre2 = this.battleSkill.genre2;
      ((Component) this.genre1).gameObject.SetActive(genre1.HasValue);
      if (genre1.HasValue)
      {
        SkillGenreIcon component = this.genrePrefab.Clone(this.genre1).GetComponent<SkillGenreIcon>();
        component.Init(genre1);
        ((UIWidget) component.iconSprite).depth = this.genreDepth;
      }
      ((Component) this.genre2).gameObject.SetActive(genre2.HasValue);
      if (genre2.HasValue)
      {
        SkillGenreIcon component = this.genrePrefab.Clone(this.genre2).GetComponent<SkillGenreIcon>();
        component.Init(genre2);
        ((UIWidget) component.iconSprite).depth = this.genreDepth;
      }
    }
  }

  public void onClickedSkill() => this.StartCoroutine(this.PopupCallSkillView());

  private IEnumerator PopupCallSkillView()
  {
    yield return (object) Singleton<PopupManager>.GetInstance().open(this.popupCallSkillDetailsPrefab).GetComponent<PopupCallSkill>().initialize(this.unit, this.battleSkill, false);
  }
}
