import os
import json
import pygame
import traceback

from lib.spritesheet import spritesheet
from lib.PotkPaths import PotkPaths

def extract_assetbundle(args):
    fpath, item = args
    try:
        sfp = os.path.join(SRC, item["FileName"])

        data = open(sfp, "rb").read()

        dfp = os.path.join(DST, *fpath.split("/"))
        uenv = UnityPy.load(data)
        objects = uenv.objects

        logger.info('Attempting to extract '+fpath +' with ' + str(len(objects))+' objects found')
        extracted = []
        for obj in objects:
            if obj.path_id not in extracted:
                extracted.extend(AssetBatchConverter.export_obj(obj, dfp, len(objects) > 2))
        return fpath, len(extracted)
    except Exception as e:
        print(e)
        return fpath, 0


def extract_streamingasset(args):
    fpath, item = args
    
    sfp = os.path.join(SRC, item["FileName"])
    dfp = os.path.join(DST, "StreamingAssets", *fpath.split("/")) + item["Extension"]
    os.makedirs(os.path.dirname(dfp), exist_ok=True)
    shutil.copyfile(sfp,dfp)
    
    return fpath

class PotkRes:
    name_font = pygame.font.Font(PotkPaths.font_path, 24)

    loadedIcons = {}
    loadedImages = {}
    loadedSounds = {}
    
    @classmethod
    def getIcon(cls, name):
        if name in cls.loadedIcons:
            return cls.loadedIcons[name]
        else:
            img = pygame.image.load(PotkPaths.getIconArtPath(name))
            cls.loadedIcons[name] = img
            return img
    
    @classmethod
    def getGameImage(cls, path):
        if path in cls.loadedImages:
            return cls.loadedImages[path]
        else:
            img = pygame.image.load(path)
            cls.loadedImages[path] = img
            return img

    @classmethod
    def getVoiceLine(cls, cid, cue):
        skey = str(cid) + '_#_' + cue
        if skey in cls.loadedSounds:
            return cls.loadedSounds[skey]
        else:
            sndPath = PotkRes.findMusicByCue(os.path.join(PotkPaths.soundRootPath, 'VO_' + str(cid)), cue)
            if sndPath:
                if not os.path.exists(sndPath):
                    print(f'Couldn\'t find voice file: {sndPath}')
                    return None
                snd = pygame.mixer.Sound(sndPath)
                cls.loadedSounds[skey] = snd
                return snd
        return None
    
    @classmethod
    def getSoundEffect(cls, cue):
        if cue in cls.loadedSounds:
            return cls.loadedSounds[cue]
        else:
            sndPath = PotkRes.findMusicByCue(os.path.join(PotkPaths.soundRootPath, 'SECueSheet'), cue)
            if not sndPath:
                sndPath = PotkRes.findMusicByCue(os.path.join(PotkPaths.soundRootPath, 'SECueSheet_2'), cue)
            if sndPath:
                if not os.path.exists(sndPath):
                    print(f'Couldn\'t find sound file: {sndPath}')
                    return None
                snd = pygame.mixer.Sound(sndPath)
                cls.loadedSounds[cue] = snd
                return snd
        return None

    @staticmethod
    def findMusicByCue(path, cue):
        if os.path.exists(path):
            for fp in os.listdir(path):
                if cue in fp:
                    return os.path.join(path,fp)
        print(f'Couldn\'t find music file: {path}/{cue}')
        return None
        
    try:
        #========================
        # Load GUI elements
        guiSpritesheet = spritesheet(os.path.join(PotkPaths.resRootPath, PotkPaths.storyGuiPath, '009-3_sozai_png.png'))
        guiDiagBorder = guiSpritesheet.image_at((565, 357, 700-565, 868-357))
        guiDiagBorderBot = pygame.transform.smoothscale(pygame.transform.rotate(guiDiagBorder.copy(), 90), (720, 190))
        guiDiagBorderTop = pygame.transform.smoothscale(pygame.transform.rotate(guiDiagBorder, -90), (720, 190))

        guiQuestionTextBox = guiSpritesheet.image_at((0, 457, 485, 578-457))
        guiQuestionAnswerButton = pygame.transform.smoothscale(guiSpritesheet.image_at((0, 4, 367, 93-4)), (616, 100))
        guiQuestionAnswerButtonClicked = pygame.transform.smoothscale(guiSpritesheet.image_at((611, 934, 978-611, 1023-934)), (616, 100))
        guiQuestionQuestionText = guiSpritesheet.image_at((367, 382, 561-367, 453-382))
        guiQuestionArrowDark = guiSpritesheet.image_at((611, 880, 1012-611, 930-880))
        guiQuestionArrowLight = guiSpritesheet.image_at((704, 709, 1006-704, 751-709))

        guiTextBoxBackgroundR = pygame.transform.smoothscale(pygame.transform.flip(guiSpritesheet.image_at((0, 872, 608, 1023-872)), True, True), (684, 159))
        guiTextBoxBackgroundG = pygame.transform.smoothscale(guiSpritesheet.image_at((0, 363, 363, 453-363)), (684, 159))
        guiTextBoxNameP = guiSpritesheet.image_at((371, 5, 608-371, 34-5))
        guiTextBoxNameG = guiSpritesheet.image_at((612, 5, 849-612, 34-5))
        guiTextBoxStandardFrameY = pygame.transform.smoothscale(guiSpritesheet.image_at((704, 757, 861-704, 876-757)), (618,119))
        guiTextBoxStandardFrameB = pygame.transform.smoothscale(guiSpritesheet.image_at((865, 756, 1012-865, 876-756)), (618,119))
        guiTextBoxSpikeFrameO = pygame.transform.smoothscale(guiSpritesheet.image_at((0, 98, 343, 260-98)), (618,119))
        guiTextBoxSpikeFrameY = pygame.transform.smoothscale(guiSpritesheet.image_at((347, 98, 675-347, 260-98)), (618,119))
        guiTextBoxThoughtFrameP = pygame.transform.smoothscale(guiSpritesheet.image_at((0, 720, 561, 868-720)), (618,119))
        guiTextBoxThoughtFrameG = pygame.transform.smoothscale(guiSpritesheet.image_at((0, 582, 450, 716-582)), (618,119))
        guiTextBoxNextArrow = guiSpritesheet.image_at((853, 8, 892-853, 34-8))

        guiTextBoxSpeaking = guiSpritesheet.image_at((453, 584, 495-453, 637-584))
        guiTextBoxSpeakingFlip = pygame.transform.flip(guiTextBoxSpeaking.copy(), True, False)

        #========================
    except Exception as e:
        print(traceback.format_exc())
    try:
        #========================
        # Load Unit Data
        udict = {}
        with open(os.path.join(PotkPaths.masterdataPath, PotkPaths.UnitUnitFilename), 'r', encoding="utf8") as file:
            jdata = json.load(file)
        for unit in jdata:
            udict[unit['ID']] = unit
            udict[unit['ID']]['type'] = 'unit'

        uStoryDict = {}
        with open(os.path.join(PotkPaths.masterdataPath, PotkPaths.UnitUnitStoryFilename), 'r', encoding="utf8") as file:
            jdata = json.load(file)
        for unit in jdata:
            uStoryDict[unit['ID']] = unit

        for uid, unit in udict.items():
            if unit['resource_reference_unit_id_UnitUnit'] in uStoryDict:
                udict[unit['ID']]['face_x'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['face_x']
                udict[unit['ID']]['face_y'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['face_y']
                udict[unit['ID']]['story_texture_scale'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_scale']
                udict[unit['ID']]['story_texture_x'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_x']
                udict[unit['ID']]['story_texture_y'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_y']
            else:
                # print('Missing UnitStory data for: ' + str(unit['ID']))
                udict[unit['ID']]['face_x'] = 0
                udict[unit['ID']]['face_y'] = 0
                udict[unit['ID']]['story_texture_scale'] = 1
                udict[unit['ID']]['story_texture_x'] = 0
                udict[unit['ID']]['story_texture_y'] = 0

        for uid, unit in uStoryDict.items():
            if unit['ID'] not in udict:
                udict[unit['ID']] = {}
                udict[unit['ID']]['type'] = 'mob'
                udict[unit['ID']]['resource_reference_unit_id_MobUnit'] = unit['ID']
                udict[unit['ID']]['face_x'] = unit['face_x']
                udict[unit['ID']]['face_y'] = unit['face_y']
                udict[unit['ID']]['story_texture_scale'] = unit['story_texture_scale']
                udict[unit['ID']]['story_texture_x'] = unit['story_texture_x']
                udict[unit['ID']]['story_texture_y'] = unit['story_texture_y']

        #========================
    except Exception as e:
        print(traceback.format_exc())
    try:
        #========================
        # Load Music Data
        musdict = {}
        with open(os.path.join(PotkPaths.masterdataPath, PotkPaths.MusicFilename), 'r', encoding="utf8") as file:
            jdata = json.load(file)
        for song in jdata:
            musdict[song['bgm_name']] = song

        #========================
    except Exception as e:
        print(traceback.format_exc())