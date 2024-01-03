using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;

public abstract class Element {

    /* VALUES */
    public int matrixX { get; private set; }
    public int matrixY { get; private set; }

    // public Vector2 Velocity;    
    // public bool isFreeFalling = true;
    public bool isActive = false;
    public bool isDead = false;

    /* ELEMENT PROPERTIES */
    public int colorIndex;
    // public float frictionFactor;

    public Element(int x, int y, int color=0) {
        matrixX = x;
        matrixY = y;
        colorIndex = color;
    }
    
    /* FORGET IT D: i forgot constructors don't inherit for computer reasons. 

    so this method is not even more efficient than just duping my thing

    public static T DefinedColor<T>(int x, int y, int color) where T:Element, new() {
        T element = new T {
            matrixX = x,
            matrixY = y,
            colorIndex = color
        };
        return element;
    }
    */

    public virtual int GetColor() {
        return colorIndex;
    }

    public abstract void Step(CelluarMatrix celluarMatrix);
    
    /*

    MATRIX MANIPULATION

    */

    public virtual void SetCoordinates(int x, int y) {
        this.matrixX = x;
        this.matrixY = y;
    }

    public virtual bool Swap(CelluarMatrix celluarMatrix, Element other) {
        return Swap(celluarMatrix, other, other.matrixX, other.matrixY);
    }

    public virtual bool Swap(CelluarMatrix celluarMatrix, Element other, int otherX, int otherY) { // neat, readable, way to store otherX, otherY vals
        if (this == other) { return false; }

        celluarMatrix.set(matrixX, matrixY, other);
        celluarMatrix.set(otherX, otherY, this);
        return true;
    }

}

public class EmptyCell : Element {

    private static Element emptyInstance;

    public static Element getInstance() {
        if (emptyInstance == null) { emptyInstance = new EmptyCell(-1, -1); }
        return emptyInstance;
    }

    public EmptyCell(int x, int y, int color=0) : base(x, y, color) {

    }

    public override void Step(CelluarMatrix celluarMatrix) {
        return;
    }

    public override void SetCoordinates(int x, int y) { }

    public override bool Swap(CelluarMatrix celluarMatrix, Element other) { return false; }

    public override bool Swap(CelluarMatrix celluarMatrix, Element other, int otherX, int otherY) { return false; }

}

public class Solid : Element {
    public Solid(int x, int y, int color=1) : base(x, y, color) {

    }

    public override void Step(CelluarMatrix celluarMatrix) {
        return;
    }
}

public class Sand : Solid {

    public Sand(int x, int y, int color=2) : base(x, y, color) {

    }

    public override void Step(CelluarMatrix celluarMatrix) {
        IntDouble previousCoords = new IntDouble(matrixX, matrixY);

        if (celluarMatrix.get(matrixX, matrixY - 1) is EmptyCell) {
            Swap(celluarMatrix, celluarMatrix.get(matrixX, matrixY - 1), matrixX, matrixY - 1);
        } else if (celluarMatrix.get(matrixX + 1, matrixY - 1) is EmptyCell) {
            Swap(celluarMatrix, celluarMatrix.get(matrixX + 1, matrixY - 1), matrixX + 1, matrixY - 1);
        } else if (celluarMatrix.get(matrixX - 1, matrixY - 1) is EmptyCell) {
            Swap(celluarMatrix, celluarMatrix.get(matrixX - 1, matrixY - 1), matrixX - 1, matrixY - 1);
        }

        isActive = previousCoords.x != matrixX || previousCoords.y != matrixY;
    }
}


public class ElementSpout {

    public enum BrushShape {
        Circle,
        Square
    }

    public int MatrixX { get; private set; }
    public int MatrixY { get; private set; }

    public ElementType SourceElement;
    public int BrushSize;
    public BrushShape Shape = BrushShape.Circle;
    public bool DoInterpolate = true;

    // cached vlaues
    private int prevMatrixX;
    private int prevMatrixY;

    public ElementSpout(int x, int y, ElementType source, int size=1, BrushShape shape=BrushShape.Circle, bool doInterpolate=true) {
        MatrixX = x;
        MatrixY = y;
        SourceElement = source;
        BrushSize = size;
        Shape = shape;
        DoInterpolate = doInterpolate;
    }

    public void SetMatrixPosition(int matrixX, int matrixY, CelluarMatrix matrix) {
        prevMatrixX = MatrixX;
        prevMatrixY = MatrixY;

        MatrixX = matrix.BoundX(matrixX);
        MatrixY = matrix.BoundY(matrixY);
    }

