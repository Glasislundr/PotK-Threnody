import pygame
import os

from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.conf.conf import conf
import lib.StreamingAsset as StreamingAsset
import lib.AssetBundle as AssetBundle

mask_list = [AssetBundle.getGUIMask('mask_Chara_L'),AssetBundle.getGUIMask('mask_Chara_C'),AssetBundle.getGUIMask('mask_Chara_R')]

class Character:
    cid: int
    utype: str
    resId: int
    image: pygame.Surface
    curImg: pygame.Surface
    faceImg: pygame.Surface
    eyeImg: pygame.Surface
    maskOn: bool
    storyPos: int
    pos: list
    story_texture_offset: list
    face_offset: list
    face: str
    eye: str
    faceImgs: dict
    speaking: bool
    alpha: float
    moving: bool
    layer: int

    def __init__(self, cid):
        self.cid = cid
        self.maskOn = True
        self.face = 'normal'
        self.eye = 'normal'
        self.speaking = False
        self.storyPos = -1
        self.alpha = 1.0
        self.baseScale = 1.0
        self.scale = 1.0
        self.moving = False
        self.layer = 0

        self.utype = PotkRes.udict[self.cid]['type']
        if self.utype == 'unit':
            self.resId = PotkRes.udict[self.cid]['resource_reference_unit_id_UnitUnit']
            self.image = StreamingAsset.getUnitArt(self.resId)
            self.baseScale = PotkRes.udict[self.cid]['story_texture_scale']
            self.curImg = pygame.transform.smoothscale_by(self.image.copy(), self.baseScale)
        else:
            self.resId = PotkRes.udict[self.cid]['resource_reference_unit_id_MobUnit']
            self.image = StreamingAsset.getMobArt(self.resId)
            self.baseScale = PotkRes.udict[self.cid]['story_texture_scale']
            self.curImg = pygame.transform.smoothscale_by(self.image.copy(), self.baseScale)
        self.story_texture_offset = (PotkRes.udict[self.cid]['story_texture_x'], PotkRes.udict[self.cid]['story_texture_y'])
        self.face_offset = (PotkRes.udict[self.cid]['face_x'], PotkRes.udict[self.cid]['face_y'])
        self.pos = self.storyPosToUnitPos(self.storyPos)
        theMask = mask_list[min(2,max(0,self.storyPos-1))].copy()
        theMask.blit(self.curImg, (theMask.get_width()/2 - self.curImg.get_width()/2, theMask.get_height()/2 - self.curImg.get_height()/2), special_flags = pygame.BLEND_RGBA_MIN)
        self.curImg = theMask

    def storyPosToUnitPos(self, pos):
        tarPosX = (conf.display_width / 6) * (pos) - (self.curImg.get_width() / 2) + self.story_texture_offset[0]
        tarPosY = conf.display_height - 100 - self.curImg.get_height() - self.story_texture_offset[1]
        
        #tarPosX = (display.get_width() / 6) * (self.pos) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x']
        #tarPosY = display.get_height() - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y']
        return (tarPosX, tarPosY)
    def offsetToUnitPos(self, pos):
        tarPosX = (conf.display_width / 2) - (self.curImg.get_width() / 2) + self.story_texture_offset[0] + pos[0]
        tarPosY = conf.display_height - 100 - self.curImg.get_height() - self.story_texture_offset[1] - pos[1]
        return (tarPosX, tarPosY)
    
    def setLayer(self, layer):
        self.layer = layer
    def changeFace(self, face):
        if face:
            self.face = face
        if self.face != 'normal':
            if self.utype == 'unit':
                self.faceImg = StreamingAsset.getUnitFace(self.resId, face)
            else:
                self.faceImg = StreamingAsset.getMobFace(self.resId, face)
        else:
            self.faceImg = None
        self.rebuildCurImage()
    def changeEye(self, eye):
        if eye:
            self.eye = eye
        if self.eye != 'normal':
            if self.utype == 'unit':
                self.eyeImg = StreamingAsset.getUnitEyes(self.resId, eye)
            else:
                self.eyeImg = StreamingAsset.getMobEyes(self.resId, eye)
        else:
            self.eyeImg = None
        self.rebuildCurImage()
    def setStoryPos(self, pos):
        self.storyPos = pos
        self.pos = self.storyPosToUnitPos(pos)
        #Update mask
    def startMoveToStoryPos(self, pos):
        self.storyPos = pos
        self.moving = True
        #Update mask
    def finishMoving(self):
        self.moving = False
    def setPos(self, pos):
        self.pos = pos
    def setAlpha(self, alpha):
        self.alpha = alpha
        self.rebuildCurImage()
    def setScale(self, scale):
        self.scale = scale
        modscale = scale / self.image.get_height() * self.curImg.get_height()
        print(f'Setting scale (for {self.cid}) to modscale: {modscale}')
        self.curImg = pygame.transform.smoothscale_by(self.curImg, modscale)
        if not self.moving:
            self.pos = self.storyPosToUnitPos(self.storyPos)
    def rebuildCurImage(self):
        self.curImg = self.image.copy()
        if self.face != 'normal' and self.faceImg is not None:
            self.curImg.blit(self.faceImg, (self.face_offset[0],self.curImg.get_height() - self.faceImg.get_height() - self.face_offset[1]))
        if self.eye != 'normal' and self.eyeImg is not None:
            self.curImg.blit(self.eyeImg, (self.face_offset[0],self.curImg.get_height() - self.eyeImg.get_height() - self.face_offset[1]))
        self.curImg = pygame.transform.smoothscale_by(self.curImg, self.scale * self.baseScale)
        theMask = mask_list[min(2,max(0,self.storyPos-1))].copy()
        theMask.blit(self.curImg, (theMask.get_width()/2 - self.curImg.get_width()/2, theMask.get_height()/2 - self.curImg.get_height()/2), special_flags = pygame.BLEND_RGBA_MIN)
        self.curImg = theMask
        self.curImg.set_alpha(int(self.alpha * 255))
        
    def draw(self, display):
        
        #tarPosX = (display.get_width() / 6) * (self.storyPos) - (self.curImg.get_width() / 2) + PotkRes.udict[self.cid]['story_texture_x']
        #tarPosY = display.get_height() - 100 - self.curImg.get_height() - PotkRes.udict[self.cid]['story_texture_y']
        display.blit(self.curImg, self.pos)#(tarPosX, tarPosY))