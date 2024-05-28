// Decompiled with JetBrains decompiler
// Type: SimpleEffectJobChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/SimpleEffectJobChange")]
public class SimpleEffectJobChange : MonoBehaviour
{
  [SerializeField]
  private Animator animator_;
  [SerializeField]
  private UILabel txtJob_;
  [SerializeField]
  private UILabel txtJobCenter_;
  [SerializeField]
  private UI2DSprite iconRarity_;
  [SerializeField]
  private GameObject objAwakening_;
  [SerializeField]
  private EffectSE effectSE_;
  [SerializeField]
  private AnchorCustomAdjustment anchorSetter_;

  public static Future<GameObject> createLoader()
  {
    return new ResourceObject("Prefabs/unit004_2/dir_CrassChange").Load<GameObject>();
  }

  private void Awake() => ((Component) this).GetComponent<UIRect>().alpha = 0.0f;

  public IEnumerator doInitialize(Transform[] anchorParens)
  {
    yield return (object) null;
    this.anchorSetter_.resetAnchors(anchorParens);
  }

  public void play(PlayerUnit playerUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    MasterDataTable.UnitJob jobData = playerUnit.getJobData();
    this.txtJob_.SetTextLocalize(jobData.name);
    this.txtJobCenter_.SetTextLocalize(jobData.name);
    this.objAwakening_.SetActive(playerUnit.unit.awake_unit_flag);
    RarityIcon.SetRarity(playerUnit, this.iconRarity_, true);
    ((Component) this).GetComponent<UIRect>().alpha = 1f;
    ((Behaviour) this.effectSE_).enabled = true;
    ((Behaviour) this.animator_).enabled = true;
    this.StartCoroutine(this.doWaitAnimation());
  }

  private IEnumerator doWaitAnimation()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SimpleEffectJobChange simpleEffectJobChange = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ((Component) simpleEffectJobChange).gameObject.SetActive(false);
      Object.Destroy((Object) ((Component) simpleEffectJobChange).gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForAnimation(simpleEffectJobChange.animator_);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
