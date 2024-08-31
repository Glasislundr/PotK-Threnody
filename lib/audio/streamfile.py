import os
import io

def close_vgmstream(vgms):
    pass #TODO?

def setup_subfile_streamfile(sf, subfile_offset, subfile_size, extension):
    return FakeByteFile(sf, subfile_offset, subfile_size, fakeExt=extension)

def reopen_streamfile(sf, buffer_size):
    return sf #TODO?

def close_streamfile(sf):
    pass#sf.close()

def read_streamfile(buf, offset, size, sf):
    sf.seek(offset)
    out = sf.read(size)
    buf[0:size] = out
    return len(out)

def get_streamfile_size(sf):
    sf.seek(0, os.SEEK_END)
    return sf.tell()

class FakeByteFile:
    f: io.IOBase
    name: str
    offset: int
    size: int
    def __init__(self, sf, offset, size, fakeName=None, fakeExt=None):
        self.f = sf
        self.name = sf.name
        self.offset = offset
        self.size = size
        if fakeName or fakeExt:
            pth, ext = os.path.splitext(self.name)
            if fakeName:
                pth = fakeName
            if fakeExt:
                ext = fakeExt
            if len(ext) > 0 and ext[0] == '.':
                self.name = pth + ext
            else:
                self.name = pth + '.' + ext
    def read(self, size=-1):
        pos = self.f.tell()
        if pos < self.offset:
            self.f.seek(self.offset)
            pos = self.offset
        elif pos >= (self.offset + self.size):
            return b''
        if size >= 0:
            return self.f.read(min(size, self.size - (pos - self.offset)))
        else:
            return self.f.read(self.size - (pos - self.offset))
    def seek(self, offset, whence=0):
        if type(offset) is bytearray or type(offset) is bytes:
            match len(offset):
                case 1:
                    offset = get_u8(offset)
                case 2:
                    offset = get_u16be(offset)
                case 4:
                    offset = get_u32be(offset)
                case 8:
                    offset = get_u64be(offset)
                case _:
                    raise RuntimeError('Offset is in unknown byte format')
    
        match whence:
            case os.SEEK_SET:
                return self.f.seek(self.offset + offset, whence)
            case os.SEEK_CUR:
                return self.f.seek(offset, whence)
            case os.SEEK_END:
                return self.f.seek(self.offset + self.size + offset, os.SEEK_SET)
            case _:
                raise IOError("Didn't implement other whence options")
    def tell(self):
        og = self.f.tell()
        if og <= self.offset:
            return 0
        elif og >= (self.offset + self.size):
            return self.size
        else:
            return og - self.offset
    def close(self):
        self.f.close()