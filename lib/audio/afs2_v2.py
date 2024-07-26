import os

import lib.audio.hca_v2 as hca
from lib.audio.utils import *

"""
This is adapted from the afs2.js file from CriTools
https://github.com/kohos/CriTools/blob/master/src/afs2.js
"""

def parseAFS2(buffer):
    if type(buffer) is str:
        with open(buffer, 'rb') as f:
            buffer = f.read()
    if (not buffer or len(buffer) < 4):
        return None
    pos = 0
    config = {}
    config['buffer'] = buffer
    config['magic'] = buffer[pos:4]
    pos += 4
    if (config['magic'] != b'AFS2'):
        return None
    config['magic'] = config['magic'].decode('utf-8')
    config['unknown1'] = get_u8(buffer[pos:pos+1])
    pos += 1
    config['sizeLen'] = get_u8(buffer[pos:pos+1])
    pos += 1
    config['unknown2'] = get_u8(buffer[pos:pos+1])
    pos += 1
    config['unknown3'] = get_u8(buffer[pos:pos+1])
    pos += 1
    config['fileCount'] = get_u32le(buffer[pos:pos+4])
    pos += 4
    config['align'] = get_u16le(buffer[pos:pos+2])
    pos += 2
    config['key'] = get_u16le(buffer[pos:pos+2])
    pos += 2
    config['fileIds'] = []
    for i in range(config['fileCount']):
        fileId = get_u16le(buffer[pos:pos+2])
        pos += 2
        config['fileIds']+=[fileId]
    
    files = []
    start = 0
    if (config['sizeLen'] == 2):
        start = get_u16le(buffer[pos:pos+2])
        pos += 2
    elif (config['sizeLen'] == 4):
        start = get_u32le(buffer[pos:pos+4])
        pos += 4
    else:
        print('Unknown Size')
    mod = start % config['align']
    if (mod != 0):
        start += config['align'] - mod
    for i in range(config['fileCount']):
        end = 0
        if (config['sizeLen'] == 2):
            end = get_u16le(buffer[pos:pos+2])
            pos += 2
        elif (config['sizeLen'] == 4):
            end = get_u32le(buffer[pos:pos+4])
            pos += 4
        else:
            print('Something went wrong')
        files += [buffer[start:end]]
        start = end
        mod = start % config['align']
        if (mod != 0):
            start += config['align'] - mod
    
    return files, config


def awb2hcas(awbPath, key, hcaDir, fType, skip):
    pathInfo = path.parse(awbPath)
    print(f'Parsing {pathInfo.base}...')
    afsList, config = parseAFS2(awbPath)
    if (hcaDir is None):
        hcaDir = path.join(pathInfo.dir, pathInfo.name)
    if (not os.path.exists(hcaDir)):
        os.mkdirs(hcaDir)
    elif (skip):
        print(f'Skipped {pathInfo.base}...')
        return
    
    strlen = len(str(len(afsList)))
    print(f'Extracting {pathInfo.base}...')
    for i in range(len(afsList)):
        hcaBuff = afsList[i]
        name = str(i + 1)
        while (len(name) < strlen):
            name = '0' + name
        if (key is not None):
            print(f'Decrypting {name}.hca...')
            hca.decryptHca(hcaBuff, key, config['key'], fType)
        
        print(f'Writing {name}.hca...')
        with open(path.join(hcaDir, name + '.hca'), 'wb') as f:
            f.write(hcaBuff)
    



def awb2wavs(awbPath, key, wavDir, volume, mode, skip):
    dirname, basename = os.path.split(awbPath)
    filename, ext = os.path.splitext(basename)
    print(f'Parsing {basename}...')
    afsList, config = parseAFS2(awbPath)
    if (wavDir is None):
        wavDir = path.join(dirname, filename)
    if (not os.path.exists(wavDir)):
        os.mkdirs(wavDir)
    elif (skip):
        print(f'Skipped {basename}...')
        return
    
    strlen = len(str(len(afsList)))
    print(f'Extracting {basename}...')
    for i in range(len(afsList)):
        hcaBuff = afsList[i]
        name = str(i + 1)
        while (len(name) < strlen):
            name = '0' + name
        wavPath = path.join(wavDir, name + '.wav')
        hca.decodeHcaToWav(hcaBuff, key, config['key'], wavPath, volume, mode)
    



def decryptAwb(awbPath, key, fType):
    basename = os.path.basename(awbPath)
    print(f'Parsing {basename}...')
    afsList, config = parseAFS2(awbPath)
    print(f'Decrypting {basename}...')
    for i in range(len(afsList)):
        hca.decryptHca(afsList[i], key, config['key'], fType)
    buffer = config['buffer']
    buffer[0xE:0xE + 0x2] = (0).to_bytes(2,'big')
    print(f'Writing {basename}...')
    with open(awbPath, 'wb') as f:
        f.write(buffer)
