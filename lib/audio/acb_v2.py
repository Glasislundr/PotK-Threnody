import os
import traceback
import json
import lib.audio.utf_v2 as utf
from lib.audio.utils import *

"""
The linking used here is mainly based on the ACB loading done by Vgmstream:

# Normally games load a .acb + .awb, and asks the .acb to play a cue by name or index.
# Since we only care for actual waves, to get its name we need to find which cue uses our wave.
# Multiple cues can use the same wave (meaning multiple names), and one cue may use multiple waves.
# There is no easy way to map cue name <> wave name so basically we parse the whole thing.
#
# .acb are created in CRI Atom Craft, where user defines N Cues with CueName each, then link somehow
# to a Waveform (.awb=streamed or memory .acb=internal, data 'material' encoded in some format),
# depending on reference types. Typical links are:
# - CueName > Cue > Waveform (type 1)
# - CueName > Cue > Synth > Waveform (type 2)
# - CueName > Cue > Sequence > Track > Command > Synth > Waveform (type 3, <=v1.27)
# - CueName > Cue > Sequence > Track > Command > Synth > Synth > Waveform (type 3, <=v1.27)
# - CueName > Cue > Sequence > Track > TrackEvent > Command > Synth > Waveform (type 3, >=v1.28)
# - CueName > Cue > Sequence > Track > TrackEvent > Command > Synth > Synth > Waveform (type 3, >=v1.28)
# - CueName > Cue > Sequence > Track > TrackEvent > Command > Sequence > (...) > Synth > Waveform (type 3, >=v1.28)
# - CueName > Cue > Block > Track > Command > Synth > Synth > Waveform (type 8)
# - others should be possible
# Atom Craft may only target certain .acb versions so some links are later removed
# Not all cues to point to Waveforms, some are just config events/commands.
# .acb link to .awb by name (loaded manually), though they have a checksum/hash/header to validate.
#
# .acb can contain info for multiple .awb, that are loaded sequentially and assigned "port numbers" (0 to N).
# Both Wave ID and port number must be passed externally to find appropriate song name.
# 
# To improve performance we pre-read each table objects's useful fields. Extra complex files may include +8000 objects,
# per table, meaning it uses a decent chunk of memory, but having to re-read with streamfiles is much slower.
 
"""

class ACB():
    parsed_file: list
    
    def __init__(self, inPath):
        with open(inPath, 'br') as buffer:
            obj = utf.parseUtf(buffer.read(), True)
        self.parsed_file = reformatAcbFile(obj)
    
    def getFirstWaveformForCue(self, cueName):
        for acb in self.parsed_file:
            for cue in acb['Cues']:
                if cueName == cue['CueName']:
                    if 'Waveform' in cue:
                        return cue['Waveform']
                    if 'Synth' in cue:
                        wave = getWaveformFromSynth(cue['Synth'])
                        if wave is not None:
                            return wave
                    if 'Sequence' in cue:
                        wave = getWaveformFromSequence(cue['Sequence'])
                        if wave is not None:
                            return wave
                    if 'BlockSequence' in cue:
                        wave = getWaveformFromBlockSequence(cue['BlockSequence'])
                        if wave is not None:
                            return wave
                        
                                
def getWaveformFromSynth(syn):
    if 'Command' in syn:
        for cmdAct in syn['Command']['CommandActions']:
            wave = getWaveformFromCommand(cmdAct)
            if wave is not None:
                return wave
    if 'SubItems' in syn:
        for subItem in syn['SubItems']:
            if subItem['SuperType'] == 'Waveform':
                return subItem
            elif subItem['SuperType'] == 'Synth':
                wave = getWaveformFromSynth(subItem)
                if wave is not None:
                    return wave
            elif subItem['SuperType'] == 'Sequence':
                wave = getWaveformFromSequence(subItem)
                if wave is not None:
                    return wave
    return None

