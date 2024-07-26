import pygame
import os

from lib.PotkRes import PotkRes
from lib.PotkPaths import PotkPaths
from lib.conf.conf import conf
import lib.StreamingAsset as StreamingAsset
import lib.AssetBundle as AssetBundle

mask_list = [AssetBundle.getGUIMask('mask_Chara_L'),AssetBundle.getGUIMask('mask_Chara_C'),AssetBundle.getGUIMask('mask_Chara_R')]
pos_list = [0, conf.display_width*.19991, conf.display_width*.2778, conf.display_width*.4889, conf.display_width*.7574, conf.display_width*.8148]


class Character:
    cid: int
    utype: str
    resId: int
    image: pygame.Surface
    sideMaskImg: pygame.Surface
    centerMaskImg: pygame.Surface
    
    curImg: pygame.Surface
    maskedImg: pygame.Surface
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
            
        self.centerMaskImg = mask_list[1].copy()
        self.sideMaskImg = mask_list[0].copy()
            
        self.story_texture_offset = (PotkRes.udict[self.cid]['story_texture_x'], PotkRes.udict[self.cid]['story_texture_y'])
        self.face_offset = (PotkRes.udict[self.cid]['face_x'], PotkRes.udict[self.cid]['face_y'])
        self.rebuildMaskedImage()
        if self.cid == 44:
            print(f'Reb current size: {self.curImg.get_width()},{self.curImg.get_height()}')
            print(f'Mask current size: {self.maskedImg.get_width()},{self.maskedImg.get_height()}')
        self.pos = self.storyPosToUnitPos(self.storyPos)

    def storyPosToUnitPos(self, pos):
        # if self.cid == 502711:
            # print(f'Damo current height: {self.curImg.get_height()}')
            # if hasattr(self,'pos'):
                # print(f'Damo current pos: {self.pos}')
        if self.maskOn:
            tarPosX = pos_list[pos] - (self.maskedImg.get_width() / 2) + self.story_texture_offset[0]
            tarPosY = (conf.display_height/2) - (self.maskedImg.get_height()/2) + self.story_texture_offset[1]
        else:
            tarPosX = pos_list[pos] - (self.curImg.get_width() / 2) + self.story_texture_offset[0]
            tarPosY = (conf.display_height/2) - (self.curImg.get_height()/2) + self.story_texture_offset[1]
            
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
    def setMaskOn(self, maskOn):
        self.maskOn = maskOn
        self.pos = self.storyPosToUnitPos(self.storyPos)
        self.rebuildCurImage()
    def setStoryPos(self, pos):
        self.storyPos = pos
        self.pos = self.storyPosToUnitPos(pos)
        self.rebuildCurImage()
    def startMoveToStoryPos(self, pos):
        self.storyPos = pos
        self.moving = True
        self.rebuildCurImage()
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
        #print(f'Setting scale (for {self.cid}) to modscale: {modscale}')
        self.curImg = pygame.transform.smoothscale_by(self.curImg, modscale)
        self.rebuildMaskedImage()
        if not self.moving:
            self.pos = self.storyPosToUnitPos(self.storyPos)
    def rebuildCurImage(self):
        self.curImg = self.image.copy()
        if self.face != 'normal' and self.faceImg is not None:
            self.curImg.blit(self.faceImg, (self.face_offset[0],self.curImg.get_height() - self.faceImg.get_height() - self.face_offset[1]))
        if self.eye != 'normal' and self.eyeImg is not None:
            self.curImg.blit(self.eyeImg, (self.face_offset[0],self.curImg.get_height() - self.eyeImg.get_height() - self.face_offset[1]))
        if self.scale * self.baseScale != 1:
            self.curImg = pygame.transform.smoothscale_by(self.curImg, self.scale * self.baseScale)
        self.curImg.set_alpha(int(self.alpha * 255))
        self.rebuildMaskedImage()
    def rebuildMaskedImage(self):
        if self.maskOn:
            if self.storyPos == 3:
                theMaskBase = self.centerMaskImg
            else:
                theMaskBase = self.sideMaskImg
            theMaskBase.fill((0,0,0,0))
            theMask = mask_list[min(2,max(0,self.storyPos-2))]
            newMaskedImage = self.curImg.copy()
            newMaskedImage.blit(theMask, (newMaskedImage.get_width()/2 - theMask.get_width()/2, newMaskedImage.get_height()/2 - theMask.get_height()/2), special_flags = pygame.BLEND_RGBA_MIN)
            theMaskBase.blit(newMaskedImage, (theMaskBase.get_width()/2 - newMaskedImage.get_width()/2, theMaskBase.get_height()/2 - newMaskedImage.get_height()/2))
            #theMaskBase.blit(self.curImg, (theMask.get_width()/2 - self.curImg.get_width()/2, theMask.get_height()/2 - self.curImg.get_height()/2), special_flags = pygame.BLEND_RGBA_MIN)
            self.maskedImg = theMaskBase
        self.maskedImg.set_alpha(int(self.alpha * 255))
        
    def draw(self, display):
        if self.maskOn:
            display.blit(self.maskedImg, self.pos)
        else:
            display.blit(self.curImg, self.pos)