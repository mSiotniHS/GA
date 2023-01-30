namespace GA.BaseProblem;

public interface ICoder<TFrom, TTo>
{
	public TTo Encode(TFrom from);
	public TFrom Decode(TTo to);
}