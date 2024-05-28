import os
from lib.downloader.paths import Paths

PATH = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))

class PotkPaths:
    paths = Paths()

    localRootPath = PATH
    resRootPath = os.path.join(PATH,'extracted')
    masterdataPath = os.path.join(PATH, r'masterdata')
    scriptPath = os.path.join(masterdataPath, 'ScriptScript')
    soundRootPath = os.path.join(resRootPath, r'StreamingAssets\android\wav')
    bkgPath = r'Prefabs\BackGround'
    assetBundlePath = r'AssetBundle\Resources'
    maskPath = r'GUI\009-3_sozai'
    storyGuiPath = r'GUI\009-3_sozai\009-3_sozai_prefab'
    iconArtPath = r'Icons'
    unitArtPath = r'StreamingAssets\AssetBundle\Resources\Units'
    unitMobArtPath = r'StreamingAssets\AssetBundle\Resources\MobUnits'
    unitArt2DPath = '2D'
    unitArtFacePath = 'Face'
    specialEffectArtPath = os.path.join(r'StreamingAssets', assetBundlePath, 'EventImages')
    
    font_path = os.path.join(localRootPath, 'resources/static', 'NotoSansJP-Regular.ttf')
    
    MusicFilename = 'Music.json'
    UnitUnitFilename = 'UnitUnit.json'
    UnitUnitStoryFilename = 'UnitUnitStory.json'
    
    @staticmethod
    def getSpecialEffectArtPath(seName):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.specialEffectArtPath, seName + '.png')
    
    @staticmethod
    def getIconArtPath(name):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.iconArtPath, name + '.png')
    
    @staticmethod
    def getUnitArtPath(resId):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, 'unit_large.png')
    
    @staticmethod
    def getMobArtPath(resId):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitMobArtPath, str(resId), 'unit_large.png')
        
    @staticmethod
    def getUnitFaceFolder(resId):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtFacePath)
        
    @staticmethod
    def getMobFaceFolder(resId):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtFacePath)
        
    @staticmethod
    def getUnitFacePath(resId, face):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtFacePath, face)
        
    @staticmethod
    def getMobFacePath(resId, face):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtFacePath, face)
        
    @staticmethod
    def getScriptFilePath(scriptId):
        return os.path.join(PotkPaths.scriptPath, scriptId+'.json')

    @staticmethod
    def getMusicPath(fileName):
        return os.path.join(PotkPaths.soundRootPath, fileName)

    @staticmethod
    def getMusicFilePath(file):
        return os.path.join(PotkPaths.soundRootPath, file)
        
        