using ChessGameApplication.Game.Figures;

namespace ChessGameApplication.Game.BoardServices;

public class PiecePlacementService
{
    private const int BoardSize = 8;
    private readonly Board _board;

    public PiecePlacementService(Board board)
    {
        _board = board;
    }

    public void SetStartingPieces()
    {
        var startingPieces = new (Type pieceType, int x, int y)[]
        {
            (typeof(Rook), 0, 0), (typeof(Knight), 1, 0), (typeof(Bishop), 2, 0),
            (typeof(Queen), 3, 0), (typeof(King), 4, 0),
            (typeof(Bishop), 5, 0), (typeof(Knight), 6, 0), (typeof(Rook), 7, 0)
        };

        foreach (var (type, x, y) in startingPieces)
            _board.PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.Black, new Position(x, y))!, new Position(x, y));

        foreach (var (type, x, y) in startingPieces)
            _board.PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.White, new Position(x, 7))!, new Position(x, 7));

        for (int x = 0; x < BoardSize; x++)
        {
            _board.PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1)), new Position(x, 1));
            _board.PlacePiece(new Pawn(PieceColor.White, new Position(x, 6)), new Position(x, 6));
        }
    }

    /// <summary>
    /// Sets randomized starting positions of pieces in accordance with the rules of Chess960.
    /// </summary>
    public void SetStartingPieces960()
    {
        Random r = new Random();

        //create list tracking all positions to ensure that two pieces are not assigned to the same spot
        List<int> unusedPositions = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

        //Assign king location -- King must have at least one open square to either side of him
        //Track king position, I will need it for the rooks
        int kingIndex = r.Next(1, unusedPositions.Count - 1);
        int kingPosition = unusedPositions[kingIndex];

        _board.PlacePiece(new King(PieceColor.Black, new Position(kingPosition, 0)), new Position(kingPosition, 0));
        _board.PlacePiece(new King(PieceColor.White, new Position(kingPosition, 7)), new Position(kingPosition, 7));


        //Assign first rook position -- random range for first rook must include only the squares to the right of the king
        int rookOnePosition = unusedPositions[r.Next(kingIndex + 1, unusedPositions.Count)];
        _board.PlacePiece(new Rook(PieceColor.Black, new Position(rookOnePosition, 0)), new Position(rookOnePosition, 0));
        _board.PlacePiece(new Rook(PieceColor.White, new Position(rookOnePosition, 7)), new Position(rookOnePosition, 7));

        //Assign second rook position -- random range for second rook must include only the squares to the left of the king
        int rookTwoPosition = unusedPositions[r.Next(0, kingIndex)];
        _board.PlacePiece(new Rook(PieceColor.Black, new Position(rookTwoPosition, 0)), new Position(rookTwoPosition, 0));
        _board.PlacePiece(new Rook(PieceColor.White, new Position(rookTwoPosition, 7)), new Position(rookTwoPosition, 7));

        //Remove King position and the position of both rooks from the list of unusedPositions
        unusedPositions.Remove(kingPosition);
        unusedPositions.Remove(rookOnePosition);
        unusedPositions.Remove(rookTwoPosition);

        //Assign bishop one position
            //Track if bishop is on a black or white square
        int bishopOnePosition = unusedPositions[r.Next(0, unusedPositions.Count)];
        _board.PlacePiece(new Bishop(PieceColor.Black, new Position(bishopOnePosition, 0)), new Position(bishopOnePosition, 0));
        _board.PlacePiece(new Bishop(PieceColor.White, new Position(bishopOnePosition, 7)), new Position(bishopOnePosition, 7));

        unusedPositions.Remove(bishopOnePosition);


        //Assign bishop two position -- limit bishop two to only black or white (opposite of what bishop one is on)
        bool isValidSquare = false;
        int bishopTwoPosition = -1;

        while(! isValidSquare)
        {
            bishopTwoPosition = unusedPositions[r.Next(0, unusedPositions.Count)];
            if((bishopOnePosition % 2) != (bishopTwoPosition % 2))
            {
                isValidSquare = true;
            }
        }

        _board.PlacePiece(new Bishop(PieceColor.Black, new Position(bishopTwoPosition, 0)), new Position(bishopTwoPosition, 0));
        _board.PlacePiece(new Bishop(PieceColor.White, new Position(bishopTwoPosition, 7)), new Position(bishopTwoPosition, 7));

        unusedPositions.Remove(bishopTwoPosition);

        //Assign Queen, Knight One and Knight Two positions at random from the remaining three squares

        int QueenPosition = unusedPositions[r.Next(0, unusedPositions.Count)];

        _board.PlacePiece(new Queen(PieceColor.Black, new Position(QueenPosition, 0)), new Position(QueenPosition, 0));
        _board.PlacePiece(new Queen(PieceColor.White, new Position(QueenPosition, 7)), new Position(QueenPosition, 7));

        unusedPositions.Remove(QueenPosition);

        _board.PlacePiece(new Knight(PieceColor.Black, new Position(unusedPositions[0], 0)), new Position(unusedPositions[0], 0));
        _board.PlacePiece(new Knight(PieceColor.Black, new Position(unusedPositions[1], 0)), new Position(unusedPositions[1], 0));
        _board.PlacePiece(new Knight(PieceColor.White, new Position(unusedPositions[0], 7)), new Position(unusedPositions[0], 7));
        _board.PlacePiece(new Knight(PieceColor.White, new Position(unusedPositions[1], 7)), new Position(unusedPositions[1], 7));


        for (int x = 0; x < BoardSize; x++)
        {
            _board.PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1)), new Position(x, 1));
            _board.PlacePiece(new Pawn(PieceColor.White, new Position(x, 6)), new Position(x, 6));
        }
    }

    public void ClearBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                _board.SetPieceAt(new Position(row, col), null);
            }
        }
    }

    public void PlacePiece(Piece? piece, Position position)
    {
        _board.SetPieceAt(position, piece);
        piece?.SetPosition(position);
    }

}
