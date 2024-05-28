import os
import math
from lib.audio.utils import *

"""
This is adapted from the hca.js file from CriTools
https://github.com/kohos/CriTools/blob/master/src/hca.js
"""

# DECRYPT START
def initAthTable(table, tType, key):
    if (tType == 0):
        for i in range(0x80):
            table[i] = 0
        return True
    elif (tType == 1):
        the_list = [
          0x78, 0x5F, 0x56, 0x51, 0x4E, 0x4C, 0x4B, 0x49, 0x48, 0x48, 0x47, 0x46, 0x46, 0x45, 0x45, 0x45,
          0x44, 0x44, 0x44, 0x44, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42,
          0x42, 0x42, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x40, 0x40, 0x40, 0x40,
          0x40, 0x40, 0x40, 0x40, 0x40, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,
          0x3F, 0x3F, 0x3F, 0x3E, 0x3E, 0x3E, 0x3E, 0x3E, 0x3E, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D,
          0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B,
          0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B,
          0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3B, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C, 0x3C,
          0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3D, 0x3E, 0x3E, 0x3E, 0x3E, 0x3E, 0x3E, 0x3E, 0x3F,
          0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F, 0x3F,
          0x3F, 0x3F, 0x3F, 0x3F, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40,
          0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41,
          0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41,
          0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x41, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42,
          0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x42, 0x43, 0x43, 0x43,
          0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x43, 0x44, 0x44,
          0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x44, 0x45, 0x45, 0x45, 0x45,
          0x45, 0x45, 0x45, 0x45, 0x45, 0x45, 0x45, 0x45, 0x46, 0x46, 0x46, 0x46, 0x46, 0x46, 0x46, 0x46,
          0x46, 0x46, 0x47, 0x47, 0x47, 0x47, 0x47, 0x47, 0x47, 0x47, 0x47, 0x47, 0x48, 0x48, 0x48, 0x48,
          0x48, 0x48, 0x48, 0x48, 0x49, 0x49, 0x49, 0x49, 0x49, 0x49, 0x49, 0x49, 0x4A, 0x4A, 0x4A, 0x4A,
          0x4A, 0x4A, 0x4A, 0x4A, 0x4B, 0x4B, 0x4B, 0x4B, 0x4B, 0x4B, 0x4B, 0x4C, 0x4C, 0x4C, 0x4C, 0x4C,
          0x4C, 0x4D, 0x4D, 0x4D, 0x4D, 0x4D, 0x4D, 0x4E, 0x4E, 0x4E, 0x4E, 0x4E, 0x4E, 0x4F, 0x4F, 0x4F,
          0x4F, 0x4F, 0x4F, 0x50, 0x50, 0x50, 0x50, 0x50, 0x51, 0x51, 0x51, 0x51, 0x51, 0x52, 0x52, 0x52,
          0x52, 0x52, 0x53, 0x53, 0x53, 0x53, 0x54, 0x54, 0x54, 0x54, 0x54, 0x55, 0x55, 0x55, 0x55, 0x56,
          0x56, 0x56, 0x56, 0x57, 0x57, 0x57, 0x57, 0x57, 0x58, 0x58, 0x58, 0x59, 0x59, 0x59, 0x59, 0x5A,
          0x5A, 0x5A, 0x5A, 0x5B, 0x5B, 0x5B, 0x5B, 0x5C, 0x5C, 0x5C, 0x5D, 0x5D, 0x5D, 0x5D, 0x5E, 0x5E,
          0x5E, 0x5F, 0x5F, 0x5F, 0x60, 0x60, 0x60, 0x61, 0x61, 0x61, 0x61, 0x62, 0x62, 0x62, 0x63, 0x63,
          0x63, 0x64, 0x64, 0x64, 0x65, 0x65, 0x66, 0x66, 0x66, 0x67, 0x67, 0x67, 0x68, 0x68, 0x68, 0x69,
          0x69, 0x6A, 0x6A, 0x6A, 0x6B, 0x6B, 0x6B, 0x6C, 0x6C, 0x6D, 0x6D, 0x6D, 0x6E, 0x6E, 0x6F, 0x6F,
          0x70, 0x70, 0x70, 0x71, 0x71, 0x72, 0x72, 0x73, 0x73, 0x73, 0x74, 0x74, 0x75, 0x75, 0x76, 0x76,
          0x77, 0x77, 0x78, 0x78, 0x78, 0x79, 0x79, 0x7A, 0x7A, 0x7B, 0x7B, 0x7C, 0x7C, 0x7D, 0x7D, 0x7E,
          0x7E, 0x7F, 0x7F, 0x80, 0x80, 0x81, 0x81, 0x82, 0x83, 0x83, 0x84, 0x84, 0x85, 0x85, 0x86, 0x86,
          0x87, 0x88, 0x88, 0x89, 0x89, 0x8A, 0x8A, 0x8B, 0x8C, 0x8C, 0x8D, 0x8D, 0x8E, 0x8F, 0x8F, 0x90,
          0x90, 0x91, 0x92, 0x92, 0x93, 0x94, 0x94, 0x95, 0x95, 0x96, 0x97, 0x97, 0x98, 0x99, 0x99, 0x9A,
          0x9B, 0x9B, 0x9C, 0x9D, 0x9D, 0x9E, 0x9F, 0xA0, 0xA0, 0xA1, 0xA2, 0xA2, 0xA3, 0xA4, 0xA5, 0xA5,
          0xA6, 0xA7, 0xA7, 0xA8, 0xA9, 0xAA, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAE, 0xAF, 0xB0, 0xB1, 0xB1,
          0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xBA, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF,
          0xC0, 0xC1, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD,
          0xCE, 0xCF, 0xD0, 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB, 0xDC, 0xDD,
          0xDE, 0xDF, 0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xED, 0xEE,
          0xEF, 0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFF, 0xFF
        ]
        v = 0
        for i in range(0x80):
            index = v >> 13
            if (index >= 0x28E):
                last = 0x80 - i
                for j in range(last):
                  table[i + j] = 0xFF
                break
            table[i] = the_list[index]
            v += key
        return True
    return False

def createTable56(r, key):
    mul = ((key & 1) << 3) | 5
    add = (key & 0xE) | 1
    key >>= 4
    for i in range(0x10):
        key = (key * mul + add) & 0xF
        r[i] = key

def initCiphTable(table, tType, key1, key2):
    if (tType == 0):
        for i in range(0x100):
            table[i] = i
        return True
    elif (tType == 1):
        v = 0
        for i in range(1, 0xFF):
            v = (v * 13 + 11) & 0xFF
            if (v == 0 or v == 0xFF):
                v = (v * 13 + 11) & 0xFF
            table[i] = v
        table[0] = 0
        table[0xFF] = 0xFF
        return True
    elif (tType == 56):
        t1 = bytearray(8)
        if (not key1):
            key2-=1
        key1-=1
        for i in range(7):
            t1[i] = key1 & 0xFF
            key1 = (key1 >> 8) | ((key2 << 24) & 0xFFFFFFFF)
            key2 >>= 8
        t2 = bytearray([
          t1[1], t1[1] ^ t1[6],
          t1[2] ^ t1[3], t1[2],
          t1[2] ^ t1[1], t1[3] ^ t1[4],
          t1[3], t1[3] ^ t1[2],
          t1[4] ^ t1[5], t1[4],
          t1[4] ^ t1[3], t1[5] ^ t1[6],
          t1[5], t1[5] ^ t1[4],
          t1[6] ^ t1[1], t1[6]
        ])
        t3 = bytearray(0x100)
        t31 = bytearray(0x10)
        t32 = bytearray(0x10)
        createTable56(t31, t1[0])
        k = 0
        for i in range(0x10):
            createTable56(t32, t2[i])
            v = (t31[i] << 4) & 0xFF
            for j in range(0x10):
                t3[k] = v | t32[j]
                k+=1
        j = 1
        v = 0
        for i in range(0x100):
            v = (v + 0x11) & 0xFF
            a = t3[v]
            if (a != 0 and a != 0xFF):
                table[j] = a
                j+=1
        table[0] = 0
        table[0xFF] = 0xFF
        return True
    return False

