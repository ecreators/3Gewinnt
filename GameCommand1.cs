public abstract class GameCommand<T>
{
    public abstract void Execute(T copyCell);
}