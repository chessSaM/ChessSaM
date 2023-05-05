
using UnityEngine;
public enum TipSahovskiFigura
{
    None=0,
    Pawn=1,
    Rook=2,
    Knight=3,
    Bishop=4,
    Queen=5,
    King=6
}

public class SahovskeFigure : MonoBehaviour
{
    public int team;// meant as color -> white or black
    public int currentX;
    public int currentY;
    public TipSahovskiFigura tip;
    private Vector3 desiredPosition;
    private Vector3 desiredScale;

}
