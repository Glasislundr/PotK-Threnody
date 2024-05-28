// Decompiled with JetBrains decompiler
// Type: DetailControllerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
public class DetailControllerData
{
  public string body;
  public int? image_height;
  public string image_url;
  public int? image_width;
  public int? extra_type;
  public int? extra_id;
  public int? extra_position;
  public string scene_name;
  public string param_name;

  public DetailControllerData(OfficialInformationArticleBodies data)
  {
    this.body = data.body;
    this.image_height = new int?(data.img_height);
    this.image_url = data.body_img_url;
    this.image_width = new int?(data.img_width);
    this.extra_type = new int?(data.extra_type);
    this.extra_id = new int?(data.extra_id);
    this.extra_position = new int?(data.extra_position);
    this.scene_name = data.scene_name;
    this.param_name = data.param_name;
  }

  public DetailControllerData(GachaDescriptionBodies data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(QuestScoreCampaignDescriptionBlockBodies data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(DescriptionBodies data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(GuildBankHowto data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(TowerHowto data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(ClassRankingHowto data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(GuildRaidHowto data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(QuestExtraDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
    this.extra_type = data.extra_type;
    this.extra_id = data.extra_id;
    this.extra_position = data.extra_position;
  }

  public DetailControllerData(CoinProductDetail data)
  {
    this.body = data.detail;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(CoinBonusDetail data)
  {
    this.body = data.body;
    this.image_height = new int?(data.image_height);
    this.image_url = data.image_url;
    this.image_width = new int?(data.image_width);
  }

  public DetailControllerData(SimplePackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(BeginnerPackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(StepupPackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(WeeklyPackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(MonthlyPackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }

  public DetailControllerData(PackDescription data)
  {
    this.body = data.body;
    this.image_height = data.image_height;
    this.image_url = data.image_url;
    this.image_width = data.image_width;
  }
}