def getWaveformFromSequence(seq):
    if 'Tracks' in seq:
        for trk in seq['Tracks']:
            wave = getWaveformFromTrack(trk)
            if wave is not None:
                return wave
    if 'Command' in seq:
        for cmdAct in seq['Command']['CommandActions']:
            wave = getWaveformFromCommand(cmdAct)
            if wave is not None:
                return wave
    return None

def getWaveformFromBlockSequence(seq):
    if 'Blocks' in seq:
        for blk in seq['Blocks']:
            wave = getWaveformFromBlock(blk)
            if wave is not None:
                return wave
    if 'Tracks' in seq:
        for trk in seq['Tracks']:
            wave = getWaveformFromTrack(trk)
            if wave is not None:
                return wave
    return None

def getWaveformFromBlock(blk):
    if 'Tracks' in blk:
        for trk in blk['Tracks']:
            wave = getWaveformFromTrack(trk)
            if wave is not None:
                return wave
    return None

def getWaveformFromTrack(trk):
    if 'Event' in trk:
        for cmdAct in trk['Event']['CommandActions']:
            wave = getWaveformFromCommand(cmdAct)
            if wave is not None:
                return wave
    if 'Command' in trk:
        for cmdAct in trk['Command']['CommandActions']:
            wave = getWaveformFromCommand(cmdAct)
            if wave is not None:
                return wave
    return None

def getWaveformFromCommand(cmd):
    if 'Synth' in cmd:
        wave = getWaveformFromSynth(cmd['Synth'])
        if wave is not None:
            return wave
    if 'Sequence' in cmd:
        wave = getWaveformFromSequence(cmd['Sequence'])
        if wave is not None:
            return wave
    return None

def readAcbFile(inPath, outPath=None):
    if (not outPath):
        pathInfo = os.path.splitext(inPath)    
        outPath = pathInfo[0] + '.json'
    print('Parsing '+inPath+'...')
    with open(inPath, 'br') as buffer:
        obj = utf.parseUtf(buffer.read(), True)
    parsed_acb = reformatAcbFile(obj)
    print('Writing '+outPath+'...')
    with open(outPath,'w') as outputFile:
        outputFile.write(json.dumps(parsed_acb, indent=2))

def reformatAcbFile(utf):
    parsed_acb = []
    for acb in utf:
        pacb = {}
        pacb['Name'] = acb['Name']
        pacb['Version'] = acb['Version'].to_bytes(4,'big').hex()
        pacb['AcbVolume'] = acb['AcbVolume']
        pacb['AwbFile'] = acb['AwbFile']
        pacb['Cues'] = []
        for cueName_i in acb['CueNameTable']:
            cue = {}
            cue['CueName'] = cueName_i['CueName']
            cue['CueIndex'] = cueName_i['CueIndex']
            cue['cueReferenceType'] = acb['CueTable'][cue['CueIndex']]['ReferenceType']
            cue['cueReferenceIndex'] = acb['CueTable'][cue['CueIndex']]['ReferenceIndex']
            cue['cueNumAisacControlMaps'] = acb['CueTable'][cue['CueIndex']]['NumAisacControlMaps']
            cue['cueAisacControlMap'] = acb['CueTable'][cue['CueIndex']]['AisacControlMap']
            match cue['cueReferenceType']:
                # 0x01: Cue > Waveform (ex. PES 2015)
                case 0x01:
                    cue['Waveform'] = getWaveform(acb, cue['cueReferenceIndex'])
                # 0x02: Cue > Synth > Waveform (ex. Ukiyo no Roushi) 
                case 0x02:
                    cue['Synth'] = getSynth(acb, cue['cueReferenceIndex'])
                # 0x03: Cue > Sequence > Track > Command > Synth > Waveform (ex. Valkyrie Profile anatomia, Yakuza Kiwami 2) 
                case 0x03:
                    cue['Sequence'] = getSequence(acb, cue['cueReferenceIndex'])
                # 0x08: Cue > BlockSequence > Track / Block > Track > Command > Synth > Waveform (ex. Sonic Lost World, Kandagawa Jet Girls, rare) 
                case 0x08:
                    cue['BlockSequence'] = getBlockSequence(acb, cue['cueReferenceIndex'])
                # 0x00: none 
                # 0x04: "track" 
                # 0x05: "outsideLink" 
                # 0x06: "insideLinkSynth" (ex. PES 2014) 
                # 0x07: "insideLinkSequence" (ex. PES 2014) 
                # 0x09: "insideLinkBlockSequence" 
                # 0x0a: "eventCue_UnUse" 
                # 0x0b: "soundGenerator" 
                case 0x00 | 0x04 | 0x05 | 0x06 | 0x07 | 0x09 | 0x0a | 0x0b | _: 
                    print(f"ACB: unknown Cue.ReferenceType={cue['cueReferenceType']}, Cue.ReferenceIndex={cue['cueReferenceIndex']}")
            pacb['Cues'] += [cue]
        parsed_acb += [pacb]
    return parsed_acb

