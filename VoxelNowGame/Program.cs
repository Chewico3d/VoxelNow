using VoxelNowEngine;

public static class Program {
    public static Game mainGame;

    public static void Main() {
        mainGame = new Game(new ExplorationWorld());
        mainGame.Run();

    }
}
