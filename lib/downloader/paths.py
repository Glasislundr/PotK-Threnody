import json
import os
import urllib.request
import gzip
from .api import Enviroment
from .version import update_app_ver, update_dlc_ver
import gzip

PATH = os.path.dirname(os.path.dirname(os.path.dirname(os.path.realpath(__file__))))
VER_FP = os.path.join(PATH, "data", "version.json")


class Paths:
    def __init__(self, download_when_present=False):
        if os.path.exists(os.path.join(PATH, "data", "paths.json")) and not download_when_present:
            with open(os.path.join(PATH, "data", "paths.json"), 'r', encoding="utf-16") as pfile:
                PATHS = json.loads(pfile.read())
            self.AssetBundle = PATHS['AssetBundle']
            self.Resource = PATHS["Resource"]
            self.StreamingAssets = PATHS["StreamingAssets"]
        else:
            # downloads paths
            print("DOWNLOAD PATHS")
            env = Enviroment(True)
            self.get_versions()
            try:
                url = "{0}{1}_{2}.json".format(env.DlcPath, self.APP_VER, self.DLC_VER)
                PATHS = json.loads(gzip.decompress(urllib.request.urlopen(url).read()))
            except:
                print("application version and/or dlc version are outdated")
                print("downloading latest apk from QooApp to update the version values")
                APP_VER = update_app_ver()
                DLC_VER = update_dlc_ver(APP_VER)
                url = "{0}{1}_{2}.json".format(env.DlcPath, APP_VER, DLC_VER)
                PATHS = json.loads(gzip.decompress(urllib.request.urlopen(url).read()))

            os.makedirs(os.path.join(PATH, "data"), exist_ok=True)
            # open(os.path.join(PATH, *['data', 'paths.json']), 'wb').write(gzip.decompress(data))

            # PATHS = json.load(open(os.path.join(PATH, *['data','paths.json']), 'rb'))
            self.AssetBundle = {
                key: {
                    "FileName": item[0],
                    "ObjectType": ObjectType[item[1]],
                    "FileSize": item[2],
                    "Steps": item[3],  # [StepsType[x] for x in item[3]]
                }
                for key, item in PATHS["AssetBundle"].items()
            }
            self.Resource = {
                key: {
                    "ObjectType": ObjectType[item[0]],
                    "Steps": item[1],  # [StepsType[x] for x in item[1]]
                }
                for key, item in PATHS["Resource"].items()
            }
            self.StreamingAssets = {
                key: {
                    "FileName": item[0],
                    "Extension": item[1],
                    "ObjectType": ObjectType[item[2]],
                    "FileSize": item[3],
                    "Steps": item[4],  # [StepsType[x] for x in item[4]]
                }
                for key, item in PATHS["StreamingAssets"].items()
            }
            with open(os.path.join(PATH, "data", "paths.json"), "wb") as f:
                f.write(json.dumps(self.__dict__, ensure_ascii=False).encode("utf16"))
            print("- finished")

    def __getitem__(self, key):
        return self.__dict__[key]

    def get_versions(self):
        try:
            with open(VER_FP, "rb") as f:
                JSON = json.loads(f.read())
            self.APP_VER = JSON["app_ver"]
            self.DLC_VER = update_dlc_ver(self.APP_VER)
            if not self.DLC_VER:
                raise FileNotFoundError()
        except (FileNotFoundError, json.decoder.JSONDecodeError):
            self.APP_VER = update_app_ver()
            self.DLC_VER = update_dlc_ver(self.APP_VER)
            with open(VER_FP, "wb") as f:
                f.write(
                    json.dumps(
                        {"app_ver": self.APP_VER, "dlc_ver": self.DLC_VER}, indent="\t"
                    ).encode("utf8")
                )


AssetBundleField = {
    0: "FileName",
    1: "ObjectType",
    2: "FileSize",
    3: "Steps",
}

StreamingAssetsField = {
    0: "FileName",
    1: "Extension",
    2: "ObjectType",
    3: "FileSize",
    4: "Steps",
}

ResourceField = {
    0: "ObjectType",
    1: "Steps",
}

PathType = {
    0: "None",
    1: "AssetBundle",
    2: "StreamingAssets",
    3: "Resource",
}

ObjectType = {
    0: "None",
    1: "AnimationClip",
    2: "AnimatorController",
    3: "FontPhysicMaterial",
    4: "GameObject",
    5: "Material",
    6: "MovieTexture",
    7: "Object",
    8: "Shader",
    9: "TextAsset",
    10: "Texture2D",
}

StepsType = {
    0: "Renderer",
    1: "UIAtlas",
    2: "UILable",
    3: "UISprite",
}
