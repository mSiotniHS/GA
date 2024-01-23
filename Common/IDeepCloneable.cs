namespace Common;

public interface IDeepCloneable<out T>
{
    public T DeepClone();
}
