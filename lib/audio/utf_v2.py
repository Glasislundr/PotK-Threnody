import os
import json
from lib.audio.utils import *

"""
This code was adapted from CriTools
https://github.com/kohos/CriTools/blob/master/src/utf.js
"""

def findZero(buffer, start):
    while start < len(buffer) and buffer[start] != 0x0:
        #print(str(buffer[start]))
        start+=1
    return start

def parseUtf(buffer, toString):
    if not buffer:
        #print('No buffer passed in: ' + str(buffer))
        return None
    if len(buffer) < 4:
        #print('Length of buffer is only ' + str(len(buffer)))
        return None
    pos = 0
    config = {}
    config['magic'] = buffer[pos:4]
    pos += 4
    if (config['magic'] != b'@UTF'):
        #print('No magic')
        return None
    config['magic'] = config['magic'].decode('utf-8')
    config['dataSize'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    buffer = buffer[pos:]
    pos = 0
    config['version'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    config['valueOffset'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    config['stringOffset'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    config['dataOffset'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    config['nameOffset'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    config['elementCount'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    config['valueSize'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    config['pageCount'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    stringEnd = findZero(buffer, config['stringOffset'])
    config['name'] = buffer[config['stringOffset']:stringEnd].decode('utf-8')
    valuePos = config['valueOffset']
    pages = []
    config['types'] = [None] * config['elementCount']
    firstPos = pos
    #print('Iterating over ' + str(config['pageCount']) + ' "pages" (rows) and ' + str(config['elementCount']) + ' "elements" (columns)')
    for i in range(config['pageCount']):
        page = {}
        pos = firstPos
        for j in range(config['elementCount']):
            ctype = get_u8(buffer[pos:pos+1])
            #print('Got ctype of ' + str(ctype))
            pos = pos + 1
            if (i == 0):
                config['types'][j] = ctype
            stringOffset = config['stringOffset'] + get_u32be(buffer[pos:pos+4])
            pos += 4
            stringEnd = findZero(buffer, stringOffset)
            key = buffer[stringOffset:stringEnd].decode('utf-8')
            method = ctype >> 5
            ctype = ctype & 0x1F
            value = None
            if (method > 0):
                offset = pos if method == 1 else valuePos
                match (ctype):
                    case 0x10:
                        value = get_s8(buffer[offset:offset+1])
                        offset += 1
                    case 0x11:
                        value = get_u8(buffer[offset:offset+1])
                        offset += 1
                    case 0x12:
                        value = get_s16be(buffer[offset:offset+2])
                        offset += 2
                    case 0x13:
                        value = get_u16be(buffer[offset:offset+2])
                        offset += 2
                    case 0x14:
                        value = get_s32be(buffer[offset:offset+4])
                        offset += 4
                    case 0x15:
                        value = get_u32be(buffer[offset:offset+4])
                        offset += 4
                    case 0x16:
                        value = get_s64be(buffer[offset:offset+8])
                        offset += 8
                    case 0x17:
                        value = get_u64be(buffer[offset:offset+8])
                        offset += 8
                    case 0x18:
                        value = get_floatbe(buffer[offset:offset+4])
                        offset += 4
                    case 0x19:
                        #debugger
                        value = get_doublebe(buffer[offset:offset+8])
                        offset += 8
                    case 0x1A:
                        stringOffset = config['stringOffset'] + get_u32be(buffer[offset:offset+4])
                        offset += 4
                        stringEnd = findZero(buffer, stringOffset)
                        value = buffer[stringOffset:stringEnd].decode('utf-8')
                    case 0x1B:
                        bufferStart = config['dataOffset'] + get_u32be(buffer[offset:offset+4])
                        offset += 4
                        bufferLen = get_u32be(buffer[offset:offset+4])
                        offset += 4
                        #print('Attempting to enter sub-table? from ' + str(bufferStart) + ' to ' + str(bufferStart + bufferLen))
                        value = buffer[bufferStart:bufferStart + bufferLen]
                        temp = parseUtf(value, toString)
                        if (temp):
                            value = temp
                        elif (toString):
                            value = buffer[bufferStart:bufferStart + bufferLen].hex()
                        else:
                            value = str(value)
                    case _:
                        print('unknown type: ' + str(ctype))
                if (method == 1):
                    pos = offset
                else:
                    valuePos = offset
            #if type(value) is bytes:
                #print('!!!!!!!!! Bytes got through with ctype: ' + str(ctype))
            page[key] = value
        pages+=[page]
    #output = {}
    #output['config'] = config
    #output['pages'] = pages
    return pages


def viewUtf(acbPath, outputPath):
    if (not outputPath):
        pathInfo = os.path.splitext(acbPath)    
        outputPath = pathInfo[0] + '.json'
    print('Parsing '+acbPath+'...')
    with open(acbPath, 'br') as buffer:
        obj = parseUtf(buffer.read(), True)
        #if (obj and obj['AwbFile'] and len(obj['AwbFile']) > 0x20):
        #    obj['AwbFile'] = obj['AwbFile'].substring(0, 0x20)
        print('Writing '+outputPath+'...')
        with open(outputPath,'w') as outputFile:
            outputFile.write(json.dumps(obj, indent=2))
