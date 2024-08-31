import io
import struct


class MasterDataReader:
    buf: io.BytesIO

    def __init__(self, length):
        self.buf = io.BytesIO()
        self.endian = '<'
        self.WriteInt(0)
        self.WriteInt(length)
        self.WriteInt(0)

    def write(self, *args):
        return self.buf.read(*args)

    def WriteBool(self, val):
        self.buf.write(struct.pack(self.endian+"b", val))

    def WriteBoolOrNull(self, val):
        if not val:
            self.WriteBool(False)
        if val:
            self.WriteBool(True)
            self.WriteBool(True)

    def WriteInt(self, val):
        self.buf.write(struct.pack(self.endian+"i", val))

    def ReadIntOrNull(self, val):
        if self.val == 0:
            self.WriteBool(False)
        else:
            self.WriteBool(True)
            self.WriteInt(val)

    def WriteString(self, val, intern=False):
        self.WriteInt(len(val))
        self.buf.write(val.encode("utf16"))

    def WriteStringOrNull(self, val, intern=False) -> str:
        if not val:
            self.WriteBool(False)
        else:
            self.WriteBool(True)
            return self.WriteString(val, intern)

    def WriteFloat(self, val):
        self.buf.write(struct.pack(self.endian+"f", val))

    def WriteFloatOrNull(self, val):
        if not val:
            self.WriteBool(False)
        else:
            self.WriteBool(True)
            self.WriteFloat(val)

    def WriteDateTime(self, val):
        self.WriteString(val, False)

    def WriteDateTimeOrNull(self, val):
        s = self.WriteStringOrNull(val, False)
