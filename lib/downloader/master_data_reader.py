import io
import struct


class MasterDataReader:
    buf: bytes
    n: int
    length: int
    charBuf: str

    def __init__(self, buf: bytes):
        self.buf = io.BytesIO(buf)
        #self.n = 0
        self.endian = '<'
        self.ReadInt()
        self.length = self.ReadInt()
        self.ReadInt()
        #self.charBuf = ' '*self.ReadInt()

    def Length(self):
        return self.length

    def read(self, *args):
        #print(self.buf.tell(), *args)
        return self.buf.read(*args)

    def ReadBool(self) -> bool:
        return struct.unpack(self.endian+"b", self.read(1))[0]

    def ReadBoolOrNull(self) -> bool:
        if not self.ReadBool():
            return False
        else:
            return self.ReadBool()

    def ReadInt(self) -> int:
        return struct.unpack(self.endian+"i", self.read(4))[0]

    def ReadIntOrNull(self) -> int:
        if not self.ReadBool():
            return 0
        else:
            return self.ReadInt()

    def ReadString(self, intern=False) -> str:
        length = self.ReadInt()
        ret = self.read(length*2)
        #if intern:
            # some internal string search? - replacemet
        return ret.decode('utf16')

    def ReadStringOrNull(self, intern=False) -> str:
        if not self.ReadBool():
            return ''
        else:
            return self.ReadString(intern)

    def ReadFloat(self) -> float:
        return struct.unpack(self.endian+"f", self.read(4))[0]

    def ReadFloatOrNull(self) -> float:
        if not self.ReadBool():
            return 0.0
        else:
            return self.ReadFloat()

    def ReadDateTime(self):
        # DateTime.Parse(self.ReadString(false));}
        return self.ReadString(False)

    def ReadDateTimeOrNull(self):
        return self.ReadStringOrNull(False)
