using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RandomPointsGenerator
{
    Vector3 InnerStartPosition;
    Vector3 InnerEndPosition;
    Vector3 OuterStartPosition;
    Vector3 OuterEndPosition;
    public Vector2Int distance;

    Vector2[] square1 = new Vector2[2];
    Vector2[] square2 = new Vector2[2];
    Vector2[] square3 = new Vector2[2];
    Vector2[] square4 = new Vector2[2];

    public RandomPointsGenerator(Vector3 innerSquareStart, Vector3 innerSquareEnd, Vector2Int outSize)
    {
        OuterStartPosition = new Vector3(innerSquareStart.x - outSize.x, innerSquareStart.y + outSize.y);
        OuterEndPosition = new Vector3(innerSquareEnd.x + outSize.x, innerSquareEnd.y - outSize.y);
        InnerStartPosition = innerSquareStart;
        InnerEndPosition = innerSquareEnd;

        square1[0] = new Vector2(OuterStartPosition.x, OuterStartPosition.y);
        square1[1] = new Vector2(OuterEndPosition.x, innerSquareStart.y);

        square2[0] = new Vector2(OuterStartPosition.x, OuterStartPosition.y);
        square2[1] = new Vector2(innerSquareStart.x, OuterEndPosition.y);

        square3[0] = new Vector2(OuterStartPosition.x, innerSquareEnd.y);
        square3[1] = new Vector2(OuterEndPosition.x, OuterEndPosition.y);

        square4[0] = new Vector2(innerSquareEnd.x, OuterStartPosition.y);
        square4[1] = new Vector2(OuterEndPosition.x, OuterEndPosition.y);

        this.distance = outSize;
    }
 
    public Vector2[] GenerateRandomPoints(int numberOfPoints)
    {
        Vector2[] randomPoints = new Vector2[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            randomPoints[i] = GetRandomPoint();
           
           
        }
        return randomPoints;
    }

    private Vector2 GetRandomPoint()
    {

        Vector2 randomPoint;
        //Random.Range(OuterStartPosition.x, OuterEndPosition.x + 1), Random.Range(OuterStartPosition.y, OuterEndPosition.y + 1)
        var r = new System.Random();
        int randomPosition = r.Next(0, 3);

        randomPoint = new Vector2(Random.Range(square1[0].x, square1[1].x + 1), Random.Range(square1[0].y, square1[1].y + 1));

        return randomPoint;
    }

}