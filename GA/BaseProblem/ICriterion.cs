﻿namespace GA.BaseProblem;

public interface ICriterion<in TBaseType>
{
	public int Calculate(TBaseType bas);
}
