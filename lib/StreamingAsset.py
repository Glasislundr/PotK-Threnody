import os
import json

from lib.PotkPaths import PotkPaths
from lib.PotkRes import PotkRes
from lib.downloader.paths import PATH

cache = os.path.join(PATH,*['data','cache'])

def getStreamingAsset(epath, ipath):
    if os.path.exists(epath):
        return PotkRes.getGameImage(epath)
    rawName = PotkPaths.paths['StreamingAssets'][ipath]['FileName']
    cpath = os.path.join(cache, rawName)
    if os.path.exists(cpath):
        return PotkRes.getGameImage(cpath)
    else:
        return None
    

def getSpecialEffect(seName):
    if seName == 'bg_red':
        seName = 'bg_white'
        epath = PotkPaths.getExtractedSpecialEffectArtPath(seName)
        ipath = PotkPaths.getInternalSpecialEffectArtPath(seName)
        if os.path.exists(epath):
            return PotkRes.getGameImage(epath)
        bge = getStreamingAsset(epath, ipath).copy()
        bge.fill((255,0,0))
        return bge
        
    epath = PotkPaths.getExtractedSpecialEffectArtPath(seName)
    ipath = PotkPaths.getInternalSpecialEffectArtPath(seName)
    return getStreamingAsset(epath, ipath)

def getUnitArt(resId):
    epath = PotkPaths.getExtractedUnitArtPath(resId)
    ipath = PotkPaths.getInternalUnitArtPath(resId)
    return getStreamingAsset(epath, ipath)

def getUnitFace(resId, face):
    epath = PotkPaths.getExtractedUnitFacePath(resId, face)
    ipath = PotkPaths.getInternalUnitFacePath(resId, face)
    return getStreamingAsset(epath, ipath)

def getUnitEyes(resId, eye):
    epath = PotkPaths.getExtractedUnitEyePath(resId, eye)
    ipath = PotkPaths.getInternalUnitEyePath(resId, eye)
    return getStreamingAsset(epath, ipath)

def getMobArt(resId):
    epath = PotkPaths.getExtractedMobArtPath(resId)
    ipath = PotkPaths.getInternalMobArtPath(resId)
    return getStreamingAsset(epath, ipath)

def getMobFace(resId, face):
    epath = PotkPaths.getExtractedMobFacePath(resId, face)
    ipath = PotkPaths.getInternalMobFacePath(resId, face)
    return getStreamingAsset(epath, ipath)

def getMobEyes(resId, eye):
    epath = PotkPaths.getExtractedMobEyePath(resId, eye)
    ipath = PotkPaths.getInternalMobEyePath(resId, eye)
    return getStreamingAsset(epath, ipath)
