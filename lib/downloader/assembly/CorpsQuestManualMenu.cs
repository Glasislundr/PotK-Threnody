// Decompiled with JetBrains decompiler
// Type: CorpsQuestManualMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/ManualMenu")]
public class CorpsQuestManualMenu : ManualMenuBase
{
  private int settingID_;
  private const int KIND_TITLE = 1;
  private const int KIND_BODY = 2;

  public IEnumerator InitializeAsync(int setting_id)
  {
    CorpsQuestManualMenu corpsQuestManualMenu = this;
    corpsQuestManualMenu.settingID_ = setting_id;
    IEnumerator e = corpsQuestManualMenu.doInitialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override string getTitle()
  {
    return Array.Find<CorpsHowto>(MasterData.CorpsHowtoList, (Predicate<CorpsHowto>) (x => x.kind == 1 && x.setting_CorpsSetting == this.settingID_))?.body ?? string.Empty;
  }

  protected override ManualMenuBase.BodyParam[] getBodies()
  {
    return ((IEnumerable<CorpsHowto>) MasterData.CorpsHowtoList).Where<CorpsHowto>((Func<CorpsHowto, bool>) (x => x.kind >= 2 && x.setting_CorpsSetting == this.settingID_)).Select<CorpsHowto, ManualMenuBase.BodyParam>((Func<CorpsHowto, ManualMenuBase.BodyParam>) (y => new ManualMenuBase.BodyParam()
    {
      body = y.body,
      image_height = y.image_height,
      image_url = y.image_url,
      image_width = y.image_width
    })).ToArray<ManualMenuBase.BodyParam>();
  }
}
