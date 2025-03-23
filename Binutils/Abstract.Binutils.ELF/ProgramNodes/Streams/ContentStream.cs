using System.Text;

namespace Abstract.Binutils.ELF.ProgramNodes.Streams;

public sealed class ContentStream : Stream
{
    private MemoryStream _memStream => new();
    public MemoryStream stream => _memStream;

    public override bool CanRead => _memStream.CanRead;
    public override bool CanSeek =>_memStream.CanSeek;
    public override bool CanWrite => _memStream.CanWrite;
    public override long Length => _memStream.Length;
    public override long Position { get => _memStream.Position; set => _memStream.Position = value; }

    public override void Flush() => _memStream.Flush();
    public override int Read(byte[] buffer, int offset, int count) => _memStream.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _memStream.Seek(offset, origin);
    public override void SetLength(long value) => _memStream.SetLength(value);


    public override void Write(byte[] buffer, int offset, int count) => _memStream.Write(buffer, offset, count);
    public void Write(byte[] buffer) => _memStream.Write(buffer);
    public void Write(Span<byte> buffer) => _memStream.Write(buffer);

    public void WriteString_ASCII(string value) => _memStream.Write(Encoding.ASCII.GetBytes(value));
    public void WriteString_UTF8(string value) => _memStream.Write(Encoding.UTF8.GetBytes(value));


    protected override void Dispose(bool disposing) => _memStream.Dispose();
}
