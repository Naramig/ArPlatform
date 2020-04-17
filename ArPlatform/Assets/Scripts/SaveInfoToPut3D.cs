public static class SaveInfoToPut3D
{
    private static string nameOfObject;
    private static string nameOfGroup;
    private static int idOfTheObjectToFind;


    public static int IdOfTheObjectToFind
    {

        get
        {
            return idOfTheObjectToFind;
        }

        set
        {
            idOfTheObjectToFind = value;
        }
    }
    public static string NameOfObject
    {

        get
        {
            return nameOfObject;
        }

        set
        {
            nameOfObject = value;
        }
    }
    public static string NameOfGroup
    {

        get
        {
            return nameOfGroup;
        }

        set
        {
            nameOfGroup = value;
        }
    }
}
