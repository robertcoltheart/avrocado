﻿using System.Text.Json;
using AvroSerialize.Serialization.Metadata.Schemas;

namespace AvroSerialize.Serialization.Converters;

public class RecordSchemaConverter : TrackedConverter<RecordSchema>
{
    public override RecordSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, TrackedResources tracked, JsonSerializerOptions options)
    {
        reader.ReadObject();

        var schema = new RecordSchema(SchemaType.Record);
        var fullName = reader.GetFullName();

        tracked.Schemas[fullName] = schema;
        tracked.SchemaTree.Push(schema);

        while (reader.IsInObject())
        {
            var property = reader.ReadMember();

            if (property == "name")
            {
                schema.SchemaName = new SchemaName
                {
                    Name = reader.GetString()!
                };
            }
            else if (property == "namespace")
            {
                schema.Namespace = reader.GetString()!;
            }
            else if (property == "doc")
            {
                schema.Documentation = reader.GetString()!;
            }
            else if (property == "aliases")
            {
                schema.Aliases = JsonSerializer.Deserialize<List<string>>(ref reader, options)!;
            }
            else if (property == "fields")
            {
                schema.Fields = reader.ReadTrackedList<Field>(tracked, options);
            }
            else
            {
                reader.Skip();
            }

            reader.Read();
        }

        return schema;
    }

    public override void Write(Utf8JsonWriter writer, RecordSchema value, TrackedResources tracked, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
