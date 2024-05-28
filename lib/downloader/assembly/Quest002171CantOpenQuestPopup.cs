// Decompiled with JetBrains decompiler
// Type: Quest002171CantOpenQuestPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest002171CantOpenQuestPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UI2DSprite keySprite;
  [SerializeField]
  private UILabel txtPossession;

  public IEnumerator Init(string name, int key_id, int key_quantity, int consume_quantity)
  {
    IEnumerator e = this.CreateKeySprite(key_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtPossession.SetTextLocalize(key_quantity);
    this.txtTitle.SetText(Consts.GetInstance().QUEST_002171_CANTRELEASEPOPUP_TITLE);
    this.txtDescription.SetText(Consts.Format(Consts.GetInstance().QUEST_002171_CANTRELEASEPOPUP_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (name),
        (object) name
      },
      {
        (object) "consume",
        (object) consume_quantity.ToLocalizeNumberText()
      }
    }));
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator CreateKeySprite(int keyID)
  {
    Future<Sprite> spriteF = MasterData.QuestkeyQuestkey[keyID].LoadSpriteThumbnail();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.keySprite.sprite2D = spriteF.Result;
  }
}
