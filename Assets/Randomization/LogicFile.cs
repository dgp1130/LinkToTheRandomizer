#nullable enable

using System.Collections.Generic;
using YamlDotNet.Serialization;

/** Parsed representation of the full YAML logic file. */
public class LogicFile
{
    /** Dictionary which maps the check name to a list of incoming edges. */
    public Dictionary<string /* check name */, List<LogicCheckEntry>?>? Checks;

    public static LogicFile Deserialize(string yaml)
    {
        return new Deserializer().Deserialize<LogicFile>(yaml);
    }
}

/** Parsed representation of an incoming entry for a check in the YAML logic file. */
public class LogicCheckEntry
{
    /** Check name the player must start from to get to this check. */
    public string? From { get; set; }

    /** List of keys required to reach this check. */
    private List<string>? keys = null;
    public List<string> Keys
    {
        get => keys ?? new List<string> { };
        set => keys = value;
    }
}
