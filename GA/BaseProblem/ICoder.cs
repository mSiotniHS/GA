using GA.Core;

namespace GA.BaseProblem;

public interface IEncoder<in TOriginal, out TEncoding>
{
	TEncoding Encode(TOriginal original);
}

public interface IDecoder<out TOriginal, in TEncoding>
{
	TOriginal Decode(TEncoding encoding);
}

//public interface ICoder<TOriginal, TEncoding> : IEncoder<TOriginal, TEncoding>, IDecoder<TOriginal, TEncoding>
//{
//}

public interface IGaEncoder<in TBaseType> : IEncoder<TBaseType, Genotype>
{
}

public interface IGaDecoder<out TBaseType> : IDecoder<TBaseType, Genotype>
{
}

public interface IGaCoder<TBaseType> : IGaEncoder<TBaseType>, IGaDecoder<TBaseType>
{
}
