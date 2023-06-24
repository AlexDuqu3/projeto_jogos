using Unity.VisualScripting;
using UnityEngine;

public class RandomPointsGenerator
{
    public Vector2 outerSquareStart;
    public Vector2 outerSquareSize;
    public Vector2 innerSquareStart;
    public Vector2 innerSquareSize;

    public RandomPointsGenerator(Vector2 innerSquareStart, Vector2 innerSquareSize, Vector2 outSize,float TileSize)
    {
        this.innerSquareSize=innerSquareSize;
        this.innerSquareStart = innerSquareStart;
        Vector2 outerPoints = new Vector2(0, 0);
        if(innerSquareStart.y<0)
        {
            outerPoints.y = innerSquareStart.y - outSize.y;
        }
        else{
            outerPoints.y=innerSquareStart.y + outSize.y;
        }
        
            outerPoints.x=innerSquareStart.x - outSize.x;

        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Calculate the starting position of the outer square
        outerSquareStart = new Vector3(worldStartPosition.x - (TileSize * outSize.x), worldStartPosition.y + (TileSize * outSize.y), 0);
        outerSquareSize = new Vector2(innerSquareStart.x+(2*outSize.x),innerSquareStart.y+(2*outSize.x));
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
        float minX = outerSquareStart.x + innerSquareSize.x;
        float minY = outerSquareStart.y + innerSquareSize.y;
        float maxX = outerSquareStart.x + outerSquareSize.x;
        float maxY = outerSquareStart.y + outerSquareSize.y;

        Vector2 randomPoint = Vector2.zero;
        do
        {
            randomPoint = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        } while (IsPointInsideInnerSquare(randomPoint));

        return randomPoint;
    }

    private bool IsPointInsideInnerSquare(Vector2 point)
    {
        return point.x >= innerSquareStart.x && point.x <= innerSquareStart.x + innerSquareSize.x
            && point.y >= innerSquareStart.y && point.y <= innerSquareStart.y + innerSquareSize.y;
    }
}