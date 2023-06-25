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
    public float TileSize;

    public RandomPointsGenerator(Vector3 innerSquareStart, Vector3 innerSquareEnd, Vector2Int outSize,float TileSize)
    {
        OuterStartPosition = new Vector3(innerSquareStart.x - outSize.x, innerSquareStart.y + outSize.y);
        OuterEndPosition = new Vector3(innerSquareEnd.x + outSize.x, innerSquareEnd.y - outSize.y);
        InnerStartPosition= innerSquareStart;
        InnerEndPosition= innerSquareEnd;
        this.distance = outSize;
        this.TileSize = TileSize;
        Vector3 worldStartPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Calculate the starting position of the outer square
       
    }
 
    public Vector2[] GenerateRandomPoints(int numberOfPoints)
    {
        Vector2[] randomPoints = new Vector2[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {

           
           
        }
        return randomPoints;
    }

    private Vector2 GetRandomPoint()
    {

        Vector2 randomPoint;
        bool isInsideInnerRect = true;

        do
        {
            randomPoint = new Vector2(Random.Range(OuterStartPosition.x, OuterEndPosition.x + 1), Random.Range(OuterStartPosition.y, OuterEndPosition.y + 1));
            int x= (int)randomPoint.x;
            int y = (int)randomPoint.y;
            int distance = this.distance.x;
            int innerMapX = (int)InnerStartPosition.x;
            int innerMapY = (int)InnerStartPosition.y;
            if (x < distance || x >= (innerMapX + distance) || y < distance || y >= (innerMapY + distance))
            {

                // Create outer tile at position (x, y)
                // Adjust the position based on the outerSquareStart and TileSize
                isInsideInnerRect=false;
            }
            else if (x >= distance && x < (innerMapX + distance) && (y == distance || y == (innerMapY + distance - 1)))
            {
                isInsideInnerRect = false;
                // Create frame tile on the top and bottom rows of the inner square
                // Adjust the position based on the outerSquareStart and TileSize
                // Example: Instantiate(frameTilePrefab, outerSquareStart + new Vector3(TileSize * x, -TileSize * y, 0), Quaternion.identity);
            }
            else if ((x == distance || x == (innerMapX + distance - 1)) && y >= distance && y < (innerMapY + distance))
            {
                isInsideInnerRect = false;
                // Create frame tile on the left and right columns of the inner square
                // Adjust the position based on the outerSquareStart and TileSize
                // Example: Instantiate(frameTilePrefab, outerSquareStart + new Vector3(TileSize * x, -TileSize * y, 0), Quaternion.identity);
            }
        } while (isInsideInnerRect);

        return randomPoint;
    }

}