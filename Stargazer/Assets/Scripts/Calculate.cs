using UnityEngine;

public static class Calculate {
    public static Vector3 DirectionBasedPosition
        (Vector3 originPosition, Vector3 eulerAngles, float distanceFromOrigin) {
        Vector3 trueDirection = HeadingBasedDirection(originPosition, eulerAngles);
        return (originPosition + trueDirection * distanceFromOrigin);
    }

    public static Vector3 HeadingBasedDirection(Vector3 originPosition, Vector3 eulerAngles){
        Vector3 heading = eulerAngles * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(heading.y) * Mathf.Cos(heading.x),
            -Mathf.Sin(heading.x),
            Mathf.Cos(heading.y) * Mathf.Cos(heading.x)
        );
    }
}