seq_command_table = ''
track_command_table = ''
synth_command_table = ''

def getSeqCommandTable(acb):
    if seq_command_table:
        return seq_command_table
    if 'SeqCommandTable' in acb:
        return 'SeqCommandTable'
    return 'CommandTable'

def getTrackCommandTable(acb):
    if track_command_table:
        return track_command_table
    if 'TrackCommandTable' in acb:
        return 'TrackCommandTable'
    return 'CommandTable'

def getSynthCommandTable(acb):
    if synth_command_table:
        return synth_command_table
    if 'SynthCommandTable' in acb:
        return 'SynthCommandTable'
    return 'CommandTable'

def getWaveform(acb, index):
    wav = acb['WaveformTable'][index]
    wav['SuperType'] = 'Waveform'
    wav['SamplingRate'] = wav['SamplingRate'] & 0xffff
    if 'Id' in wav:
        #This is using the old format, rather than the new MemoryAwbId/StreamAwbId split
        if wav['Streaming']:
            # Accesses a separate awb file
            wav['MemoryAwbId'] = -1
            wav['StreamAwbId'] = wav['Id']
        else:
            # Accesses the internal awb file
            wav['MemoryAwbId'] = wav['Id']
            wav['StreamAwbId'] = -1
    if 'EncodeType' in wav:
        # Newer format that has different encoding type options. Older seems to assume HCA?
        # Types found at https://github.com/LazyBone152/XV2-Tools/blob/master/Xv2CoreLib/ACB/ACB_File.cs
            # ADX = 0,
            # HCA = 2,
            # HCA_ALT = 6,
            # VAG = 7,
            # ATRAC3 = 8,
            # BCWAV = 9,
            # ATRAC9 = 11,
            # DSP = 13,
            # None = 255
        match wav['EncodeType']:
            case 0:
                wav['Encoding'] = 'ADX'
            case 2:
                wav['Encoding'] = 'HCA'
            case 6:
                wav['Encoding'] = 'HCA_ALT'
            case 7:
                wav['Encoding'] = 'VAG'
            case 8:
                wav['Encoding'] = 'ATRAC3'
            case 9:
                wav['Encoding'] = 'BCWAV'
            case 11:
                wav['Encoding'] = 'ATRAC9'
            case 13:
                wav['Encoding'] = 'DSP'
            case 255:
                wav['Encoding'] = 'None'
            case _:
                wav['Encoding'] = 'UNKNOWN'
                
    if 'ExtensionData' in wav and wav['ExtensionData'] >= 0 and wav['ExtensionData'] < len(acb['WaveformExtensionDataTable']):
        wav['LoopStart'] = acb['WaveformExtensionDataTable'][wav['ExtensionData']]['LoopStart']
        wav['LoopEnd'] = acb['WaveformExtensionDataTable'][wav['ExtensionData']]['LoopEnd']
    return wav

