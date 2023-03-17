using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializationOption
{
// The content type to deal with serialization
    string ContentType { get; }
// Deserialize the content type into a c# object
    T Deserialize<T>(string text);
}
