import os
from lib.downloader.paths import Paths

PATH = os.path.dirname(os.path.dirname(os.path.realpath(__file__)))

class PotkPaths:
    paths = Paths()

    localRootPath = PATH
    resRootPath = os.path.join(PATH,'extracted')
    masterdataPath = os.path.join(PATH, r'masterdata')
    scriptPath = os.path.join(masterdataPath, 'ScriptScript')
    streamingAssetPath = os.path.join(resRootPath, r'StreamingAssets')
    soundRootPath = os.path.join(streamingAssetPath, r'android\wav')
    bkgPath = r'Prefabs\BackGround'
    assetBundlePath = r'AssetBundle\Resources'
    maskPath = r'GUI\009-3_sozai'
    storyGuiPath = r'GUI\009-3_sozai\009-3_sozai_prefab'
    iconArtPath = r'Icons'
    unitArtPath = r'AssetBundle\Resources\Units'
    unitMobArtPath = r'AssetBundle\Resources\MobUnits'
    unitArt2DPath = '2D'
    unitArtFacePath = 'Face'
    unitArtEyePath = 'Eye'
    specialEffectArtPath = os.path.join(assetBundlePath, 'EventImages')
    
    font_path = os.path.join(localRootPath, 'static', 'NotoSansJP-Regular.ttf')
    
    MusicFilename = 'Music.json'
    UnitUnitFilename = 'UnitUnit.json'
    UnitUnitStoryFilename = 'UnitUnitStory.json'
    
    @staticmethod
    def getInternalBackgroundPath(name):
        return os.path.join(PotkPaths.bkgPath, name).replace('\\','/')
    
    @staticmethod
    def getExtractedBackgroundPath(name):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.bkgPath, name + '.png')
    
    @staticmethod
    def getIconArtPath(name):
        return os.path.join(PotkPaths.resRootPath, PotkPaths.iconArtPath, name + '.png')
    
    @staticmethod
    def getInternalSpecialEffectArtPath(seName):
        return os.path.join(PotkPaths.specialEffectArtPath, seName).replace('\\','/')
    
    @staticmethod
    def getExtractedSpecialEffectArtPath(seName):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.specialEffectArtPath, seName + '.png')
    
    @staticmethod
    def getInternalUnitArtPath(resId):
        return os.path.join(PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, 'unit_large').replace('\\','/')

    @staticmethod
    def getExtractedUnitArtPath(resId):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, 'unit_large.png')
    
    @staticmethod
    def getInternalMobArtPath(resId):
        return os.path.join(PotkPaths.unitMobArtPath, str(resId), 'unit_large').replace('\\','/')
        
    @staticmethod
    def getExtractedMobArtPath(resId):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitMobArtPath, str(resId), 'unit_large.png')
        
    @staticmethod
    def getUnitFaceFolder(resId):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtFacePath)
        
    @staticmethod
    def getMobFaceFolder(resId):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtFacePath)
        
    @staticmethod
    def getInternalUnitFacePath(resId, face):
        return os.path.join(PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtFacePath, face).replace('\\','/')
        
    @staticmethod
    def getExtractedUnitFacePath(resId, face):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtFacePath, face + '.png')
        
    @staticmethod
    def getInternalMobFacePath(resId, face):
        return os.path.join(PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtFacePath, face).replace('\\','/')
        
    @staticmethod
    def getExtractedMobFacePath(resId, face):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtFacePath, face + '.png')
        
    @staticmethod
    def getInternalUnitEyePath(resId, eye):
        return os.path.join(PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtEyePath, eye).replace('\\','/')
        
    @staticmethod
    def getExtractedUnitEyePath(resId, eye):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitArtPath, str(resId), PotkPaths.unitArt2DPath, PotkPaths.unitArtEyePath, eye + '.png')
        
    @staticmethod
    def getInternalMobEyePath(resId, eye):
        return os.path.join(PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtEyePath, eye).replace('\\','/')
        
    @staticmethod
    def getExtractedMobEyePath(resId, eye):
        return os.path.join(PotkPaths.streamingAssetPath, PotkPaths.unitMobArtPath, str(resId), PotkPaths.unitArtEyePath, eye + '.png')
        
    @staticmethod
    def getExtractedMasterDataFilePath(scriptId):
        return os.path.join(PotkPaths.masterdataPath, scriptId+'.json')

    @staticmethod
    def getInternalMasterDataPath(scriptId):
        return 'MasterData/' + scriptId
        
    @staticmethod
    def getExtractedScriptFilePath(scriptId):
        return os.path.join(PotkPaths.scriptPath, scriptId+'.json')

    @staticmethod
    def getInternalScriptPath(scriptId):
        return 'MasterData/ScriptScript_part_' + scriptId

    @staticmethod
    def getMusicPath(fileName):
        return os.path.join(PotkPaths.soundRootPath, fileName)

    @staticmethod
    def getMusicFilePath(file):
        return os.path.join(PotkPaths.soundRootPath, file)
        
        