    public void SetPointer(CelluarMatrix matrix) {
        Debug.Log(matrix.set(MatrixX, MatrixY, matrix.NewElementInstance(MatrixX, MatrixY, ElementType.Solid)));
        Debug.Log($"{MatrixX}, {MatrixY}");
    }

    public void Spoot(CelluarMatrix matrix) {
        if (!DoInterpolate) {
            PaintElement(matrix);
        } else {

            foreach (Vector2 point in matrix.GetInterpolatedPoints(prevMatrixX, prevMatrixY, MatrixX, MatrixY)) {
                PaintElement((int) point.x, (int) point.y, matrix);
            }

        }
    }

    private void PaintElement(CelluarMatrix matrix) {
        PaintElement(MatrixX, MatrixY, matrix);
    }

    /*
    probably wanna cache already modified values
    */
    private void PaintElement(int pointX, int pointY, CelluarMatrix matrix) {
        int radius = Mathf.FloorToInt(BrushSize / 2);
        float exactRadius = BrushSize / 2;

        for (int y=matrix.BoundY(pointY - radius); y <= matrix.BoundY(pointY + radius); y++) {
            for (int x=matrix.BoundX(pointX - radius); x <= matrix.BoundX(pointX + radius); x++) {
                if (Shape == BrushShape.Square) {
                    matrix.set(x, y, matrix.NewElementInstance(x, y, SourceElement));
                } else if (Shape == BrushShape.Circle) {
                    bool isPointWithinCircle = (x-pointX)*(x-pointX) + (y-pointY)*(y-pointY) <= exactRadius*exactRadius;
                    if (isPointWithinCircle) {
                        matrix.set(x, y, matrix.NewElementInstance(x, y, SourceElement));
                    }
                }
            }
        }

        matrix.set(pointX, pointY, matrix.NewElementInstance(pointX, pointY, SourceElement));
    }
}
public enum ElementType {
    Air,
    Solid,
    BlueSand,
    YellowSand,
    PinkSand,
    PurpleSand
}


public struct IntDouble {
    public int x;
    public int y;

    public IntDouble(int x, int y) {
        this.x = x;
        this.y = y;
    }
}


public class Chunk {

    public bool isActive=true;

    public int left;
    public int right;
    public int top;
    public int bottom;

    private int width;
    private int height;
    
    public int length => width * height;
    public IntDouble[] Coordinates;

    public Chunk(int x, int y, int width, int height) {
        left = x;
        right = x + width;
        bottom = y;
        top = y + height;
        this.width = width;
        this.height = height;

        Coordinates = GetCoordinates();
    }

    public bool isWithinBounds(int x, int y) {
        return left <= x && x <= right && bottom <= y && x <= top;
    }

    private IntDouble[] GetCoordinates() {
        IntDouble[] coordinates = new IntDouble[length];
        for (int i=0; i<length; i++) {
            coordinates[i] = new IntDouble(left + i%width, bottom + (int)i/width);
        }
        return coordinates;
    }

    public bool CalculateActive(CelluarMatrix matrix) {
        foreach (IntDouble coordinates in Coordinates) {
            Element element = matrix.get(coordinates.x, coordinates.y);
            if (element != null && element.isActive) {
                isActive = true;
                return true;
            }
        }

        isActive = false;
        return false;
    }

}


public class CelluarMatrix {

    public Color[] colorLibrary = new Color[] {
        new Color(1, 0, 0), // air
        new Color(0, 1, 0), // solid
        new Color(0, 0, 1), // sand 1
        new Color(1, 1, 0), // sand 2
        new Color(0, 1, 1), // sand 3
        new Color(1, 0, 1), // sand 4
    };

    private Chunk[][] chunkMatrix;
    private int chunkMatrixWidth;
    private int chunkMatrixHeight;
    private int chunkWidth = 32; // each chunk holds 1024 elements
    private int chunkHeight = 32;

    private Element[][] matrix;
    private int matrixWidth;
    private int matrixHeight;

    public Texture2D texture;

    /*
    
    INITIALISATION

    */
    public CelluarMatrix(int width, int height) {
        matrixWidth = width;
        matrixHeight = height;
        matrix = GenerateMatrix();
    }

