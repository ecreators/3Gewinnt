public abstract class GameCommand<T>
{
    public abstract void Execute(T parameter);

    protected static void Delete(ZelleComponent zelle)
    {
        UnityEngine.Object.Destroy(zelle.gameObject);
    }
}