def getSynth(acb, index, cueReference=2):
    syn = acb['SynthTable'][index]
    syn['SuperType'] = 'Synth'
    
    syn['Type'] = 'Synth'
    
    #  - 0: polyphonic (1 item)
    #  - 1: sequential (1 to N?)
    #  - 2: shuffle (1 from N?)
    #  - 3: random (1 from N?)
    #  - 4: random no repeat
    #  - 5: switch game variable
    #  - 6: combo sequential
    #  - 7: switch selector
    #  - 8: track transition by selector
    #  - other: undefined?
    
    if 'CommandIndex' in syn and syn['CommandIndex'] >= 0 and syn['CommandIndex'] < len(acb[getSynthCommandTable(acb)]):
        cmd = {}
        cmd['SuperType'] = 'Command'
        cmd['RawCommand'] = acb[getSynthCommandTable(acb)][syn['CommandIndex']]['Command']
        cmd['CommandActions'] = parseCommand(acb, cmd['RawCommand'])
        syn['Command'] = cmd
    if 'LocalAisacs' in syn and syn['LocalAisacs']:
        aisacs = syn['LocalAisacs']
        syn['Aisacs'] = []
        for i in range(int(len(aisacs) / 4)):
            aisac = int(aisacs[i*4:i*4+4], 16)
            syn['Aisacs'] += [getAisac(acb, aisac)]
            
    if 'ReferenceItems' in syn and syn['ReferenceItems']:
        items = syn['ReferenceItems']
        syn['SubItems'] = []
        for i in range(int(len(items) / 8)):
            itemtype = int(items[i*8:i*8+4], 16)
            itemindex = int(items[i*8+4:i*8+8], 16)
            match itemtype:
                case 0x00: # no reference
                    pass
                case 0x01: # Waveform (most common)
                    syn['SubItems'] += [getWaveform(acb,itemindex)]
                case 0x02: # Synth, possibly random (rare, found in Sonic Lost World with ReferenceType 2)
                    syn['SubItems'] += [getSynth(acb,itemindex)]
                case 0x03: # Sequence of Synths w/ % in Synth.TrackValues (rare, found in Sonic Lost World with ReferenceType 2) 
                    syn['SubItems'] += [getSequence(acb,itemindex)]
                # others: same as cue's ReferenceType? 

                # 0x06: this seems to point to Synth but results don't make sense (rare, from Sonic Lost World) 
                # _: undefined/crashes AtomViewer 
                case 0x06 | _:
                    print(f'Unknown ReferenceType for Synth: {itemtype}')
    return syn
    
def getSequence(acb, index):
    seq = acb['SequenceTable'][index]
    seq['SuperType'] = 'Sequence'
    seq['Tracks'] = []
    for i in range(seq['NumTracks']):
        trk = int(seq['TrackIndex'][i*4:i*4+4], 16)
        seq['Tracks'] += [getTrack(acb, trk)]
    if 'CommandIndex' in seq and seq['CommandIndex'] >= 0 and seq['CommandIndex'] < len(acb[getSeqCommandTable(acb)]):
        cmd = {}
        cmd['SuperType'] = 'Command'
        cmd['RawCommand'] = acb[getSeqCommandTable(acb)][seq['CommandIndex']]['Command']
        cmd['CommandActions'] = parseCommand(acb, cmd['RawCommand'])
        seq['Command'] = cmd
    return seq
    
def getBlockSequence(acb, index):
    bsq = acb['BlockSequenceTable'][index]
    bsq['SuperType'] = 'BlockSequence'
    bsq['Blocks'] = []
    for i in range(bsq['NumBlocks']):
        trk = int(bsq['BlockIndex'][i*4:i*4+4], 16)
        bsq['Blocks'] += [getBlock(acb, trk)]
    bsq['Tracks'] = []
    for i in range(bsq['NumTracks']):
        trk = int(bsq['TrackIndex'][i*4:i*4+4], 16)
        bsq['Tracks'] += [getTrack(acb, trk)]
    
    return bsq
    
