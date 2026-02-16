
using ChessGameApplication.Game;
using ChessGameApplication.Game.BoardServices;
using ChessGameApplication.Game.Figures;

namespace ChessGame.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void KingIsBetweenRooks()
        {
            Board board = new Board();
            PiecePlacementService placement = new PiecePlacementService(board);

            placement.SetStartingPieces960();

            IEnumerable<Piece> pieces = board.GetAllPieces();

            //Console.WriteLine(pieces);

            int wRooksGathered = 0;
            int bRooksGathered = 0;
            Position wRook1 = new Position();
            Position wRook2 = new Position();
            Position wKing = new Position();
            Position bRook1 = new Position();
            Position bRook2 = new Position();
            Position bKing = new Position();

            foreach (Piece piece in pieces)
            {
                if(piece.Color == PieceColor.White)
                {
                    if(piece.GetType() == typeof(Rook))
                    {
                        if(wRooksGathered == 0)
                        {
                            wRook1 = piece.Position;
                            wRooksGathered++;
                        } else
                        {
                            wRook2 = piece.Position;
                            wRooksGathered++;
                        }
                    }
                    else if(piece.GetType() == typeof(King))
                    {
                        wKing = piece.Position;
                    }
                }

                if (piece.Color == PieceColor.Black)
                {
                    if (piece.GetType() == typeof(Rook))
                    {
                        if (bRooksGathered == 0)
                        {
                            bRook1 = piece.Position;
                            bRooksGathered++;
                        }
                        else
                        {
                            bRook2 = piece.Position;
                            bRooksGathered++;
                        }
                    }
                    else if (piece.GetType() == typeof(King))
                    {
                        bKing = piece.Position;
                    }
                }
            }

            //Assert for white pieces
            if(wRook1.Row > wRook2.Row)
            {
                Assert.True((wRook1.Row > wKing.Row) && (wKing.Row > wRook2.Row));
            }
            else
            {
                Assert.True((wRook2.Row > wKing.Row) && (wKing.Row > wRook1.Row));
            }

            //Assert for black pieces
            if (bRook1.Row > bRook2.Row)
            {
                Assert.True((bRook1.Row > bKing.Row) && (bKing.Row > bRook2.Row));
            }
            else
            {
                Assert.True((bRook2.Row > bKing.Row) && (bKing.Row > bRook1.Row));
            }
        }
    }
}