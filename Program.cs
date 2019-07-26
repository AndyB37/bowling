using System;

namespace BowlingScore
{
    public interface ISimpleBowlingGame
    {
        // Called when a player completes a frame.
        // This method will be called 10 times for a bowling game.
        // The throws parameter provides the number of pins knocked down on each throw in the frame being recorded.
        // The 10th frame may have 3 values.
        void RecordFrame(params int[] throws);

        // Called at the end of the game to get the final score.
        int Score { get; }
    }
    class SimpleBowlingGame : ISimpleBowlingGame
    {
        static void Main(string[] args)
        {
            // A couple of sample games for testing purposes
            SimpleBowlingGame game = new SimpleBowlingGame();
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10);
            game.RecordFrame(10, 10, 10);
            Console.WriteLine("Perfect Game: " + game.Score + "!");

            SimpleBowlingGame game2 = new SimpleBowlingGame();
            game2.RecordFrame(10);
            game2.RecordFrame(9, 1);
            game2.RecordFrame(1, 5);
            game2.RecordFrame(7, 2);
            game2.RecordFrame(7, 3);
            game2.RecordFrame(2, 5);
            game2.RecordFrame(1, 7);
            game2.RecordFrame(6, 1);
            game2.RecordFrame(2, 7);
            game2.RecordFrame(9, 1, 1);
            Console.WriteLine("Not-so-perfect Game: " + game2.Score);
        }
        public void RecordFrame(params int[] throws)
        {
            for (int t = 0; t < throws.Length; t++)
            {
                Frames[CurrentFrame, t] = throws[t];
            }
            UpdateScore();
            CurrentFrame++;
        }
        public int Score { get;  set; }
        private int[,] Frames = new int[11, 3];
        private int CurrentFrame = 1;
        private void UpdateScore()
        {
            Score = 0;
            // Loop through Frames and update score according to standard bowling rules
            for (int f = 1; f <= CurrentFrame; f++)
            {
                int frameTotal = Frames[f, 0] + Frames[f, 1];
                // If the total for this frame is 10 and the throws for the next frame have been recorded, try to get the score for the current frame using the following frames
                if (frameTotal == 10 && f < CurrentFrame)
                {
                    int nextThrow = Frames[f + 1, 0];
                    // If a strike was thrown in this frame, get the next two throws and add them
                    if (Frames[f,0] == 10)
                    {
                        int thirdThrow;
                        // If the next throw was a strike and a following frame is on record, get the first throw from the following frame
                        if (nextThrow == 10 && f + 1 < CurrentFrame)
                        {
                            thirdThrow = Frames[f + 2, 0];
                            Score += 20 + thirdThrow;
                        }
                        else if (nextThrow < 10 || f + 1 == 10) // Otherwise: the next throw wasn't a strike or it's the 9th frame, so get the second throw from the next frame
                        {
                            thirdThrow = Frames[f + 1, 1];
                            Score += 10 + nextThrow + thirdThrow;
                        }
                    }
                    else // Otherwise: the current frame was a spare, so add 10 plus the next throw
                    {
                        Score += 10 + nextThrow;
                    }
                }
                else if (f == 10) // It's the 10th frame, so add the score for all throws
                {
                    Score += Frames[f, 0] + Frames[f, 1] + Frames[f, 2];
                }
                else if (frameTotal < 10) // Otherwise: not a strike or a spare and not 10th frame, so just add the score from this frame
                {
                    Score += Frames[f, 0] + Frames[f, 1];
                }
            }
        }
    }
}
