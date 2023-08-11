using System;

[Serializable]
public class SchemaNetworkPackage
{
    #region Fields

    public string action;
    public string data;

    #endregion
}

[Serializable]
public class SchemaTestMessage
{
    public string dataString;
    public int dataInt;
    public float dataFloat;
}