def getBlock(acb, index):
    blk = acb['BlockTable'][index]
    blk['SuperType'] = 'Block'
    blk['Tracks'] = []
    for i in range(blk['NumTracks']):
        trk = int(blk['TrackIndex'][i*4:i*4+4], 16)
        blk['Tracks'] += [getTrack(acb, trk)]
    
    return blk
    
def getAisac(acb, index):
    asc = acb['AisacTable'][index]
    asc['SuperType'] = 'Aisac'
    if 'GraphIndexes' in asc and asc['GraphIndexes']:
        graphs = asc['GraphIndexes']
        asc['Graphs'] = []
        for i in range(int(len(graphs) / 4)):
            graph = int(graphs[i*4:i*4+4], 16)
            asc['Graphs'] += [getGraph(acb, graph)]
    
    return asc
    
def getGraph(acb, index):
    gph = acb['GraphTable'][index]
    gph['SuperType'] = 'Graph'
    if 'Controls' in gph and gph['Controls'] and 'Destinations' in gph and gph['Destinations']:
        ctrls = gph['Controls']
        dests = gph['Destinations']
        clen = int(len(ctrls) / 8)
        dlen = int(len(dests) / 4)
        if (clen != dlen):
            print("Graph has non-matching controls and destinations")
        else:
            gph['GraphActions'] = []
            for i in range(clen):
                ctrl = get_floatbe(bytes.fromhex(ctrls[i*8:i*8+8]))
                dest = int(dests[i*4:i*4+4], 16)
                nc = {}
                nc['Time'] = ctrl
                nc['Volumne'] = dest
                gph['GraphActions'] += [nc]

    return gph

def getTrack(acb, index):
    trk = acb['TrackTable'][index]
    trk['SuperType'] = 'Track'
    
    if 'EventIndex' in trk and trk['EventIndex'] != 0xffff and trk['EventIndex'] >= 0 and trk['EventIndex'] < len(acb[getTrackCommandTable(acb)]):
        cmd = {}
        cmd['SuperType'] = 'Command'
        cmd['RawCommand'] = acb[getTrackCommandTable(acb)][trk['EventIndex']]['Command']
        cmd['CommandActions'] = parseCommand(acb, cmd['RawCommand'])
        trk['Event'] = cmd
    if 'CommandIndex' in trk and trk['CommandIndex'] >= 0 and trk['CommandIndex'] < len(acb[getTrackCommandTable(acb)]):
        cmd = {}
        cmd['SuperType'] = 'Command'
        cmd['RawCommand'] = acb[getTrackCommandTable(acb)][trk['CommandIndex']]['Command']
        cmd['CommandActions'] = parseCommand(acb, cmd['RawCommand'])
        trk['Command'] = cmd
    if 'LocalAisacs' in trk and trk['LocalAisacs']:
        aisacs = trk['LocalAisacs']
        trk['Aisacs'] = []
        for i in range(int(len(aisacs) / 4)):
            aisac = int(aisacs[i*4:i*4+4], 16)
            trk['Aisacs'] += [getAisac(acb, aisac)]
    
    return trk


def storeOtherCommandTlv(cmdTxt, newCmd, pos, tlv_size, tlv_code, name):
    tlv_extraData = cmdTxt[pos:pos + tlv_size*2]
    newCmd['CommandType'] = tlv_code
    newCmd['CommandTypeText'] = name
    newCmd['CommandExtraData'] = tlv_extraData

