import os
import json
import pygame

from lib.PotkPaths import PotkPaths
from lib.PotkRes import PotkRes
from lib.downloader.paths import PATH
from lib.Spritesheet import Spritesheet

import lib.downloader.UnityPy as UnityPy

cache = os.path.join(PATH,*['data','cache'])

loaded_texture_2ds = {}
def getTexture2D(epath, ipath):
    if ipath in loaded_texture_2ds:
        return loaded_texture_2ds[ipath]
    if os.path.exists(epath):
        return PotkRes.getGameImage(epath)
    rawName = PotkPaths.paths['AssetBundle'][ipath]['FileName']
    cpath = os.path.join(cache, rawName)
    if os.path.exists(cpath):
        with open(cpath, "rb") as f:
            data = f.read()
        uenv = UnityPy.load(data)
        for obj in uenv.objects:
            if str(obj.type) == "ClassIDType.Texture2D":
                img = obj.read().image
                if img is not None:
                    loaded_texture_2ds[ipath] = img
                    return pygame.image.fromstring(img.tobytes(), img.size, img.mode)
    return None

def getFromGameObject(epath, ipath):
    if os.path.exists(epath):
        return PotkRes.getGameImage(epath)
    rawName = PotkPaths.paths['AssetBundle'][ipath]['FileName']
    cpath = os.path.join(cache, rawName)
    if os.path.exists(cpath):
        return PotkRes.getGameImage(cpath)
    else:
        return None

def getBackground(bgname):
    epath = PotkPaths.getExtractedBackgroundPath(bgname)
    ipath = PotkPaths.getInternalBackgroundPath(bgname)
    return getTexture2D(epath, ipath)
    
def getGUI(guiID):
    eImgPath = os.path.join(PotkPaths.resRootPath, 'GUI', guiID, guiID + '_prefab', guiID + '_png.png')
    eBhvPath = os.path.join(PotkPaths.resRootPath, 'GUI', guiID, guiID + '_prefab', 'MonoBehaviour_8.json')
    if os.path.exists(eImgPath) and os.path.exists(eBhvPath):
        return GUIElements(eImgPath, eBhvPath)
    else:
        return GUIElements(guiID)

def getGUIMask(mask):
    ipath = r'GUI/009-3_sozai/' + mask
    
    if ipath in loaded_texture_2ds:
        return loaded_texture_2ds[ipath]
    
    epath = os.path.join(PotkPaths.resRootPath, PotkPaths.maskPath, mask + '.png')
    loadedMask = getTexture2D(epath, ipath)
    recolor = pygame.Surface(loadedMask.get_size(), pygame.SRCALPHA)
    recolor.fill((255,255,255,0))
    loadedMask.blit(recolor, (0,0), special_flags = pygame.BLEND_RGBA_MAX)
    return loadedMask

class GUIElements():
    image: pygame.Surface
    subimgs: dict
    spritesheet: Spritesheet
    
    def __init__(self, name, sprites_file=None):
        if sprites_file is None:
            # The name is just the GUI name, loading from the cache
            self.subimgs = {}
            ipath = 'GUI/' + name + '/' + name + '_prefab'
            if ipath in loaded_texture_2ds:
                self.image = loaded_texture_2ds[ipath]
            rawName = PotkPaths.paths['AssetBundle'][ipath]['FileName']
            cpath = os.path.join(cache, rawName)
            if os.path.exists(cpath):
                with open(cpath, "rb") as f:
                    data = f.read()
                uenv = UnityPy.load(data)
                for obj in uenv.objects:
                    if str(obj.type) == "ClassIDType.Texture2D":
                        img = obj.read().image
                        if img is not None:
                            loaded_texture_2ds[ipath] = img
                            self.image = pygame.image.fromstring(img.tobytes(), img.size, img.mode).convert_alpha()
                    elif str(obj.type) == "ClassIDType.MonoBehaviour":
                        if obj.serialized_type.nodes:
                            tree = obj.read_typetree()
                            for subimg in tree['mSprites']:
                                self.subimgs[subimg['name'].split('.')[0]] = subimg
                                del subimg['name']
        else:
            # The name is the file path for the image file, and the sprites file points to the json
            self.subimgs = {}
            self.image = pygame.image.load(name).convert_alpha()
            
            with open(sprites_file, 'r') as f:
                tree = json.load(f)
            for subimg in tree['mSprites']:
                self.subimgs[subimg['name'].split('.')[0]] = subimg
                
        self.spritesheet = Spritesheet(self.image)
        
        for key,img in self.subimgs.items():
            img['leftBorderImg'] = self.spritesheet.image_at((img['x'], img['y'], img['borderLeft'], img['height']))
            img['rightBorderImg'] = self.spritesheet.image_at((img['x']+img['width']-img['borderRight'], img['y'], img['borderRight'], img['height']))
            img['topBorderImg'] = self.spritesheet.image_at((img['x'], img['y'], img['width'], img['borderTop']))
            img['bottomBorderImg'] = self.spritesheet.image_at((img['x'], img['y']+img['height']-img['borderBottom'], img['width'], img['borderBottom']))
            img['coreImg'] = self.spritesheet.image_at((img['x']+img['borderLeft'], img['y']+img['borderTop'], img['width']-img['borderLeft']-img['borderRight'], img['height']-img['borderTop']-img['borderBottom']))
            
    def getSubImg(self, name, size=None):
        img = self.subimgs[name]
        if size is None:
            size = (img['width'], img['height'])
        outImg = pygame.Surface(size).convert_alpha()
        outImg.fill((0,0,0,0))
        coreSize = (max(size[0]-img['borderLeft']-img['borderRight'],0), max(size[1]-img['borderTop']-img['borderBottom'],0))
        if coreSize == img['coreImg'].get_size():
            outImg.blit(img['leftBorderImg'], (0, 0))
            outImg.blit(img['rightBorderImg'], (size[0]-img['borderRight'], 0))
            outImg.blit(img['topBorderImg'], (0, 0))
            outImg.blit(img['bottomBorderImg'], (0, size[1]-img['borderBottom']))
            outImg.blit(img['coreImg'], (img['borderLeft'], img['borderTop']))
        else:
            if size[0] != img['width']:
                tpBdr = img['topBorderImg'].copy()
                tpBdr = pygame.transform.smoothscale(tpBdr, (size[0],tpBdr.get_height()))
                outImg.blit(tpBdr, (0, 0))
                btmBdr = img['bottomBorderImg'].copy()
                btmBdr = pygame.transform.smoothscale(btmBdr, (size[0],btmBdr.get_height()))
                outImg.blit(btmBdr, (0, size[1]-img['borderBottom']))
            else:
                outImg.blit(img['topBorderImg'], (0, 0))
                outImg.blit(img['bottomBorderImg'], (0, size[1]-img['borderBottom']))
                
            if size[1] != img['height']:
                lBdr = img['leftBorderImg'].copy()
                lBdr = pygame.transform.smoothscale(lBdr, (lBdr.get_width(), size[1]))
                outImg.blit(lBdr, (0, 0))
                rBdr = img['rightBorderImg'].copy()
                rBdr = pygame.transform.smoothscale(rBdr, (rBdr.get_width(), size[1]))
                outImg.blit(rBdr, (size[0]-img['borderRight'], 0))
            else:
                outImg.blit(img['leftBorderImg'], (0, 0))
                outImg.blit(img['rightBorderImg'], (size[0]-img['borderRight'], 0))
            
            sclCore = pygame.transform.smoothscale(img['coreImg'], coreSize)
            outImg.blit(sclCore, (img['borderLeft'], img['borderTop']))
        return outImg