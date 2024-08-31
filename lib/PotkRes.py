import os
import json
import pygame
import traceback

from lib.PotkPaths import PotkPaths
import lib.MasterData as MasterData

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
            if os.path.exists(PotkPaths.getIconArtPath(name)):
                img = pygame.image.load(PotkPaths.getIconArtPath(name))
                cls.loadedIcons[name] = img
                return img
            else:
                return None
    
    @classmethod
    def getGameImage(cls, path):
        if path in cls.loadedImages:
            return cls.loadedImages[path]
        else:
            if os.path.exists(path):
                img = pygame.image.load(path)
                cls.loadedImages[path] = img
                return img
            else:
                return None
    
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
        
    @classmethod
    def preloadStandardStoryGuiElements(cls):
        try:
            import lib.AssetBundle as AssetBundle
            #========================
            # Load GUI elements
            storyGui = AssetBundle.getGUI('009-3_sozai')
            cls.guiDiagBorder = storyGui.getSubImg('slc_Frame_Btm', (261,720))
            cls.guiDiagBorderBot = pygame.transform.rotate(cls.guiDiagBorder.copy(), 90)
            cls.guiDiagBorderTop = pygame.transform.rotate(cls.guiDiagBorder, -90)

            cls.guiQuestionTextBox = storyGui.getSubImg('slc_Balloon_Question')
            cls.guiQuestionAnswerButton = storyGui.getSubImg('ibtn_Choices_idle', (562, 90))#pygame.transform.smoothscale(storyGui.getSubImg('ibtn_Choices_idle', (562, 90)), (624, 100))
            cls.guiQuestionAnswerButtonClicked = storyGui.getSubImg('ibtn_Choices_pressed', (562, 90))#pygame.transform.smoothscale(storyGui.getSubImg('ibtn_Choices_pressed', (562, 90)), (624, 100))
            cls.guiQuestionQuestionText = storyGui.getSubImg('slc_Titlesub_Question')
            cls.guiQuestionArrowDark = storyGui.getSubImg('slc_Pinkline2')
            cls.guiQuestionArrowLight = storyGui.getSubImg('slc_Pinkline1')

            cls.guiTextBoxBackgroundR = pygame.transform.flip(storyGui.getSubImg('slc_Balloonbase_Btm_N', (608, 152)), True, True)
            cls.guiTextBoxBackgroundG = storyGui.getSubImg('slc_Balloonbase_Protagonist_Btm_N', (608, 152))
            cls.guiTextBoxNameP = storyGui.getSubImg('slc_Balloon_Name_Base', (238, 30))
            cls.guiTextBoxNameG = storyGui.getSubImg('slc_Balloon_Name_Base_Protagonist', (238, 30))
            cls.guiTextBoxStandardFrameY = storyGui.getSubImg('slc_balloon1_base', (550, 120))
            cls.guiTextBoxStandardFrameB = storyGui.getSubImg('slc_Balloon1_Protagonist_Btm', (550, 121))
            cls.guiTextBoxSpikeFrameO = storyGui.getSubImg('slc_balloon2_base', (550, 166))
            cls.guiTextBoxSpikeFrameY = storyGui.getSubImg('slc_Balloon2_Protagonist_Btm', (550, 166))
            cls.guiTextBoxThoughtFrameP = storyGui.getSubImg('slc_balloon3_base', (562, 149))
            cls.guiTextBoxThoughtFrameG = storyGui.getSubImg('slc_Balloon3_Protagonist_Btm_C', (562, 149))
            cls.guiTextBoxNextArrow = storyGui.getSubImg('slc_Balloon_Arrow')

            cls.guiTextBoxSpeaking = storyGui.getSubImg('slc_balloon_base_nozzle')
            cls.guiTextBoxSpeakingFlip = pygame.transform.flip(cls.guiTextBoxSpeaking.copy(), True, False)
            
            #========================
        except Exception as e:
            print(traceback.format_exc())

    @classmethod
    def preloadUnitData(cls):
        try:
            #========================
            # Load Unit Data
            if hasattr(cls, 'udict'):
                return
            cls.udict = {}
            jdata = MasterData.getMasterData('UnitUnit')
            for unit in jdata:
                cls.udict[unit['ID']] = unit
                cls.udict[unit['ID']]['type'] = 'unit'

            uStoryDict = {}
            jdata = MasterData.getMasterData('UnitUnitStory')
            for unit in jdata:
                uStoryDict[unit['ID']] = unit

            for uid, unit in cls.udict.items():
                if unit['resource_reference_unit_id_UnitUnit'] in uStoryDict:
                    cls.udict[unit['ID']]['face_x'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['face_x']
                    cls.udict[unit['ID']]['face_y'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['face_y']
                    cls.udict[unit['ID']]['story_texture_scale'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_scale']
                    cls.udict[unit['ID']]['story_texture_x'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_x']
                    cls.udict[unit['ID']]['story_texture_y'] = uStoryDict[unit['resource_reference_unit_id_UnitUnit']]['story_texture_y']
                else:
                    # print('Missing UnitStory data for: ' + str(unit['ID']))
                    cls.udict[unit['ID']]['face_x'] = 0
                    cls.udict[unit['ID']]['face_y'] = 0
                    cls.udict[unit['ID']]['story_texture_scale'] = 1
                    cls.udict[unit['ID']]['story_texture_x'] = 0
                    cls.udict[unit['ID']]['story_texture_y'] = 0

            for uid, unit in uStoryDict.items():
                if unit['ID'] not in cls.udict:
                    cls.udict[unit['ID']] = {}
                    cls.udict[unit['ID']]['type'] = 'mob'
                    cls.udict[unit['ID']]['resource_reference_unit_id_MobUnit'] = unit['ID']
                    cls.udict[unit['ID']]['face_x'] = unit['face_x']
                    cls.udict[unit['ID']]['face_y'] = unit['face_y']
                    cls.udict[unit['ID']]['story_texture_scale'] = unit['story_texture_scale']
                    cls.udict[unit['ID']]['story_texture_x'] = unit['story_texture_x']
                    cls.udict[unit['ID']]['story_texture_y'] = unit['story_texture_y']

            #========================
        except Exception as e:
            print(traceback.format_exc())

    @classmethod
    def preloadJukeboxData(cls):
        try:
            #========================
            # Load Music Data
            cls.musdict = {}
            jdata = MasterData.getMasterData('Music')
            for song in jdata:
                cls.musdict[song['bgm_name']] = song

            #========================
        except Exception as e:
            print(traceback.format_exc())