    public CelluarMatrix(int width, int height, Color[] colorLibrary) {
        matrixWidth = width;
        matrixHeight = height;
        matrix = GenerateMatrix();

        chunkMatrixWidth = Mathf.CeilToInt((float)matrixWidth / chunkWidth);
        chunkMatrixHeight = Mathf.CeilToInt((float)matrixHeight / chunkHeight);
        chunkMatrix = GenerateChunkMatrix();

        this.colorLibrary = colorLibrary;
        texture = new Texture2D(matrixWidth, matrixHeight, TextureFormat.ARGB32, false);
        texture = HardGenerateTexture();
    }

    public Chunk[][] GenerateChunkMatrix() {
        Debug.Log($"width:{chunkWidth} height:{chunkHeight}");
        Debug.Log($"width:{chunkMatrixWidth} height:{chunkMatrixHeight}");

        Chunk[][] newMatrix = new Chunk[chunkMatrixHeight][];
        for (int y = 0; y < chunkMatrixHeight; y++) {
            Chunk[] row = new Chunk[chunkMatrixWidth];
            int matrixY = y*chunkHeight;
            for (int x = 0; x < chunkMatrixWidth; x++) {
                int matrixX = x*chunkWidth;
                row[x] = new Chunk(matrixX, matrixY, chunkWidth, chunkHeight)
                {
                    isActive = false
                };
            }
            newMatrix[y] = row;
        }

        chunkMatrix = newMatrix;

        return newMatrix;
    }

    public Element[][] GenerateMatrix() {
        Debug.Log($"width:{matrixWidth} height:{matrixHeight}");

        Element[][] newMatrix = new Element[matrixHeight][];
        for (int y = 0; y < matrixHeight; y++) {
            Element[] row = new Element[matrixWidth];
            for (int x = 0; x < matrixWidth; x++) {
                row[x] = NewElementInstance(x, y, ElementType.Air);
            }
            newMatrix[y] = row;
        }

        matrix = newMatrix;

        return newMatrix;
    }

    public Element NewElementInstance(int x, int y, ElementType elementType) {
        if (elementType == ElementType.Air) {
            return EmptyCell.getInstance();
        } else if (elementType == ElementType.Solid) {
            return new Solid(x, y);
        } else if (elementType == ElementType.BlueSand) {
            return new Sand(x, y, 2);
        } else if (elementType == ElementType.YellowSand) {
            return new Sand(x, y, 3);
        } else if (elementType == ElementType.PinkSand) {
            return new Sand(x, y, 4);
        } else if (elementType == ElementType.PurpleSand) {
            return new Sand(x, y, 5);
        }
        return EmptyCell.getInstance();
    }


    /*
    
    AUTOMATA

    */

    public void StepAndDrawAll(bool doChunk=true) {
        StepAll(doChunk);
        DrawAll();
        CalculateActiveAll();
    }

    private void StepAll(bool doChunk=true) {
        if (doChunk) {
            StepAllChunked();
        } else {
            List<int> indices = GenerateShuffledIndices(matrixWidth);
            for (int y = 0; y < matrixHeight; y++) {
                Element[] row = getRow(y);

                /*for (int x = 0; x < matrixWidth; x++) {
                    Element element = row[x];
                    if (element != null) {
                        element.Step(this);
                    }
                }*/

                foreach (int x in indices) {
                    Element element = row[x];

                    if (element != null) {
                        element.Step(this);
                    }
                }
            }
        }
        
    }

    private void StepAllChunked() {
        for (int chunkY=0; chunkY<chunkMatrix.Length; chunkY++) {
            Chunk[] chunkRow = chunkMatrix[chunkY];
            for (int chunkX=0; chunkX<chunkRow.Length; chunkX++) {
                Chunk chunk = chunkRow[chunkX];
                if (chunk.isActive) {
                    // step all chunk elements
                    foreach (IntDouble coordinates in chunk.Coordinates) {
                        Element element = get(coordinates.x, coordinates.y);
                        if (element != null) { element.Step(this); }
                    }
                }

                
            }
        }
        
    }


    private void CalculateActiveAll() {
        for (int chunkY=0; chunkY<chunkMatrix.Length; chunkY++) {
            Chunk[] chunkRow = chunkMatrix[chunkY];
            for (int chunkX=0; chunkX<chunkRow.Length; chunkX++) {
                
                Chunk chunk = chunkRow[chunkX];
                if (chunk.isActive) {
                    chunk.CalculateActive(this);
                }
            }
        }
    }

