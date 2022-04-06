﻿using System.Globalization;

namespace Parser.TypeParsers;

public class ParserDate : IDataTypeParser
{
    public override object Parse(ByteStream byteStream, string tag, bool hasType)
    {
        var length = hasType ? byteStream.ReadUInt16() : byteStream.ReadUInt32();
        var dateString = byteStream.ReadString(length);
        if (string.IsNullOrEmpty(dateString))
            return dateString;

        return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
    }
}