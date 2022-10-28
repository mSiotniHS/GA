namespace EA.BaseProblem;

public interface ICriterion<in TBase> { public int Calculate(TBase bas); }