    private List<int> GenerateShuffledIndices(int length) {
        List<int> indices = new List<int>(length);
        for (int i=0; i<length; i++) { indices.Add(i); }

        while (length > 1) {  
            length--;  
            int k = Random.Range(0, length+1);  
            int value = indices[k];  
            indices[k] = indices[length];  
            indices[length] = value;  
        }

        return indices;
    }


    /*
    
    REFERENCE TOOLS

    */
    public Element get(int x, int y) { return !isWithinBounds(x, y) ? null : matrix[y][x]; }
    public Element[] getRow(int y) { return !isWithinBounds(0, y) ? null : matrix[y]; }

    public bool set(int x, int y, Element element) {
        if (!isWithinBounds(x, y)) { return false; }
        matrix[y][x] = element;
        element.SetCoordinates(x, y);

        if (element != EmptyCell.getInstance()) {
            GetChunk(x, y).isActive = true;
        }

        return true;
    }

    public Chunk GetChunk(int matrixX, int matrixY) {

        // Debug.Log($"{chunkMatrixHeight}{chunkMatrix.Length} : {chunkMatrixWidth}{chunkMatrix[0].Length}");
        return chunkMatrix[Mathf.FloorToInt(matrixY / chunkHeight)][Mathf.FloorToInt(matrixX / chunkWidth)];
    }

    public bool isWithinBounds(int x, int y) { return x >= 0 && y >= 0 && x < matrixWidth && y < matrixHeight; }
    public Vector2 BoundCoordinates(int x, int y) { return new Vector2(Mathf.Clamp(x, 0, matrixWidth - 1), Mathf.Clamp(y, 0, matrixHeight - 1)); }
    public int BoundX(int x) { return Mathf.Clamp(x, 0, matrixWidth - 1); }
    public int BoundY(int y) { return Mathf.Clamp(y, 0, matrixHeight - 1); }

    /*

    Given a the starting and ending points of a line, return a list of the points in between.

    */
    public Vector2[] GetInterpolatedPoints(int prevx, int prevy, int currx, int curry) {
        
        // return the only point if there is no change
        if (prevx == currx && prevy == curry) { return new Vector2[]{ new Vector2(currx, curry) }; }

        int diffx = currx - prevx;
        int diffy = curry - prevy;

        // the number of points is whichever axis has more difference
        Vector2[] points;

        if (Mathf.Abs(diffx) >= Mathf.Abs(diffy)) { // going along the x-axis
            // points are added starting from the previous to the current point
            points = new Vector2[Mathf.Abs(diffx) + 1];

            int minX = Mathf.Min(prevx, currx);
            int maxX = Mathf.Max(prevx, currx);
            for (int x=minX; x<=maxX; x+=1) {
                int y = Mathf.RoundToInt(((diffx == 0) ? 0 : (float)diffy / diffx) * (x - prevx) + prevy); // y value = mx + b
                points[Mathf.Abs(x - prevx)] = new Vector2(x, y);
            }

        } else { // going along the y-axis
            points = new Vector2[Mathf.Abs(diffy) + 1];

            int minY = Mathf.Min(prevy, curry);
            int maxY = Mathf.Max(prevy, curry);
            for (int y=minY; y<=maxY; y+=1) {
                int x = Mathf.RoundToInt(((diffx == 0) ? 0 : (float)diffx / diffy) * (y - prevy) + prevx); // y value = mx + b
                points[Mathf.Abs(y - prevy)] = new Vector2(x, y);
            }
        }


        return points;

    }

 
    /*

    GRAPHICS

    */
    private void DrawAll() {
        texture = GenerateTexture();
    }

    private Texture2D GenerateTexture() {
        for (int chunkY=0; chunkY<chunkMatrix.Length; chunkY++) {
            Chunk[] chunkRow = chunkMatrix[chunkY];
            for (int chunkX=0; chunkX<chunkRow.Length; chunkX++) {
                Chunk chunk = chunkRow[chunkX];
                if (chunk.isActive) {
                    // step all chunk elements
                    foreach (IntDouble coord in chunk.Coordinates) {
                        Element element = get(coord.x, coord.y);
                        if (element == null) {

                        } else {
                            texture.SetPixel(coord.x, coord.y, colorLibrary[element.colorIndex]); // element.color
                        }
                    }
                }
                
            }
        }
        texture.Apply();

        return texture;
    }

    private Texture2D HardGenerateTexture() {
        for (int y = 0; y < matrixHeight; y++) {
            Element[] row = getRow(y);
            for (int x = 0; x < matrixWidth; x++) {
                Element element = row[x];
                if (element == null) {

                } else{
                    texture.SetPixel(x, y, colorLibrary[element.colorIndex]); // element.color
                }
                
            }
        }
        texture.Apply();

        return texture;
    }

