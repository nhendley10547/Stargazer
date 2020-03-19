using UnityEngine;

public static class Calculate {
    public static Vector3 PositionFromAngle
        (Vector3 originPosition, Vector3 eulerAngles, float distanceFromOrigin) {
        Vector3 direction = DirectionFromAngle(eulerAngles);
        return (originPosition + direction * distanceFromOrigin);
    }

    public static Vector3 DirectionFromAngle(Vector3 eulerAngles){
        Vector3 radianAngles = eulerAngles * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(radianAngles.y) * Mathf.Cos(radianAngles.x),
            -Mathf.Sin(radianAngles.x),
            Mathf.Cos(radianAngles.y) * Mathf.Cos(radianAngles.x)
        );
    }
}
