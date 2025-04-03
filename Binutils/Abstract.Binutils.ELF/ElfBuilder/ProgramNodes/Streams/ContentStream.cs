using System.Text;

namespace Abstract.Binutils.ELF.ElfBuilder.ProgramNodes.Streams;

public sealed class ContentStream : Stream
{
    internal readonly MemoryStream _memStream = new();

    public override bool CanRead => _memStream.CanRead;
    public override bool CanSeek => _memStream.CanSeek;
    public override bool CanWrite => _memStream.CanWrite;
    public override long Length => _memStream.Length;
    public override long Position { get => _memStream.Position; set => _memStream.Position = value; }

    public override void Flush() => _memStream.Flush();
    public override int Read(byte[] buffer, int offset, int count) => _memStream.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _memStream.Seek(offset, origin);
    public override void SetLength(long value) => _memStream.SetLength(value);


    public override void Write(byte[] buffer, int offset, int count) => _memStream.Write(buffer, offset, count);

    public void WriteString_ASCII(string value) => Write(Encoding.ASCII.GetBytes(value));
    public void WriteString_UTF8(string value) => Write(Encoding.UTF8.GetBytes(value));


    public ContentStream Truncate()
    {
        _memStream.Capacity = (int)_memStream.Length;
        return this;
    }
    protected override void Dispose(bool disposing) => _memStream.Dispose();

    public override string ToString()
    {
        var sb = new StringBuilder();
        bool writingBinary = false;

        while (_memStream.Position < _memStream.Length)
        {
            if (writingBinary)
            {
                var b = _memStream.ReadByte();
                if (char.IsAsciiLetter((char)b))
                    sb.Append($"{(char)b} ");
                else
                    sb.Append($"{b:X2} ");
            }
            else
            {
                var b = _memStream.ReadByte();

                if (!char.IsControl((char)b)) sb.Append($"{(char)b}");
                else // Convert to binary
                {
                    var sbval = sb.ToString();
                    sb.Clear();
                    sb.Append(string.Join(" ", sbval.Split()));
                    sb.Append(' ');
                }
            }
        }
        if (writingBinary && _memStream.Length > 0) sb.Length -= 1;

        return sb.ToString();
    }
}
