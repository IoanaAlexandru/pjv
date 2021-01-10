using UnityEditor;

[CustomPropertyDrawer(typeof(TileProbabilityDictionary))]
[CustomPropertyDrawer(typeof(ObjectProbabilityDictionary))]
public class CustomSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
