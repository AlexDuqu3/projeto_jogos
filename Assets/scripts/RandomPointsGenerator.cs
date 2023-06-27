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


    PointsRec[] squares = new PointsRec[4];

    //Vector2[] square1 = new Vector2[2];
    //Vector2[] square2 = new Vector2[2];
    //Vector2[] square3 = new Vector2[2];
    //Vector2[] square4 = new Vector2[2];

    public RandomPointsGenerator(Vector3 innerSquareStart, Vector3 innerSquareEnd, Vector2Int outSize)
    {
        outSize.x += 15; outSize.y += 15;
        innerSquareStart = new Vector3(innerSquareStart.x - 15, innerSquareStart.y + 15);
        innerSquareEnd = new Vector3(innerSquareEnd.x + 15, innerSquareEnd.y - 15);
        OuterStartPosition = new Vector3(innerSquareStart.x - outSize.x, innerSquareStart.y + outSize.y);
        OuterEndPosition = new Vector3(innerSquareEnd.x + outSize.x, innerSquareEnd.y - outSize.y);
        InnerStartPosition = innerSquareStart;
        
        InnerEndPosition = innerSquareEnd;

        squares[0] = new PointsRec(new Vector2(OuterStartPosition.x, OuterStartPosition.y), new Vector2(OuterEndPosition.x, innerSquareStart.y));

        squares[1] = new PointsRec(new Vector2(OuterStartPosition.x, OuterStartPosition.y), new Vector2(innerSquareStart.x, OuterEndPosition.y));

        squares[2] = new PointsRec(new Vector2(OuterStartPosition.x, innerSquareEnd.y), new Vector2(OuterEndPosition.x, OuterEndPosition.y));

        squares[3] = new PointsRec(new Vector2(innerSquareEnd.x, OuterStartPosition.y),new Vector2(OuterEndPosition.x, OuterEndPosition.y));

        /*
        square1[0] = new Vector2(OuterStartPosition.x, OuterStartPosition.y);
        square1[1] = new Vector2(OuterEndPosition.x, innerSquareStart.y);

        square2[0] = new Vector2(OuterStartPosition.x, OuterStartPosition.y);
        square2[1] = new Vector2(innerSquareStart.x, OuterEndPosition.y);

        square3[0] = new Vector2(OuterStartPosition.x, innerSquareEnd.y);
        square3[1] = new Vector2(OuterEndPosition.x, OuterEndPosition.y);

        square4[0] = new Vector2(innerSquareEnd.x, OuterStartPosition.y);
        square4[1] = new Vector2(OuterEndPosition.x, OuterEndPosition.y);*/

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
        int randomPosition = r.Next(0, 4);

        randomPoint = new Vector2(Random.Range(squares[randomPosition].start.x, squares[randomPosition].end.x + 1), Random.Range(squares[randomPosition].start.y, squares[randomPosition].end.y + 1));

        //randomPoint = new Vector2(Random.Range(square1[0].x, square1[1].x + 1), Random.Range(square1[0].y, square1[1].y + 1));
        //randomPoint = new Vector2(Random.Range(square3[0].x, square3[1].x + 1), Random.Range(square3[0].y, square3[1].y + 1));

        return randomPoint;
    }

    private class PointsRec
    {
        public Vector2 start;
        public Vector2 end;

        public PointsRec(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }
    }
}