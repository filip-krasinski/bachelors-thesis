﻿namespace Parser.TypeParsers;

public class ParserUnlimitedText : IDataTypeParser
{
    public override object Parse(ByteStream byteStream, string tag, bool hasType)
    {
        var length = hasType ? byteStream.ReadUInt16() : byteStream.ReadUInt32();
        return byteStream.ReadString(length);
    }
}