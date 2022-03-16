using UnityEngine;

public static class StaticDirection
{
    public static Vector2 northVector = new Vector2(0, 1);
    public static Vector2 southVector = new Vector2(0, -1);
    public static Vector2 eastVector = new Vector2(1, 0);
    public static Vector2 westVector = new Vector2(-1, 0);


    public static Vector2 GetRandomDirVector()
    {
        int d = UnityEngine.Random.Range(0, 4);

        switch (d)
        {
            case 0:
                return northVector;
            case 1:
                return eastVector;
            case 2:
                return southVector;
            case 3:
                return westVector;
        }
        return Vector2.zero;
    }

}

