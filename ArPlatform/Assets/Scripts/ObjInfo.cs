public static class ObjInfo
{
    private static int id;
    private static string name, obj;
    private static float lat, longt;

    public static int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public static string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public static float Lat
    {
        get
        {
            return lat;
        }
        set
        {
            lat = value;
        }
    }

    public static float Longt
    {
        get
        {
            return longt;
        }
        set
        {
            longt = value;
        }
    }
    public static string Obj
    {
        get
        {
            return obj;
        }
        set
        {
            obj = value;
        }
    }
}