import struct


# transforms a string to uint32 (for comparison), but if this is static + all goes well
# compiler should pre-calculate and use uint32 directly
def get_id32be(s):
    return struct.unpack(">L", s[:4])[0]
def get_id64be(s):
    return struct.unpack(">Q", s[:8])[0]

#static inline int16_t get_s16be(const uint8_t* p) {
#    return ((uint16_t)p[0]<<8) | ((uint16_t)p[1])
#}
#static inline uint16_t get_u16be(const uint8_t* p) { return (uint16_t)get_s16be(p) }

#static inline int32_t get_s32be(const uint8_t* p) {
#    return ((uint32_t)p[0]<<24) | ((uint32_t)p[1]<<16) | ((uint32_t)p[2]<<8) | ((uint32_t)p[3])

#static inline uint32_t get_u32be(const uint8_t* p) { return (uint32_t)get_s32be(p) }

dle = struct.Struct('<d')
fle = struct.Struct('<f')
u64le = struct.Struct('<L')
s64le = struct.Struct('<l')
u32le = struct.Struct('<I')
s32le = struct.Struct('<i')
u16le = struct.Struct('<H')
s16le = struct.Struct('<h')

dbe = struct.Struct('>d')
fbe = struct.Struct('>f')
u64be = struct.Struct('>L')
s64be = struct.Struct('>l')
u32be = struct.Struct('>I')
s32be = struct.Struct('>i')
u16be = struct.Struct('>H')
s16be = struct.Struct('>h')
u8 = struct.Struct('B')
s8 = struct.Struct('b')

def pack_floatle(p):
    return fle.pack(p)

def get_doublele(p):
    return dle.unpack(p[0:8])[0]
def get_floatle(p):
    return fle.unpack(p[0:4])[0]

def get_u64le(p):
    return u64le.unpack(p[0:8])[0]
def get_s64le(p):
    return s64le.unpack(p[0:8])[0]
def get_u32le(p):
    return u32le.unpack(p[0:4])[0]
def get_s32le(p):
    return s32le.unpack(p[0:4])[0]
def get_u16le(p):
    return u16le.unpack(p[0:2])[0]
def get_s16le(p):
    return s16le.unpack(p[0:2])[0]

def get_doublebe(p):
    return dbe.unpack(p[0:8])[0]
def get_floatbe(p):
    return fbe.unpack(p[0:4])[0]

def get_u64be(p):
    return u64be.unpack(p[0:8])[0]
def get_s64be(p):
    return s64be.unpack(p[0:8])[0]
def get_u32be(p):
    return u32be.unpack(p[0:4])[0]
def get_s32be(p):
    return s32be.unpack(p[0:4])[0]
def get_u16be(p):
    return u16be.unpack(p[0:2])[0]
def get_s16be(p):
    return s16be.unpack(p[0:2])[0]
def get_u8(p):
    return u8.unpack(p[0:1])[0]
def get_s8(p):
    return s8.unpack(p[0:1])[0]


def read_u64be(offset, sf):
    return read_s64be(offset, sf)
def read_s64be(offset, sf):
    buf = bytearray(8)
    sf.seek(offset)
    buf[0:8] = sf.read(8)
    return buf
def read_f32be(offset, sf):
    return read_s32be(offset, sf)
def read_u32be(offset, sf):
    return read_s32be(offset, sf)
def read_s32be(offset, sf):
    buf = bytearray(4)
    sf.seek(offset)
    buf[0:4] = sf.read(4)
    return buf
def read_u16be(offset, sf):
    return read_s16be(offset, sf)
def read_s16be(offset, sf):
    buf = bytearray(2)
    sf.seek(offset)
    buf[0:2] = sf.read(2)
    return buf
def read_u8(offset, sf):
    return read_s8(offset, sf)
def read_s8(offset, sf):
    buf = bytearray(1)
    sf.seek(offset)
    buf[0:1] = sf.read(1)
    return buf

#def getStringFromBuffer(buf):
    
    
# fastest to compare would be read_u32x == (uint32), but should be pre-optimized (see get_id32x) */
def is_id32be(offset, sf, s):
    return read_u32be(offset, sf) == get_id32be(s).to_bytes(4,'big');

def is_id64be(offset, sf, s):
    return read_u64be(offset, sf) == get_id64be(s).to_bytes(8,'big');