public static class ScoreManager {

    public static int? Score { get; private set; }
    public static int? BestScore { get; private set; }

    static ScoreManager() {
        Score = null;
        BestScore = null;
    }

    public static void SetScore(int score) {
        Score = score;
        if (!BestScore.HasValue || score > BestScore.Value) {
            BestScore = score;
        }
    }
}