    /* 

    PLAYER TOOLS

    */

    public bool SpawnElement(Vector2 screenSpacePoint, RectTransform spriteRect, Camera camera, Element elementType) {
        Vector2 matrixCoordinates = GetMatrixCoordinates(screenSpacePoint, spriteRect, camera);
        if (!isWithinBounds((int) matrixCoordinates.x, (int) matrixCoordinates.y)) { return false; }

        set((int) matrixCoordinates.x, (int) matrixCoordinates.x, elementType);
        return true;
    }

    public Vector2 GetMatrixCoordinates(Vector2 screenSpacePoint, RectTransform spriteRect, Camera camera) {
        Vector2 RectSpacePoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(spriteRect, screenSpacePoint, camera, out RectSpacePoint);
        
        Vector2 RectFractionalPoint = RectSpacePoint / spriteRect.rect.size;

        Vector2 MatrixPoint = new Vector2(
            Mathf.RoundToInt(RectFractionalPoint.x * matrixWidth), 
            Mathf.RoundToInt(RectFractionalPoint.y * matrixHeight)
        );
        return MatrixPoint;
    }

}

public class SandSim : MonoBehaviour {

    /* 
    Inspector Values
    */
    public Color[] ColorLibrary = new Color[] {
        new Color(1, 0, 0), // air
        new Color(0, 1, 0), // solid
        new Color(0, 0, 1), // sand 1
        new Color(1, 1, 0), // sand 2
        new Color(0, 1, 1), // sand 3
        new Color(1, 0, 1), // sand 4
    };
    public UnityEngine.UI.Image simulationImage;
    public Camera ViewCamera;

    /*
    Sim References
    */
    public CelluarMatrix celluarMatrix;

    public bool doChunk;
    public bool useImageDeltaAsSim;

    public static int SimulationWidth = 240;
    public static int SimulationHeight = 360;
    
    [Header("Sand Spooter")]
    public Game game;
    public ElementSpout userBrush;
    private DateTime startTime;
    public Vector2 brushCoordinates;
    private int secondsSinceStart => (int) (DateTime.Now - startTime).TotalSeconds * 1000;
    private int numSpoots;

    private void Awake() {
        if (useImageDeltaAsSim) {
            Vector2 imageDelta = simulationImage.GetComponent<RectTransform>().sizeDelta;
            celluarMatrix = new CelluarMatrix((int) imageDelta.x, (int) imageDelta.y, ColorLibrary);
        } else {
            celluarMatrix = new CelluarMatrix(SimulationWidth, SimulationHeight, ColorLibrary);
        }

        userBrush = new ElementSpout(0, 0, ElementType.BlueSand, 1, ElementSpout.BrushShape.Circle, false);

        // StartCoroutine(UpdateSandSim());

        Texture2D texture = celluarMatrix.texture;
        
        simulationImage.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        startTime = DateTime.Now;
    }

    private void Update() {
        if (game.CurrentSand.SandType == Game.SandType.Work) {
            userBrush.SourceElement = ElementType.BlueSand;
        } else if (game.CurrentSand.SandType == Game.SandType.Necessity) {
            userBrush.SourceElement = ElementType.YellowSand;
        } else if (game.CurrentSand.SandType == Game.SandType.Break) {
            userBrush.SourceElement = ElementType.PinkSand;
        } else if (game.CurrentSand.SandType == Game.SandType.LongBreak) {
            userBrush.SourceElement = ElementType.PurpleSand;
        }

        brushCoordinates = celluarMatrix.GetMatrixCoordinates(Mouse.current.position.ReadValue(), simulationImage.GetComponent<RectTransform>(), ViewCamera);
        if (celluarMatrix.isWithinBounds((int) brushCoordinates.x, (int) brushCoordinates.y)) {
            userBrush.SetMatrixPosition((int) brushCoordinates.x, (int) brushCoordinates.y, celluarMatrix);
        } else {
            userBrush.SetMatrixPosition(120, 360, celluarMatrix);
        }
        
        Debug.Log($"{secondsSinceStart}, {numSpoots}");
        if (numSpoots < secondsSinceStart) {
            userBrush.Spoot(celluarMatrix);
            numSpoots += 1;
        }

        Step();
    }

    [ContextMenu("Step")]
    public void Step() {
        celluarMatrix.StepAndDrawAll(doChunk);
    }

}