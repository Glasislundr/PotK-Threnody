import pygame
import traceback
from lib.conf.conf import conf

pygame.font.init()
pygame.mixer.init()
gameDisplay = pygame.display.set_mode((conf.display_width,conf.display_height))
pygame.display.set_caption('Phantom of the Kill: Threnody')

from lib.story_viewer.ParsedScriptFile import ParsedScriptFile
from lib.story_viewer.ScriptReaderEnv import ScriptReaderEnv
from lib.PotkRes import PotkRes

appIcon = PotkRes.getIcon('Unit_Icon')
if appIcon is not None:
    pygame.display.set_icon(appIcon)

clock = pygame.time.Clock()
crashed = False

#scriptName = '10013201' # old Masamune chara story
#scriptName = '10015301' # old wedding Masamune chara story
#scriptName = '10026303' # old Laev chara story
scriptName = '6070108' # original Ancient Killer story
#scriptName = '100104011' # early LR budoukai
#scriptName = '82019101' #learn with manga
#scriptName = '241051210' #Gakuen April 1 Start

#scriptName = '100205041' #early LR story 第５話　タイトル：笑顔でいられる世界
#scriptName = '100102051' # another early LR 1章2話_5　戦闘前　タイトル：使命
#scriptName = '100102053' # again, 1章2話5 戦闘後
#scriptName = '210152040' #IN, Gran vs rampaging Almace
        #;;【PUNK】インテグラルノア編
        #;;第１５章 章タイトル「クリティカルフェイズ―連携―」
        #;;第2話―４　タイトル：終焉を望む者
        #;;2_4_0

#scriptName = '100101011' # Start of LR, plays movie
#scriptName = '414021051' #Asca/Grim/Pre-Fail LR story, lots of filters, rects
#scriptName = '1540102' #old style without newer textbox prompts
#scriptName = '310010010' # First SEA story
#scriptName = '310010060' # First SEA journal

#scriptName = '2762001' # Konosuba
#scriptName = '2761001' # Konosuba Pre-battle
#scriptName = '2761002' # Konosuba Post-battle

scriptName = '1010101' # to test the memory leak bugfix

try:
    theScript = ParsedScriptFile(scriptName)
    theEnv = ScriptReaderEnv(theScript)
except Exception as e:
    crashed = True
    print(traceback.format_exc())

while not crashed:
    for event in pygame.event.get():
        match event.type:
            case pygame.QUIT:
                crashed = True
            case pygame.MOUSEBUTTONDOWN:
                theEnv.userMouseDown(pygame.mouse.get_pos())
            case pygame.MOUSEBUTTONUP:
                theEnv.userMouseUp(pygame.mouse.get_pos())
    
    theEnv = theEnv.update(gameDisplay)
    pygame.display.update()
    clock.tick(conf.fps)

pygame.quit()
quit()
