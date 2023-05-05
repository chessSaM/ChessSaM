using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Tabla : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardcenter = Vector3.zero;

    [Header("Prefabs & Material")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterial;

    private SahovskeFigure[,] chessPieces;
    private SahovskeFigure currentlyDragging;
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private void Awake()
    {
        GenerateAllTiles(tileSize,TILE_COUNT_X,TILE_COUNT_Y);
        SpawingAllPieces();
        PositionAllPieces();
        
    }

    private void Update()
    {
        // Hover-ovanje i oznacavanje polja na tabli 
        if(!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }
        RaycastHit info;
        Ray ray=currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile","Hover")))
        {
            Vector2Int hitPosition = LookupTileIndeks(info.transform.gameObject);

            if(currentHover== -Vector2Int.one) 
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            if (currentHover !=hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
            
            // Pokreti misa 
        if(Input.GetMouseButtonDown(0))
        {
            if (chessPieces[hitPosition.x,hitPosition.y]!=null)
                {
                    if(true)
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];
                    }
                }
        }
        if(Input.GetMouseButtonDown(0))
        {

        }


        }
        else
        {
            if(currentHover!=-Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover=-Vector2Int.one;
            }
        }

       
    }
    
    // Generisanje Table i polja 
    private void GenerateAllTiles(float tileSize, int tile_count_x, int tile_count_y)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tile_count_x / 2) * tileSize, 0, (tile_count_x / 2) * tileSize) + boardcenter;
       
        
        tiles=new GameObject[tile_count_x, tile_count_y]; 
        for(int i = 0; i < tile_count_x; i++)
        {
            for(int j = 0;j<tile_count_y;j++)
                tiles[i,j]=GenerateSingleTile(tileSize,i,j);
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int i, int j)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0},Y:{1}", i, j));
        tileObject.transform.parent = transform;
        Mesh mesh=new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh=mesh;
        tileObject.AddComponent<MeshRenderer>().material=tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0]=new Vector3(i*tileSize,yOffset,j*tileSize)-bounds;
        vertices[1] = new Vector3(i * tileSize, yOffset, (j+1) * tileSize)-bounds;
        vertices[2] = new Vector3((i+1) * tileSize, yOffset, j * tileSize)-bounds;
        vertices[3] = new Vector3((i + 1) * tileSize, yOffset, (j+1) * tileSize)-bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");

        tileObject.AddComponent<BoxCollider>();
        return tileObject;
    }

    // Operacije
    private Vector2Int LookupTileIndeks(GameObject hitInfo)
    {
        for (int i = 0; i < TILE_COUNT_X; i++) 
        {
            for (int j = 0; j < TILE_COUNT_Y; j++)
                if (tiles[i, j] == hitInfo)
                    return new Vector2Int(i, j);
        }
        return -Vector2Int.one;
    }

    // Stvaranje figura
     private void SpawingAllPieces()
    {
        chessPieces = new SahovskeFigure[TILE_COUNT_X, TILE_COUNT_Y];

        int whiteTeam=0,  blackTeam=1;

        chessPieces[0, 0] = SpawingSinglePiece(TipSahovskiFigura.Rook, whiteTeam);
        chessPieces[1, 0] = SpawingSinglePiece(TipSahovskiFigura.Knight, whiteTeam);
        chessPieces[2, 0] = SpawingSinglePiece(TipSahovskiFigura.Bishop, whiteTeam);
        chessPieces[3, 0] = SpawingSinglePiece(TipSahovskiFigura.Queen, whiteTeam);
        chessPieces[4, 0] = SpawingSinglePiece(TipSahovskiFigura.King, whiteTeam);
        chessPieces[5, 0] = SpawingSinglePiece(TipSahovskiFigura.Bishop, whiteTeam);
        chessPieces[6, 0] = SpawingSinglePiece(TipSahovskiFigura.Knight, whiteTeam);
        chessPieces[7, 0] = SpawingSinglePiece(TipSahovskiFigura.Rook, whiteTeam);

        for(int i=0; i<TILE_COUNT_X; i++)
        {
            chessPieces[i, 1] = SpawingSinglePiece(TipSahovskiFigura.Pawn, whiteTeam);

        }

        chessPieces[0, 7] = SpawingSinglePiece(TipSahovskiFigura.Rook, blackTeam);
        chessPieces[1, 7] = SpawingSinglePiece(TipSahovskiFigura.Knight,blackTeam);
        chessPieces[2, 7] = SpawingSinglePiece(TipSahovskiFigura.Bishop,blackTeam);
        chessPieces[3, 7] = SpawingSinglePiece(TipSahovskiFigura.Queen,blackTeam);
        chessPieces[4, 7] = SpawingSinglePiece(TipSahovskiFigura.King, blackTeam);
        chessPieces[5, 7] = SpawingSinglePiece(TipSahovskiFigura.Bishop,blackTeam);
        chessPieces[6, 7] = SpawingSinglePiece(TipSahovskiFigura.Knight,blackTeam);
        chessPieces[7, 7] = SpawingSinglePiece(TipSahovskiFigura.Rook,blackTeam);

        for (int i = 0; i < TILE_COUNT_X; i++)
        {
            chessPieces[i, 6] = SpawingSinglePiece(TipSahovskiFigura.Pawn, blackTeam);

        }
    } 
    private SahovskeFigure SpawingSinglePiece(TipSahovskiFigura tip, int team)
    {
        SahovskeFigure sf = Instantiate(prefabs[(int) tip - 1],transform).GetComponent<SahovskeFigure>();
        sf.tip = tip;
        sf.team= team;
        sf.GetComponent<MeshRenderer>().material = teamMaterial[team];

        return sf;
    }

    // Pozicioniranje figura

    private void PositionAllPieces()
    {
        for (int i = 0;i < TILE_COUNT_X;i++)
        {
            for (int j= 0; j < TILE_COUNT_Y; j++)
            {
                if (chessPieces[i, j] != null)
                    PositionSinglePiece(i, j, true);
            }
        }

    }

  private void PositionSinglePiece(int x, int y, bool force= false)
    {
        chessPieces[x, y].currentX = x;
        chessPieces[x,y].currentY = y;
        chessPieces[x, y].transform.position = GetTileCenter(x, y);
    }
   
    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3 (tileSize/2,yOffset,tileSize/2);
    }
}