def parseCommand(acb, cmd):
    """
        Starting info for commands came from Vgmstream
        https://github.com/vgmstream/vgmstream/blob/master/src/meta/acb.c
        Additional information gathered from XV2-Tools
        https://github.com/LazyBone152/XV2-Tools/blob/master/Xv2CoreLib/ACB/ACB_File.cs
    """
    pos = 0x00
    max_pos = len(cmd)
    
    cmds = []

    # read a (name)Command multiple TLV data 
    while (pos < max_pos):
        newCmd = {}
        tlv_code = int(cmd[pos:pos + 4], 16)#get_u16be(read_u16be(Command_offset + pos + 0x00, sf))
        tlv_size = int(cmd[pos + 4:pos + 6], 16)#get_u8(read_u8(Command_offset + pos + 0x02, sf))
        pos  += 6
        
        # There are around 160 codes (some unused), with things like set volume, pan, stop, mute, and so on.
        #  Multiple commands are linked and only "note on" seems to point so other objects, so maybe others
        #  apply to current object (since there is "note off" without reference. 
        match tlv_code:
            case 2000 | 2003 | 2004: 
                # 2000: noteOn
                # 2003: noteOnWithNo plus 16b (null?) [rare, ex. PES 2014] 
                # 2004: noteOnWithDuration same as the above plus extra field 
                # 
                # From XV2:
                #      2003/0x07d3 has 6 parameters (3 uint16s). Same as 2000/0x07d0, 
                #       but with an additional 2 parameters. Requires a preceeding 1991/0x07c7 to function.
                if (tlv_size < 0x04):
                    print("acb: TLV with unknown size\n")
                else:
                    tlv_type = int(cmd[pos:pos + 4], 16)#get_u16be(read_u16be(Command_offset + pos + 0x00, sf)) # ReferenceItem 
                    tlv_index = int(cmd[pos + 4:pos + 8], 16)#get_u16be(read_u16be(Command_offset + pos + 0x02, sf))
                    tlv_extraData = cmd[pos + 8:pos + tlv_size*2]
                    
                    newCmd['CommandType'] = tlv_code
                    newCmd['CommandTypeText'] = 'noteOn' if tlv_code == 2000 else ('noteOnWithNo' if tlv_code == 2003 else 'noteOnWithDuration')
                    newCmd['CommandIndex'] = tlv_index
                    newCmd['CommandExtraData'] = tlv_extraData
                    #print("acb: TLV at %x: type %x, index=%x\n", offset, tlv_type, tlv_index)

                    # same as Synth's ReferenceItem type? 
                    match tlv_type:
                        case 0x02: # Synth (common) 
                            newCmd['Synth'] = getSynth(acb,tlv_index)

                        case 0x03: # Sequence (common, ex. Yakuza 6, Yakuza Kiwami 2) 
                            newCmd['Sequence'] = getSequence(acb,tlv_index)

                        case _:
                            print(f"acb: unknown TLV type {tlv_type} at {Command_offset + pos} + {tlv_size}")
            
            case 33: # 33: mute 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'mute')
            case 124: # 124: stopAtLoopEnd 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'stopAtLoopEnd')
            case 1000: # 1000: noteOff 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'noteOff')
            case 1251: # 1251: sequenceCallbackWithId 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'sequenceCallbackWithId')
            case 1252: # 1252: sequenceCallbackWithString 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'sequenceCallbackWithString')
            case 1253: # 1253: sequenceCallbackWithIdAndString 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'sequenceCallbackWithIdAndString')
            case 2002: # 2002: setSynthOrWaveform 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'setSynthOrWaveform')
            case 4051: # 4051: transitionTrack 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'transitionTrack')
            case 7102: # 7102: muteTrackAction 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'muteTrackAction')
            case 7100: # 7100: startAction 
                # XV2: No parameters. Uses target information from ActionTrack.
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'startAction')
            case 7101: # 7101: stopAction 
                # XV2: No parameters. Uses target information from ActionTrack.
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'stopAction')
                # may be needed? 
                #print("acb: TLV at %x: check code %i?\n", offset-0x03, tlv_code)

            case 0: # 0: no-op 
                newCmd['CommandType'] = tlv_code
                newCmd['CommandTypeText'] = 'no-op'
            case 998: # 998: sequenceStartRandom (plays following note ons in random?)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'sequenceStartRandom')
            case 999: # 999: sequenceStart (plays following note ons in sequence?) 
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'sequenceStart')
                
            # From XV2-Tools:
            # The following items are not all clear, but are included for informative purposes
            case 0x004b: #GlobalAisacReference - 2 parameters (1 uint16). 0 = GlobalAisacReference (index)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'GlobalAisacReference')
            case 0x004f: #CueLimit - 5 bytes, 1st value (uint16) = cue limit (Sequence only?)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'CueLimit')
            case 0x0058: #VolumeRandomization1 - 2 parameters (1 uint16) 0 = Random Range (scale 0-100, base volume is 0)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'VolumeRandomization1')
            case 0x0059: #VolumeRandomization2 - 4 parameters (2 uint16s) 0 = base volume, 1 = Random Range (scale 0-100)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'VolumeRandomization2')
            case 0x006f: #VolumeBus - 4 parameters (2 uint16s). 1st value = StringValue, 2nd value = volume, scale between 0 and 10000 (Sequence only)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'VolumeBus')
            case 0x04b1: #LoopStart - 4 parameters, 2 uint16s (0=LoopID,1=Loop Count)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'LoopStart')
            case 0x04b0: #LoopEnd - 6 parameters 3 uint16s (0=LoopID)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'LoopEnd')
            case 0x07d1: #Wait - 4 parameters (1 uint32). Wait command, freezes command execution for desired time (0 = miliseconds)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Wait')
            case 0x0071: #BeatSyncReference - 4 parameters (2 uint16s). 0 = ?, 1 = BeatSyncTable index
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'BeatSyncReference')

            #Slightly known commands:
            case 0x0010: #Unk10 - 2 params (1 uint16). Seems to control volume, but only with a Unk85 entry present.
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk10')
            case 0x0055: #Bus - 2 parameters (1 uint16). Points to a StringValue (Bus).
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Bus')

            #Unknown, but safe to copy commands - these are known not to have any references on them (invalid references will CRASH the game)
            #Added here so they dont get purged along with the other unknown (unsafe) commands
            case 0x0031: #Unk49 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk49')
            case 0x0054: #Unk84 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk84')
            case 0x0053: #Unk83 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk83')
            case 0x000b: #Unk11 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk11')
            case 0x000e: #Unk14 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk14')
            case 0x0024: #Unk36 - 4 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk36')
            case 0x0026: #Unk38 - 4 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk38')
            case 0x0028: #Unk40 - 4 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk40')
            case 0x002d: #Unk45 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk45')
            case 0x002f: #Unk47 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk47')
            case 0x0005: #Unk5 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk5')
            case 0x0046: #Unk70 - 1 parameter
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk70')
            case 0x0056: #Unk86 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk86')
            case 0x07c7: #Unk199 - 6 parameters. Required for ReferencedItem2 to function (must preceed it).
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk199')
            case 0x0041: #Unk65 - 4 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk65')
            case 0x0057: #Unk87 - 2 parameters
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk87')
            case 0x0069: #Unk105 - 1 parameter. Used in ParameterPallets
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk105')
            case 0x0022: #Unk34 - 2 parameters (1 uint16)
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk34')
            case 0x0033: #Unk51 - 2 params
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk51')
            case 0x0035: #Unk53 - 2 params
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk53')
            case 0x0037: #Unk55 - 2 params
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk55')
            case 0x0039: #Unk57 - 2 params
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk57')
            case 0x003b: #Unk59 - 2 params
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'Unk59')
                
                
                
                
                
                
            case  _: # default: catch all
                storeOtherCommandTlv(cmd, newCmd, pos, tlv_size, tlv_code, 'UNKNOWN')
        cmds += [newCmd]
        pos += tlv_size * 2
    return cmds