def decryptBlock(table, block):
    for i in range(len(block)):
        block[i] = table[block[i]]

def checkSum(data, size):
    the_sum = 0
    v = [
        0x0000, 0x8005, 0x800F, 0x000A, 0x801B, 0x001E, 0x0014, 0x8011, 0x8033, 0x0036, 0x003C, 0x8039, 0x0028, 0x802D, 0x8027, 0x0022,
        0x8063, 0x0066, 0x006C, 0x8069, 0x0078, 0x807D, 0x8077, 0x0072, 0x0050, 0x8055, 0x805F, 0x005A, 0x804B, 0x004E, 0x0044, 0x8041,
        0x80C3, 0x00C6, 0x00CC, 0x80C9, 0x00D8, 0x80DD, 0x80D7, 0x00D2, 0x00F0, 0x80F5, 0x80FF, 0x00FA, 0x80EB, 0x00EE, 0x00E4, 0x80E1,
        0x00A0, 0x80A5, 0x80AF, 0x00AA, 0x80BB, 0x00BE, 0x00B4, 0x80B1, 0x8093, 0x0096, 0x009C, 0x8099, 0x0088, 0x808D, 0x8087, 0x0082,
        0x8183, 0x0186, 0x018C, 0x8189, 0x0198, 0x819D, 0x8197, 0x0192, 0x01B0, 0x81B5, 0x81BF, 0x01BA, 0x81AB, 0x01AE, 0x01A4, 0x81A1,
        0x01E0, 0x81E5, 0x81EF, 0x01EA, 0x81FB, 0x01FE, 0x01F4, 0x81F1, 0x81D3, 0x01D6, 0x01DC, 0x81D9, 0x01C8, 0x81CD, 0x81C7, 0x01C2,
        0x0140, 0x8145, 0x814F, 0x014A, 0x815B, 0x015E, 0x0154, 0x8151, 0x8173, 0x0176, 0x017C, 0x8179, 0x0168, 0x816D, 0x8167, 0x0162,
        0x8123, 0x0126, 0x012C, 0x8129, 0x0138, 0x813D, 0x8137, 0x0132, 0x0110, 0x8115, 0x811F, 0x011A, 0x810B, 0x010E, 0x0104, 0x8101,
        0x8303, 0x0306, 0x030C, 0x8309, 0x0318, 0x831D, 0x8317, 0x0312, 0x0330, 0x8335, 0x833F, 0x033A, 0x832B, 0x032E, 0x0324, 0x8321,
        0x0360, 0x8365, 0x836F, 0x036A, 0x837B, 0x037E, 0x0374, 0x8371, 0x8353, 0x0356, 0x035C, 0x8359, 0x0348, 0x834D, 0x8347, 0x0342,
        0x03C0, 0x83C5, 0x83CF, 0x03CA, 0x83DB, 0x03DE, 0x03D4, 0x83D1, 0x83F3, 0x03F6, 0x03FC, 0x83F9, 0x03E8, 0x83ED, 0x83E7, 0x03E2,
        0x83A3, 0x03A6, 0x03AC, 0x83A9, 0x03B8, 0x83BD, 0x83B7, 0x03B2, 0x0390, 0x8395, 0x839F, 0x039A, 0x838B, 0x038E, 0x0384, 0x8381,
        0x0280, 0x8285, 0x828F, 0x028A, 0x829B, 0x029E, 0x0294, 0x8291, 0x82B3, 0x02B6, 0x02BC, 0x82B9, 0x02A8, 0x82AD, 0x82A7, 0x02A2,
        0x82E3, 0x02E6, 0x02EC, 0x82E9, 0x02F8, 0x82FD, 0x82F7, 0x02F2, 0x02D0, 0x82D5, 0x82DF, 0x02DA, 0x82CB, 0x02CE, 0x02C4, 0x82C1,
        0x8243, 0x0246, 0x024C, 0x8249, 0x0258, 0x825D, 0x8257, 0x0252, 0x0270, 0x8275, 0x827F, 0x027A, 0x826B, 0x026E, 0x0264, 0x8261,
        0x0220, 0x8225, 0x822F, 0x022A, 0x823B, 0x023E, 0x0234, 0x8231, 0x8213, 0x0216, 0x021C, 0x8219, 0x0208, 0x820D, 0x8207, 0x0202,
    ]
    for i in range(size):
        the_sum = ((the_sum << 8) & 0xFFFF) ^ v[(the_sum >> 8) ^ data[i]]
    return the_sum

