﻿namespace Avrocado;

internal sealed class AvroReaderException : AvroException
{
    public AvroReaderException(string? message)
        : base(message)
    {
    }
}