def parseHCA(buffer, key, awbKey):
    if (awbKey is None):
        awbKey = 0
    if (not buffer or len(buffer) < 4):
        return None
    pos = 0
    hca = {}
    # HCA
    hca['magic'] = get_u32le(buffer[pos:pos+4])
    pos += 4
    if ((hca['magic'] & 0x7F7F7F7F) != 0x00414348):
        return None
    hca['version'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    hca['dataOffset'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    # fmt
    hca['fmt'] = get_u32le(buffer[pos:pos+4])
    pos += 4
    if ((hca['fmt'] & 0x7F7F7F7F) != 0x00746D66):
        return None
    hca['channelCount'] = get_u8(buffer[pos:pos+1])
    hca['samplingRate'] = 0xFFFFFF & get_u32be(buffer[pos:pos+4])
    pos += 4
    hca['blockCount'] = get_u32be(buffer[pos:pos+4])
    pos += 4
    hca['muteHeader'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    hca['muteFooter'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    if (not (hca['channelCount'] >= 1 and hca['channelCount'] <= 16)):
        return None
    if (not (hca['samplingRate'] >= 1 and hca['samplingRate'] <= 0x7FFFFF)):
        return None
    label = get_u32le(buffer[pos:pos+4])
    pos += 4
    hca['compdec'] = label
    hca['blockSize'] = get_u16be(buffer[pos:pos+2])
    pos += 2
    hca['r01'] = get_u8(buffer[pos:pos+1])
    pos += 1
    hca['r02'] = get_u8(buffer[pos:pos+1])
    pos += 1
    if ((label & 0x7F7F7F7F) == 0x706D6F63): # comp
        hca['r03'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r04'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r05'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r06'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r07'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r08'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['reserve1'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['reserve2'] = get_u8(buffer[pos:pos+1])
        pos += 1
    elif ((label & 0x7F7F7F7F) == 0x00636564): # dec
        hca['count1'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['count2'] = get_u8(buffer[pos:pos+1])
        pos += 1
        hca['r03'] = (get_u8(buffer[pos:pos+1]) >> 4) & 0xF
        hca['r04'] = get_u8(buffer[pos:pos+1]) & 0xF
        pos += 1
        hca['enableCount2'] = get_u8(buffer[pos:pos+1])
        pos += 1
    else:
        return None
    if (not ((hca['blockSize'] >= 1 and hca['blockSize'] <= 0xFFFF) or (hca['blockSize'] == 0))):
        return None
    if (not (hca['r01'] >= 0 and hca['r01'] <= hca['r02'] and hca['r02'] <= 0x1F)):
        return None
    label = get_u32le(buffer[pos:pos+4])
    pos += 4
    if ((label & 0x7F7F7F7F) == 0x00726276): # vbr
        hca['vbr'] = label
        hca['vbrPos'] = pos - 4
        hca['vbrR1'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        hca['vbrR2'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        if (not (hca['blockSize'] == 0 and hca['vbrR1'] >= 0 and hca['vbrR2'] <= 0x1FF)):
            return None
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    if ((label & 0x7F7F7F7F) == 0x00687461): # ath
        hca['ath'] = label
        hca['athPos'] = pos - 4
        hca['athType'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    else:
        hca['athType'] = 1 if hca['version'] < 0x200 else 0
    if ((label & 0x7F7F7F7F) == 0x706F6F6C): # loop
        hca['loop'] = label
        hca['loopPos'] = pos - 4
        hca['loopStart'] = get_u32be(buffer[pos:pos+4])
        pos += 4
        hca['loopEnd'] = get_u32be(buffer[pos:pos+4])
        pos += 4
        hca['loopCount'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        if (not (hca['loopStart'] >= 0 and hca['loopStart'] <= hca['loopEnd'] and hca['loopEnd'] <= hca['blockCount'])):
            return None
        hca['loopR1'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    if ((label & 0x7F7F7F7F) == 0x68706963): # ciph
        hca['ciph'] = label
        hca['ciphPos'] = pos - 4
        hca['ciphType'] = get_u16be(buffer[pos:pos+2])
        pos += 2
        if (not (hca['ciphType'] == 0 or hca['ciphType'] == 1 or hca['ciphType'] == 56)):
            return None
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    if ((label & 0x7F7F7F7F) == 0x00617672): # rva
        hca['rva'] = label
        hca['rvaPos'] = pos - 4
        hca['volume'] = get_floatbe(buffer[pos:pos+4])
        pos += 4
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    else:
        hca['volume'] = 1
    if ((label & 0x7F7F7F7F) == 0x6D6D6F63): # comm
        hca['comm'] = label
        hca['commPos'] = pos - 4
        hca['commLen'] = get_u8(buffer[pos:pos+1])
        pos += 1
        if (hca['commLen']):
          hca['comment'] = buffer[pos, pos + hca['commLen']].decode('utf-8')
          pos += hca['commLen']

        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    if ((label & 0x7F7F7F7F) == 0x00646170): # pad
        hca['pad'] = label
        hca['padPos'] = pos - 4
        label = get_u32le(buffer[pos:pos+4])
        pos += 4
    hca['athTable'] = bytearray(0x80)
    if (not initAthTable(hca['athTable'], hca['athType'], hca['samplingRate'])):
        return None
    key1 = 0
    key2 = 0
    if (key):
        key = int(key)
        if (awbKey):
            key = (int(key) * ((int(awbKey) << 16) | int(((~awbKey & 0xFFFF) + 2) & 0xFFFF))) & 0xFFFFFFFFFFFFFFFF
        key1 = int(key & 0xFFFFFFFF)
        key2 = int((key >> 32) & 0xFFFFFFFF)
    hca['ciphTable'] = bytearray(0x100)
    if (not initCiphTable(hca['ciphTable'], hca['ciphType'], key1, key2)):
        return None
    return hca

def decryptHca(buffer, key, awbKey, hType, hcaPath):
    if (type(buffer) is str):
        print(f'Decrypting {os.path.parse(buffer).base}...')
        with open(buffer, 'rb') as f:
            buffer = f.read()
    if (type(hType) is str):
        hcaPath = hType
        hType = 1
    elif (hType is None):
        hType = 1
    hca = parseHCA(buffer, key, awbKey)
    if (not hca):
        raise RuntimeError('Not a valid HCA file')
    buffer[0:4] = (hca['magic'] & 0x7F7F7F7F).to_bytes(4,'little')
    buffer[8:12] = (hca['fmt'] & 0x7F7F7F7F).to_bytes(4,'little')
    buffer[24:28] = (hca['compdec'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['vbr']):
        buffer[hca['vbrPos']:hca['vbrPos']+4] = (hca['vbr'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['ath']):
        buffer[hca['athPos']:hca['athPos']+4] = (hca['ath'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['loop']):
        buffer[hca['loopPos']:hca['loopPos']+4] = (hca['loop'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['ciph']):
        buffer[hca['ciphPos']:hca['ciphPos']+4] = (hca['ciph'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['rva']):
        buffer[hca['rvaPos']:hca['rvaPos']+4] = (hca['rva'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['comm']):
        buffer[hca['commPos']:hca['commPos']+4] = (hca['comm'] & 0x7F7F7F7F).to_bytes(4,'little')
    if (hca['pad']):
        buffer[hca['padPos']:hca['padPos']+4] = (hca['pad'] & 0x7F7F7F7F).to_bytes(4,'little')
    buffer[hca['ciphPos'] + 4:hca['ciphPos'] + 6] = (hType).to_bytes(2,'big')
    buffer[hca['dataOffset'] - 2:hca['dataOffset']] = (checkSum(buffer, hca['dataOffset'] - 2)).to_bytes(2,'big')
    if (hca['ciphType'] != hType):
        ciphTable = bytearray(0x100)
        revTable = bytearray(0x100)
        initCiphTable(ciphTable, 1)
        for i in range(len(revTable)):
            revTable[ciphTable[i]] = i.to_bytes(1)
        offset = hca['dataOffset']
        for i in range(hca['blockCount']):
            if (offset >= len(buffer)):
                break
            block = buffer[offset, offset + hca['blockSize']]
            decryptBlock(hca['ciphTable'], block)
            if (hType == 1):
                decryptBlock(revTable, block)
            block[len(block) - 2:len(block)] = checkSum(block, len(block) - 2)
            buffer[offset, offset + hca['blockSize']] = block
            offset += hca['blockSize']
    if (hcaPath is not None):
        with open(hcaPath, 'wb') as f:
            f.write(buffer)


# DECODE START
def ceil2(a, b):
    return (math.floor(a / b) + (1 if (a % b) else 0)) if (b > 0) else 0

def Float32Array(a):
    return [0.0] * a

def initDecode(hca):
    isComp = (hca['compdec'] & 0x7F7F7F7F) == 0x706D6F63
    hca['comp'] = {}
    hca['comp']['r01'] = hca['r01']
    hca['comp']['r02'] = hca['r02']
    hca['comp']['r03'] = hca['r03']
    hca['comp']['r04'] = hca['r04']
    hca['comp']['r05'] = hca['r05'] if isComp else hca['count1'] + 1
    hca['comp']['r06'] = hca['r06'] if isComp else (hca['count2'] + 1 if hca['enableCount2'] else hca['count1'] + 1)
    hca['comp']['r07'] = hca['r07'] if isComp else r05 - r06
    hca['comp']['r08'] = hca['r08'] if isComp else 0
    if (not hca['comp']['r03']):
        hca['comp']['r03'] = 1
    if (not (hca['comp']['r01'] == 1 and hca['comp']['r02'] == 15)):
        return False
    hca['comp']['r09'] = ceil2(hca['comp']['r05'] - (hca['comp']['r06'] + hca['comp']['r07']), hca['comp']['r08'])
    r = bytearray(0x10)
    b = math.floor(hca['channelCount'] / hca['comp']['r03'])
    if (hca['comp']['r07'] and b > 1):
        c = 0
        for i in range(hca['comp']['r03']):
            match b:
                case 2:
                    r[c] = 1
                    r[c + 1] = 2
                case 3:
                    r[c] = 1
                    r[c + 1] = 2
                case 4:
                    r[c] = 1
                    r[c + 1] = 2
                    if (hca['comp']['r04'] == 0):
                        r[c + 2] = 1
                        r[c + 3] = 2
                case 5:
                    r[c] = 1
                    r[c + 1] = 2
                    if (hca['comp']['r04'] <= 2):
                        r[c + 3] = 1
                        r[c + 4] = 2
                case 6:
                    r[c] = 1
                    r[c + 1] = 2
                    r[c + 4] = 1
                    r[c + 5] = 2
                case 7:
                    r[c] = 1
                    r[c + 1] = 2
                    r[c + 4] = 1
                    r[c + 5] = 2
                case 8:
                    r[c] = 1
                    r[c + 1] = 2
                    r[c + 4] = 1
                    r[c + 5] = 2
                    r[c + 6] = 1
                    r[c + 7] = 2
            c += b
    hca['channels'] = []
    for i in range(hca['channelCount']):
        channel = {}
        channel['block'] = Float32Array(0x80)
        channel['base'] = Float32Array(0x80)
        channel['value'] = bytearray(0x80)
        channel['scale'] = bytearray(0x80)
        channel['value2'] = bytearray(8)
        channel['type'] = r[i]
        channel['value3'] = channel['value'][hca['comp']['r06'] + hca['comp']['r07']:]
        channel['value3Start'] = hca['comp']['r06'] + hca['comp']['r07']
        channel['count'] = hca['comp']['r06'] + (hca['comp']['r07'] if (r[i] != 2) else 0)
        channel['wav1'] = Float32Array(0x80) #new Float32Array(0x80)
        channel['wav2'] = Float32Array(0x80) #new Float32Array(0x80)
        channel['wav3'] = Float32Array(0x80) #new Float32Array(0x80)
        channel['wave'] = [
            Float32Array(0x80), Float32Array(0x80),
            Float32Array(0x80), Float32Array(0x80),
            Float32Array(0x80), Float32Array(0x80),
            Float32Array(0x80), Float32Array(0x80)
        ]
        hca['channels'] += [channel]
    return True

class BlockReader:

    def __init__(self, buffer):
        self.data = buffer
        self.size = len(buffer) * 8 - 16
        self.bit = 0
        self.mask = [0xFFFFFF, 0x7FFFFF, 0x3FFFFF, 0x1FFFFF, 0x0FFFFF, 0x07FFFF, 0x03FFFF, 0x01FFFF]

    def checkBit(self, bitSize):
        v = 0
        if (self.bit + bitSize <= self.size):
            pos = self.bit >> 3
            a = 0
            if (pos + 1) < len(self.data):
                a = self.data[pos + 1]
            b = 0
            if (pos + 2) < len(self.data):
                b = self.data[pos + 2]
            
            v = self.data[pos]
            v = (v << 8) | a
            v = (v << 8) | b
            v &= self.mask[self.bit & 7]
            v >>= 24 - (self.bit & 7) - bitSize
        return v

    def getBit(self, bitSize):
        v = self.checkBit(bitSize)
        self.bit += bitSize
        return v

    def addBit(self, bitSize):
        self.bit += bitSize

def arrayIntToFloat(arrayInt):
    arrayFloat = []
    buffer = bytearray(4)
    for i in range(len(arrayInt)):
        buffer[:] = arrayInt[i].to_bytes(4, 'little')
        arrayFloat += [get_floatle(buffer)]
    return arrayFloat

def fillArray(array, value, start=None, end=None):
    if start is None:
        start = 0
    if end is None:
        end = len(array)
    for i in range(start, end):
        array[i] = value

DECODE1 = {
    'scalelist': [
        0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0E, 0x0D, 0x0D,
        0x0D, 0x0D, 0x0D, 0x0D, 0x0C, 0x0C, 0x0C, 0x0C,
        0x0C, 0x0C, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B,
        0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x09,
        0x09, 0x09, 0x09, 0x09, 0x09, 0x08, 0x08, 0x08,
        0x08, 0x08, 0x08, 0x07, 0x06, 0x06, 0x05, 0x04,
        0x04, 0x04, 0x03, 0x03, 0x03, 0x02, 0x02, 0x02,
        0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    ],
    'valueFloat': arrayIntToFloat([
        0x342A8D26, 0x34633F89, 0x3497657D, 0x34C9B9BE, 0x35066491, 0x353311C4, 0x356E9910, 0x359EF532,
        0x35D3CCF1, 0x360D1ADF, 0x363C034A, 0x367A83B3, 0x36A6E595, 0x36DE60F5, 0x371426FF, 0x3745672A,
        0x37838359, 0x37AF3B79, 0x37E97C38, 0x381B8D3A, 0x384F4319, 0x388A14D5, 0x38B7FBF0, 0x38F5257D,
        0x3923520F, 0x39599D16, 0x3990FA4D, 0x39C12C4D, 0x3A00B1ED, 0x3A2B7A3A, 0x3A647B6D, 0x3A9837F0,
        0x3ACAD226, 0x3B071F62, 0x3B340AAF, 0x3B6FE4BA, 0x3B9FD228, 0x3BD4F35B, 0x3C0DDF04, 0x3C3D08A4,
        0x3C7BDFED, 0x3CA7CD94, 0x3CDF9613, 0x3D14F4F0, 0x3D467991, 0x3D843A29, 0x3DB02F0E, 0x3DEAC0C7,
        0x3E1C6573, 0x3E506334, 0x3E8AD4C6, 0x3EB8FBAF, 0x3EF67A41, 0x3F243516, 0x3F5ACB94, 0x3F91C3D3,
        0x3FC238D2, 0x400164D2, 0x402C6897, 0x4065B907, 0x40990B88, 0x40CBEC15, 0x4107DB35, 0x413504F3,
    ]),
    'scaleFloat': arrayIntToFloat([
        0x00000000, 0x3F2AAAAB, 0x3ECCCCCD, 0x3E924925, 0x3E638E39, 0x3E3A2E8C, 0x3E1D89D9, 0x3E088889,
        0x3D842108, 0x3D020821, 0x3C810204, 0x3C008081, 0x3B804020, 0x3B002008, 0x3A801002, 0x3A000801,
    ])
}
def decode1(channel, reader, a, b, athTable):
    v = reader.getBit(3)
    if (v >= 6):
        for i in range(channel['count']):
            channel['value'][i] = reader.getBit(6)
    elif (v):
        v1 = reader.getBit(6)
        v2 = (1 << v) - 1
        v3 = v2 >> 1
        channel['value'][0] = v1
        for i in range(1,channel['count']):
            v4 = reader.getBit(v)
            #print('v1 pre-mod ' + str(v1))
            if (v4 != v2):
                v1 += v4 - v3
            else:
                v1 = reader.getBit(6)
            #print('v1 ' + str(v1))
            #print('v2 ' + str(v2))
            #print('v3 ' + str(v3))
            #print('v4 ' + str(v4))
            channel['value'][i] = v1 % 256
    else:
        fillArray(channel['value'], 0)

    if (channel['type'] == 2):
        v = reader.checkBit(4)
        channel['value2'][0] = v
        if (v < 15):
            for i in range(8):
                channel['value2'][i] = reader.getBit(4)
    else:
        for i in range(a):
            channel['value3'][i] = reader.getBit(6)
        channel['value'][channel['value3Start']:] = channel['value3']

    for i in range(channel['count']):
        v = channel['value'][i]
        if (v):
            v = athTable[i] + ((b + i) >> 8) - math.floor((v * 5) / 2) + 1
            if (v < 0):
                v = 15
            elif (v >= 0x39):
                v = 1
            else:
                v = DECODE1['scalelist'][v]
        channel['scale'][i] = v
    fillArray(channel['scale'], 0, channel['count'], 0x80)
    #print('CCount ' + str(channel['count']))
    #print('base ' + str(len(channel['base'])))
    #print('value ' + str(len(channel['value'])))
    #print('scale ' + str(len(channel['scale'])))
    
    #print('DVal ' + str(len(DECODE1['valueFloat'])))
    #print('DScl ' + str(len(DECODE1['scaleFloat'])))
    for i in range(channel['count']):
        # if channel['value'][i] > len(DECODE1['valueFloat']):
            # print('VALUE outside the Decode list: ' + str(channel['value'][i]))
        # if channel['scale'][i] > len(DECODE1['scaleFloat']):
            # print('SCALE outside the Decode list: ' + str(channel['scale'][i]))
        if channel['value'][i] >= len(DECODE1['valueFloat']) or channel['scale'][i] >= len(DECODE1['scaleFloat']):
            channel['base'][i] = 0#float("NaN")
        else:
            channel['base'][i] = DECODE1['valueFloat'][channel['value'][i]] * DECODE1['scaleFloat'][channel['scale'][i]]

DECODE2 = {
    'list1': [
        0, 2, 3, 3, 4, 4, 4, 4, 5, 6, 7, 8, 9, 10, 11, 12,
    ],
    'list2': [
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        2, 2, 2, 2, 2, 2, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        2, 2, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4,
        3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
        3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
    ],
    'list3': [
        +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0,
        +0, +0, +1, -1, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0, +0,
        +0, +0, +1, +1, -1, -1, +2, -2, +0, +0, +0, +0, +0, +0, +0, +0,
        +0, +0, +1, -1, +2, -2, +3, -3, +0, +0, +0, +0, +0, +0, +0, +0,
        +0, +0, +1, +1, -1, -1, +2, +2, -2, -2, +3, +3, -3, -3, +4, -4,
        +0, +0, +1, +1, -1, -1, +2, +2, -2, -2, +3, -3, +4, -4, +5, -5,
        +0, +0, +1, +1, -1, -1, +2, -2, +3, -3, +4, -4, +5, -5, +6, -6,
        +0, +0, +1, -1, +2, -2, +3, -3, +4, -4, +5, -5, +6, -6, +7, -7,
    ]
}
def decode2(channel, reader):
    for i in range(channel['count']):
        s = channel['scale'][i]
        bitSize = DECODE2['list1'][s]
        v = reader.getBit(bitSize)
        if (s < 8):
            v += s << 4
            reader.addBit(DECODE2['list2'][v] - bitSize)
            f = DECODE2['list3'][v]
        else:
            v = (1 - ((v & 1) << 1)) * math.floor(v / 2)
            if (not v):
                reader.addBit(-1)
            f = v
        channel['block'][i] = channel['base'][i] * f
    fillArray(channel['block'], 0, channel['count'], 0x80)

DECODE3 = {
    'listFloat': arrayIntToFloat([
        0x00000000, 0x00000000, 0x32A0B051, 0x32D61B5E, 0x330EA43A, 0x333E0F68, 0x337D3E0C, 0x33A8B6D5,
        0x33E0CCDF, 0x3415C3FF, 0x34478D75, 0x3484F1F6, 0x34B123F6, 0x34EC0719, 0x351D3EDA, 0x355184DF,
        0x358B95C2, 0x35B9FCD2, 0x35F7D0DF, 0x36251958, 0x365BFBB8, 0x36928E72, 0x36C346CD, 0x370218AF,
        0x372D583F, 0x3766F85B, 0x3799E046, 0x37CD078C, 0x3808980F, 0x38360094, 0x38728177, 0x38A18FAF,
        0x38D744FD, 0x390F6A81, 0x393F179A, 0x397E9E11, 0x39A9A15B, 0x39E2055B, 0x3A16942D, 0x3A48A2D8,
        0x3A85AAC3, 0x3AB21A32, 0x3AED4F30, 0x3B1E196E, 0x3B52A81E, 0x3B8C57CA, 0x3BBAFF5B, 0x3BF9295A,
        0x3C25FED7, 0x3C5D2D82, 0x3C935A2B, 0x3CC4563F, 0x3D02CD87, 0x3D2E4934, 0x3D68396A, 0x3D9AB62B,
        0x3DCE248C, 0x3E0955EE, 0x3E36FD92, 0x3E73D290, 0x3EA27043, 0x3ED87039, 0x3F1031DC, 0x3F40213B,
        0x3F800000, 0x3FAA8D26, 0x3FE33F89, 0x4017657D, 0x4049B9BE, 0x40866491, 0x40B311C4, 0x40EE9910,
        0x411EF532, 0x4153CCF1, 0x418D1ADF, 0x41BC034A, 0x41FA83B3, 0x4226E595, 0x425E60F5, 0x429426FF,
        0x42C5672A, 0x43038359, 0x432F3B79, 0x43697C38, 0x439B8D3A, 0x43CF4319, 0x440A14D5, 0x4437FBF0,
        0x4475257D, 0x44A3520F, 0x44D99D16, 0x4510FA4D, 0x45412C4D, 0x4580B1ED, 0x45AB7A3A, 0x45E47B6D,
        0x461837F0, 0x464AD226, 0x46871F62, 0x46B40AAF, 0x46EFE4BA, 0x471FD228, 0x4754F35B, 0x478DDF04,
        0x47BD08A4, 0x47FBDFED, 0x4827CD94, 0x485F9613, 0x4894F4F0, 0x48C67991, 0x49043A29, 0x49302F0E,
        0x496AC0C7, 0x499C6573, 0x49D06334, 0x4A0AD4C6, 0x4A38FBAF, 0x4A767A41, 0x4AA43516, 0x4ADACB94,
        0x4B11C3D3, 0x4B4238D2, 0x4B8164D2, 0x4BAC6897, 0x4BE5B907, 0x4C190B88, 0x4C4BEC15, 0x00000000,
    ])
}
def decode3(channel, a, b, c, d):
    channel['value3'] = channel['value'][channel['value3Start']:]
    if (channel['type'] != 2 and b > 0):
        for i in range(a):
            j = 0
            k = c
            l = c - 1
            while j < b and k < d:
                channel['block'][k] = DECODE3['listFloat'][0x40 + channel['value3'][i] - channel['value'][l]] * channel['block'][l]
                k+=1
                j+=1
                l-=1
        channel['block'][0x80 - 1] = 0

DECODE4 = {
    'listFloat': arrayIntToFloat([
        0x40000000, 0x3FEDB6DB, 0x3FDB6DB7, 0x3FC92492, 0x3FB6DB6E, 0x3FA49249, 0x3F924925, 0x3F800000,
        0x3F5B6DB7, 0x3F36DB6E, 0x3F124925, 0x3EDB6DB7, 0x3E924925, 0x3E124925, 0x00000000, 0x00000000,
    ])
}
def decode4(channel, nextChannel, index, a, b, c):
    if (channel['type'] == 1 and c):
        f1 = DECODE4['listFloat'][nextChannel['value2'][index]]
        f2 = f1 - 2.0
        for i in range(a):
            nextChannel['block'][b + i] = channel['block'][b + i] * f2
            channel['block'][b + i] *= f1

DECODE5 = {
    'list1Float': [
        arrayIntToFloat([
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
          0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75, 0x3DA73D75,
        ]),
        arrayIntToFloat([
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
          0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31, 0x3F7B14BE, 0x3F54DB31,
        ]),
        arrayIntToFloat([
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
          0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403, 0x3F7EC46D, 0x3F74FA0B, 0x3F61C598, 0x3F45E403,
        ]),
        arrayIntToFloat([
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
          0x3F7FB10F, 0x3F7D3AAC, 0x3F7853F8, 0x3F710908, 0x3F676BD8, 0x3F5B941A, 0x3F4D9F02, 0x3F3DAEF9,
        ]),
        arrayIntToFloat([
          0x3F7FEC43, 0x3F7F4E6D, 0x3F7E1324, 0x3F7C3B28, 0x3F79C79D, 0x3F76BA07, 0x3F731447, 0x3F6ED89E,
          0x3F6A09A7, 0x3F64AA59, 0x3F5EBE05, 0x3F584853, 0x3F514D3D, 0x3F49D112, 0x3F41D870, 0x3F396842,
          0x3F7FEC43, 0x3F7F4E6D, 0x3F7E1324, 0x3F7C3B28, 0x3F79C79D, 0x3F76BA07, 0x3F731447, 0x3F6ED89E,
          0x3F6A09A7, 0x3F64AA59, 0x3F5EBE05, 0x3F584853, 0x3F514D3D, 0x3F49D112, 0x3F41D870, 0x3F396842,
          0x3F7FEC43, 0x3F7F4E6D, 0x3F7E1324, 0x3F7C3B28, 0x3F79C79D, 0x3F76BA07, 0x3F731447, 0x3F6ED89E,
          0x3F6A09A7, 0x3F64AA59, 0x3F5EBE05, 0x3F584853, 0x3F514D3D, 0x3F49D112, 0x3F41D870, 0x3F396842,
          0x3F7FEC43, 0x3F7F4E6D, 0x3F7E1324, 0x3F7C3B28, 0x3F79C79D, 0x3F76BA07, 0x3F731447, 0x3F6ED89E,
          0x3F6A09A7, 0x3F64AA59, 0x3F5EBE05, 0x3F584853, 0x3F514D3D, 0x3F49D112, 0x3F41D870, 0x3F396842,
        ]),
        arrayIntToFloat([
          0x3F7FFB11, 0x3F7FD397, 0x3F7F84AB, 0x3F7F0E58, 0x3F7E70B0, 0x3F7DABCC, 0x3F7CBFC9, 0x3F7BACCD,
          0x3F7A7302, 0x3F791298, 0x3F778BC5, 0x3F75DEC6, 0x3F740BDD, 0x3F721352, 0x3F6FF573, 0x3F6DB293,
          0x3F6B4B0C, 0x3F68BF3C, 0x3F660F88, 0x3F633C5A, 0x3F604621, 0x3F5D2D53, 0x3F59F26A, 0x3F5695E5,
          0x3F531849, 0x3F4F7A1F, 0x3F4BBBF8, 0x3F47DE65, 0x3F43E200, 0x3F3FC767, 0x3F3B8F3B, 0x3F373A23,
          0x3F7FFB11, 0x3F7FD397, 0x3F7F84AB, 0x3F7F0E58, 0x3F7E70B0, 0x3F7DABCC, 0x3F7CBFC9, 0x3F7BACCD,
          0x3F7A7302, 0x3F791298, 0x3F778BC5, 0x3F75DEC6, 0x3F740BDD, 0x3F721352, 0x3F6FF573, 0x3F6DB293,
          0x3F6B4B0C, 0x3F68BF3C, 0x3F660F88, 0x3F633C5A, 0x3F604621, 0x3F5D2D53, 0x3F59F26A, 0x3F5695E5,
          0x3F531849, 0x3F4F7A1F, 0x3F4BBBF8, 0x3F47DE65, 0x3F43E200, 0x3F3FC767, 0x3F3B8F3B, 0x3F373A23,
        ]),
        arrayIntToFloat([
          0x3F7FFEC4, 0x3F7FF4E6, 0x3F7FE129, 0x3F7FC38F, 0x3F7F9C18, 0x3F7F6AC7, 0x3F7F2F9D, 0x3F7EEA9D,
          0x3F7E9BC9, 0x3F7E4323, 0x3F7DE0B1, 0x3F7D7474, 0x3F7CFE73, 0x3F7C7EB0, 0x3F7BF531, 0x3F7B61FC,
          0x3F7AC516, 0x3F7A1E84, 0x3F796E4E, 0x3F78B47B, 0x3F77F110, 0x3F772417, 0x3F764D97, 0x3F756D97,
          0x3F748422, 0x3F73913F, 0x3F7294F8, 0x3F718F57, 0x3F708066, 0x3F6F6830, 0x3F6E46BE, 0x3F6D1C1D,
          0x3F6BE858, 0x3F6AAB7B, 0x3F696591, 0x3F6816A8, 0x3F66BECC, 0x3F655E0B, 0x3F63F473, 0x3F628210,
          0x3F6106F2, 0x3F5F8327, 0x3F5DF6BE, 0x3F5C61C7, 0x3F5AC450, 0x3F591E6A, 0x3F577026, 0x3F55B993,
          0x3F53FAC3, 0x3F5233C6, 0x3F5064AF, 0x3F4E8D90, 0x3F4CAE79, 0x3F4AC77F, 0x3F48D8B3, 0x3F46E22A,
          0x3F44E3F5, 0x3F42DE29, 0x3F40D0DA, 0x3F3EBC1B, 0x3F3CA003, 0x3F3A7CA4, 0x3F385216, 0x3F36206C,
        ])
    ],
    'list2Float': [
        arrayIntToFloat([
          0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4,
          0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4,
          0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4,
          0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4,
          0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4,
          0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4,
          0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4,
          0x3D0A8BD4, 0xBD0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4, 0x3D0A8BD4, 0x3D0A8BD4, 0xBD0A8BD4,
        ]),
        arrayIntToFloat([
          0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA,
          0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA,
          0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA,
          0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA,
          0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA,
          0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA,
          0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA,
          0x3E47C5C2, 0x3F0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0xBE47C5C2, 0xBF0E39DA, 0x3E47C5C2, 0x3F0E39DA,
        ]),
        arrayIntToFloat([
          0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799, 0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799,
          0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799, 0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799,
          0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799, 0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799,
          0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799, 0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799,
          0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799, 0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799,
          0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799, 0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799,
          0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799, 0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799,
          0x3DC8BD36, 0x3E94A031, 0x3EF15AEA, 0x3F226799, 0xBDC8BD36, 0xBE94A031, 0xBEF15AEA, 0xBF226799,
        ]),
        arrayIntToFloat([
          0xBD48FB30, 0xBE164083, 0xBE78CFCC, 0xBEAC7CD4, 0xBEDAE880, 0xBF039C3D, 0xBF187FC0, 0xBF2BEB4A,
          0x3D48FB30, 0x3E164083, 0x3E78CFCC, 0x3EAC7CD4, 0x3EDAE880, 0x3F039C3D, 0x3F187FC0, 0x3F2BEB4A,
          0x3D48FB30, 0x3E164083, 0x3E78CFCC, 0x3EAC7CD4, 0x3EDAE880, 0x3F039C3D, 0x3F187FC0, 0x3F2BEB4A,
          0xBD48FB30, 0xBE164083, 0xBE78CFCC, 0xBEAC7CD4, 0xBEDAE880, 0xBF039C3D, 0xBF187FC0, 0xBF2BEB4A,
          0x3D48FB30, 0x3E164083, 0x3E78CFCC, 0x3EAC7CD4, 0x3EDAE880, 0x3F039C3D, 0x3F187FC0, 0x3F2BEB4A,
          0xBD48FB30, 0xBE164083, 0xBE78CFCC, 0xBEAC7CD4, 0xBEDAE880, 0xBF039C3D, 0xBF187FC0, 0xBF2BEB4A,
          0xBD48FB30, 0xBE164083, 0xBE78CFCC, 0xBEAC7CD4, 0xBEDAE880, 0xBF039C3D, 0xBF187FC0, 0xBF2BEB4A,
          0x3D48FB30, 0x3E164083, 0x3E78CFCC, 0x3EAC7CD4, 0x3EDAE880, 0x3F039C3D, 0x3F187FC0, 0x3F2BEB4A,
        ]),
        arrayIntToFloat([
          0xBCC90AB0, 0xBD96A905, 0xBDFAB273, 0xBE2F10A2, 0xBE605C13, 0xBE888E93, 0xBEA09AE5, 0xBEB8442A,
          0xBECF7BCA, 0xBEE63375, 0xBEFC5D27, 0xBF08F59B, 0xBF13682A, 0xBF1D7FD1, 0xBF273656, 0xBF3085BB,
          0x3CC90AB0, 0x3D96A905, 0x3DFAB273, 0x3E2F10A2, 0x3E605C13, 0x3E888E93, 0x3EA09AE5, 0x3EB8442A,
          0x3ECF7BCA, 0x3EE63375, 0x3EFC5D27, 0x3F08F59B, 0x3F13682A, 0x3F1D7FD1, 0x3F273656, 0x3F3085BB,
          0x3CC90AB0, 0x3D96A905, 0x3DFAB273, 0x3E2F10A2, 0x3E605C13, 0x3E888E93, 0x3EA09AE5, 0x3EB8442A,
          0x3ECF7BCA, 0x3EE63375, 0x3EFC5D27, 0x3F08F59B, 0x3F13682A, 0x3F1D7FD1, 0x3F273656, 0x3F3085BB,
          0xBCC90AB0, 0xBD96A905, 0xBDFAB273, 0xBE2F10A2, 0xBE605C13, 0xBE888E93, 0xBEA09AE5, 0xBEB8442A,
          0xBECF7BCA, 0xBEE63375, 0xBEFC5D27, 0xBF08F59B, 0xBF13682A, 0xBF1D7FD1, 0xBF273656, 0xBF3085BB,
        ]),
        arrayIntToFloat([
          0xBC490E90, 0xBD16C32C, 0xBD7B2B74, 0xBDAFB680, 0xBDE1BC2E, 0xBE09CF86, 0xBE22ABB6, 0xBE3B6ECF,
          0xBE541501, 0xBE6C9A7F, 0xBE827DC0, 0xBE8E9A22, 0xBE9AA086, 0xBEA68F12, 0xBEB263EF, 0xBEBE1D4A,
          0xBEC9B953, 0xBED53641, 0xBEE0924F, 0xBEEBCBBB, 0xBEF6E0CB, 0xBF00E7E4, 0xBF064B82, 0xBF0B9A6B,
          0xBF10D3CD, 0xBF15F6D9, 0xBF1B02C6, 0xBF1FF6CB, 0xBF24D225, 0xBF299415, 0xBF2E3BDE, 0xBF32C8C9,
          0x3C490E90, 0x3D16C32C, 0x3D7B2B74, 0x3DAFB680, 0x3DE1BC2E, 0x3E09CF86, 0x3E22ABB6, 0x3E3B6ECF,
          0x3E541501, 0x3E6C9A7F, 0x3E827DC0, 0x3E8E9A22, 0x3E9AA086, 0x3EA68F12, 0x3EB263EF, 0x3EBE1D4A,
          0x3EC9B953, 0x3ED53641, 0x3EE0924F, 0x3EEBCBBB, 0x3EF6E0CB, 0x3F00E7E4, 0x3F064B82, 0x3F0B9A6B,
          0x3F10D3CD, 0x3F15F6D9, 0x3F1B02C6, 0x3F1FF6CB, 0x3F24D225, 0x3F299415, 0x3F2E3BDE, 0x3F32C8C9,
        ]),
        arrayIntToFloat([
          0xBBC90F88, 0xBC96C9B6, 0xBCFB49BA, 0xBD2FE007, 0xBD621469, 0xBD8A200A, 0xBDA3308C, 0xBDBC3AC3,
          0xBDD53DB9, 0xBDEE3876, 0xBE039502, 0xBE1008B7, 0xBE1C76DE, 0xBE28DEFC, 0xBE354098, 0xBE419B37,
          0xBE4DEE60, 0xBE5A3997, 0xBE667C66, 0xBE72B651, 0xBE7EE6E1, 0xBE8586CE, 0xBE8B9507, 0xBE919DDD,
          0xBE97A117, 0xBE9D9E78, 0xBEA395C5, 0xBEA986C4, 0xBEAF713A, 0xBEB554EC, 0xBEBB31A0, 0xBEC1071E,
          0xBEC6D529, 0xBECC9B8B, 0xBED25A09, 0xBED8106B, 0xBEDDBE79, 0xBEE363FA, 0xBEE900B7, 0xBEEE9479,
          0xBEF41F07, 0xBEF9A02D, 0xBEFF17B2, 0xBF0242B1, 0xBF04F484, 0xBF07A136, 0xBF0A48AD, 0xBF0CEAD0,
          0xBF0F8784, 0xBF121EB0, 0xBF14B039, 0xBF173C07, 0xBF19C200, 0xBF1C420C, 0xBF1EBC12, 0xBF212FF9,
          0xBF239DA9, 0xBF26050A, 0xBF286605, 0xBF2AC082, 0xBF2D1469, 0xBF2F61A5, 0xBF31A81D, 0xBF33E7BC,
        ])
    ],
    'list3Float': arrayIntToFloat([
        0x3A3504F0, 0x3B0183B8, 0x3B70C538, 0x3BBB9268, 0x3C04A809, 0x3C308200, 0x3C61284C, 0x3C8B3F17,
        0x3CA83992, 0x3CC77FBD, 0x3CE91110, 0x3D0677CD, 0x3D198FC4, 0x3D2DD35C, 0x3D434643, 0x3D59ECC1,
        0x3D71CBA8, 0x3D85741E, 0x3D92A413, 0x3DA078B4, 0x3DAEF522, 0x3DBE1C9E, 0x3DCDF27B, 0x3DDE7A1D,
        0x3DEFB6ED, 0x3E00D62B, 0x3E0A2EDA, 0x3E13E72A, 0x3E1E00B1, 0x3E287CF2, 0x3E335D55, 0x3E3EA321,
        0x3E4A4F75, 0x3E56633F, 0x3E62DF37, 0x3E6FC3D1, 0x3E7D1138, 0x3E8563A2, 0x3E8C72B7, 0x3E93B561,
        0x3E9B2AEF, 0x3EA2D26F, 0x3EAAAAAB, 0x3EB2B222, 0x3EBAE706, 0x3EC34737, 0x3ECBD03D, 0x3ED47F46,
        0x3EDD5128, 0x3EE6425C, 0x3EEF4EFF, 0x3EF872D7, 0x3F00D4A9, 0x3F0576CA, 0x3F0A1D3B, 0x3F0EC548,
        0x3F136C25, 0x3F180EF2, 0x3F1CAAC2, 0x3F213CA2, 0x3F25C1A5, 0x3F2A36E7, 0x3F2E9998, 0x3F32E705,
        0xBF371C9E, 0xBF3B37FE, 0xBF3F36F2, 0xBF431780, 0xBF46D7E6, 0xBF4A76A4, 0xBF4DF27C, 0xBF514A6F,
        0xBF547DC5, 0xBF578C03, 0xBF5A74EE, 0xBF5D3887, 0xBF5FD707, 0xBF6250DA, 0xBF64A699, 0xBF66D908,
        0xBF68E90E, 0xBF6AD7B1, 0xBF6CA611, 0xBF6E5562, 0xBF6FE6E7, 0xBF715BEF, 0xBF72B5D1, 0xBF73F5E6,
        0xBF751D89, 0xBF762E13, 0xBF7728D7, 0xBF780F20, 0xBF78E234, 0xBF79A34C, 0xBF7A5397, 0xBF7AF439,
        0xBF7B8648, 0xBF7C0ACE, 0xBF7C82C8, 0xBF7CEF26, 0xBF7D50CB, 0xBF7DA88E, 0xBF7DF737, 0xBF7E3D86,
        0xBF7E7C2A, 0xBF7EB3CC, 0xBF7EE507, 0xBF7F106C, 0xBF7F3683, 0xBF7F57CA, 0xBF7F74B6, 0xBF7F8DB6,
        0xBF7FA32E, 0xBF7FB57B, 0xBF7FC4F6, 0xBF7FD1ED, 0xBF7FDCAD, 0xBF7FE579, 0xBF7FEC90, 0xBF7FF22E,
        0xBF7FF688, 0xBF7FF9D0, 0xBF7FFC32, 0xBF7FFDDA, 0xBF7FFEED, 0xBF7FFF8F, 0xBF7FFFDF, 0xBF7FFFFC,
    ])
}
def decode5(channel, index):
    s = channel['block']
    d = channel['wav1']
    count1 = 1
    count2 = 0x40
    for i in range(7):
        x = 0
        d1 = 0
        d2 = count2
        for j in range(count1):
            for k in range(count2):
                a = s[x]
                x += 1
                b = s[x]
                x += 1
                d[d1] = b + a
                d1 += 1
                d[d2] = a - b
                d2 += 1
            d1 += count2
            d2 += count2
        w = s
        s = d
        d = w
        count1 <<= 1
        count2 >>= 1
    s = channel['wav1']
    d = channel['block']
    count1 = 0x40
    count2 = 1
    for i in range(7):
        list1Float = DECODE5['list1Float'][i]
        list2Float = DECODE5['list2Float'][i]
        x = 0
        y = 0
        s1 = 0
        s2 = count2
        d1 = 0
        d2 = count2 * 2 - 1
        for j in range(count1):
            for k in range(count2):
                a = s[s1]
                s1 += 1
                b = s[s2]
                s2 += 1
                c = list1Float[x]
                x += 1
                e = list2Float[y]
                y += 1
                d[d1] = a * c - b * e
                d1 += 1
                d[d2] = a * e + b * c
                d2 -= 1
            s1 += count2
            s2 += count2
            d1 += count2
            d2 += count2 * 3
        w = s
        s = d
        d = w
        count1 >>= 1
        count2 <<= 1
    d = channel['wav2']
    for i in range(0x80):
        d[i] = s[i]
    s = DECODE5['list3Float']
    d = channel['wave'][index]
    s1 = channel['wav2']
    s2 = channel['wav3']
    for i in range(0x40):
        d[i] = s1[0x40 + i] * s[i] + s2[i]
    for i in range(0x40):
        d[0x40 + i] = s[0x40 + i] * s1[0x7f - i] - s2[0x40 + i]
    for i in range(0x40):
        s2[i] = s1[0x3f - i] * s[0x7f - i]
    for i in range(0x40):
        s2[0x40 + i] = s[0x3f - i] * s1[i]

def decodeBlock(hca, buffer, address):
    block = buffer[address : address + hca['blockSize']]
    if (checkSum(block, hca['blockSize'])):
        return False
    if (hca['ciphType']):
        decryptBlock(hca['ciphTable'], block)
        buffer[address : address + hca['blockSize']] = block
    reader = BlockReader(block)
    magic = reader.getBit(16)
    if (magic == 0xFFFF):
        a = (reader.getBit(9) << 8) - reader.getBit(7)
        comp = hca['comp']
        for i in range(hca['channelCount']):
            decode1(hca['channels'][i], reader, comp['r09'], a, hca['athTable'])
        for i in range(8):
            for j in range(hca['channelCount']):
                decode2(hca['channels'][j], reader)
            for j in range(hca['channelCount']):
                decode3(hca['channels'][j], comp['r09'], comp['r08'], comp['r07'] + comp['r06'], comp['r05'])
            for j in range(hca['channelCount'] - 1):
                decode4(hca['channels'][j], hca['channels'][j + 1], i, comp['r05'] - comp['r06'], comp['r06'], comp['r07'])
            for j in range(hca['channelCount']):
                decode5(hca['channels'][j], i)
    return True

def decodeHca(buffer, key, awbKey, volume):
    if (volume is None):
        volume = 1.0
    if (type(buffer) is str):
        with open(buffer, 'rb') as f:
            buffer = bytearray(f.read())
    hca = parseHCA(buffer, key, awbKey)
    if (not hca):
        raise RuntimeError('Not HCA File')
    if (not initDecode(hca)):
        raise RuntimeError('Init Decode Failed')
    n = 0
    address = hca['dataOffset']
    pcmData = Float32Array(hca['blockCount'] * 8 * 0x80 * hca['channelCount'])
    for m in range(hca['blockCount']):
        if (not decodeBlock(hca, buffer, address)):
            raise RuntimeError('Decode Error')
        for i in range(8):
            for j in range(0x80):
                for k in range(hca['channelCount']):
                    pcmData[n] = hca['channels'][k]['wave'][i][j] * hca['volume'] * volume
                    n+=1
        address += hca['blockSize']
    hca['pcmData'] = pcmData
    return hca

def writeWavFile(wavPath, mode, channelCount, samplingRate, pcmData):
    wavRiff = bytearray(36)
    wavRiff[0:4] = b'RIFF'#.write('RIFF', 0)
    wavRiff[8:16] = b'WAVEfmt '#.write('WAVEfmt ', 8)
    wavRiff[16:20] = (0x10).to_bytes(4,'little')#.writeUInt32LE(0x10, 0x10)
    wavData = bytearray(8)
    wavData[0:4] = b'data'#.write('data', 0)
    wav = {}
    wav['fmtType'] = 1 if (mode > 0) else 3
    wav['fmtChannelCount'] = channelCount
    wav['fmtBitCount'] = mode if (mode > 0) else 32
    wav['fmtSamplingRate'] = samplingRate
    wav['fmtSamplingSize'] = math.floor(wav['fmtBitCount'] / 8 * wav['fmtChannelCount'])
    wav['fmtSamplesPerSec'] = wav['fmtSamplingRate'] * wav['fmtSamplingSize']
    wavRiff[20:22] = (wav['fmtType']).to_bytes(2,'little')#.writeUInt16LE(wav['fmtType'], 0x14)
    wavRiff[22:24] = (wav['fmtChannelCount']).to_bytes(2,'little')#.writeUInt16LE(wav['fmtChannelCount'], 0x16)
    wavRiff[24:28] = (wav['fmtSamplingRate']).to_bytes(4,'little')#.writeUInt32LE(wav['fmtSamplingRate'], 0x18)
    wavRiff[28:32] = (wav['fmtSamplesPerSec']).to_bytes(4,'little')#.writeUInt32LE(wav['fmtSamplesPerSec'], 0x1C)
    wavRiff[32:36] = (wav['fmtSamplingSize']).to_bytes(4,'little')#.writeUInt32LE(wav['fmtSamplingSize'], 0x20)
    wavRiff[34:36] = (wav['fmtBitCount']).to_bytes(2,'little')#.writeUInt16LE(wav['fmtBitCount'], 0x22)
    wav['dataSize'] = math.floor(len(pcmData) * wav['fmtSamplingSize'] / channelCount)
    wav['riffSize'] = 0x1C + len(wavData) + wav['dataSize']
    wavData[4:8] = (wav['dataSize']).to_bytes(4,'little')#.writeUInt32LE(wav['dataSize'], 0x4)
    wavRiff[4:8] = (wav['riffSize']).to_bytes(4,'little')#.writeUInt32LE(wav['riffSize'], 0x4)
    with open(wavPath, 'wb') as f:
        f.write(wavRiff)
        f.write(wavData)
    buffer = bytearray(0xFFFFF)
    n = 0
    once = 0
    for i in range(len(pcmData)):
        f = pcmData[i]
        if (f > 1):
            f = 1
        elif (f < -1):
            f = -1
        match (mode):
            case 0:
                buffer[n:n+4] = pack_floatle(f)#.writeFloatLE(f, n)
                n += 4
            case 8:
                buffer[n:n+1] = (math.floor(f * 0x7F) + 0x80).to_bytes(1, signed=True)#.writeInt8(math.floor(f * 0x7F) + 0x80, n)
                n += 1
            case 16:
                buffer[n:n+2] = (math.floor(f * 0x7FFF)).to_bytes(2, 'little', signed=True)#.writeInt16LE(math.floor(f * 0x7FFF), n)
                n += 2
            case 24:
                buffer[n:n+4] = (math.floor(f * 0x7FFFFF)).to_bytes(4, 'little', signed=True)#.writeInt32LE(math.floor(f * 0x7FFFFF), n)
                n += 3
            case 32:
                buffer[n:n+4] = (math.floor(f * 0x7FFFFFFF)).to_bytes(4, 'little', signed=True)#.writeInt32LE(math.floor(f * 0x7FFFFFFF), n)
                n += 4
        if (once == 0):
            once = n
        if (n + once > len(buffer)):
            with open(wavPath, 'ab') as f:
                f.write(buffer[0:n])
            n = 0
    if (n > 0):
        with open(wavPath, 'ab') as f:
            f.write(buffer[0:n])

def decodeHcaToWav(buffer, key, awbKey, wavPath, volume, mode):
    if (mode is None):
        mode = 16
    if (type(buffer) is str):
        dirname, basename = os.path.split(buffer)
        filename, ext = os.path.splitext(basename)
        print(f'Reading {basename}...')
        if (wavPath is None):
            wavPath = os.path.join(dirname, filename + '.wav')
    
    wavdirname, wavbasename = os.path.split(wavPath)
    hca = decodeHca(buffer, key, awbKey, volume)
    print(f'Writing {wavbasename}...')
    writeWavFile(wavPath, mode, hca['channelCount'], hca['samplingRate'], hca['pcmData'])
