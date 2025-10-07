using System;

public class RealRandom : Random
{
	// Last results for the range to avoid giving the same number twice
	private int lastValue;
	private int lastValueCount;
	private int maxLastValue;

	public RealRandom(int maxConsecutiveSameResult = 0) {
		lastValue = 0;
		lastValueCount = 0;
		maxLastValue = maxConsecutiveSameResult;
	}

	public double NextDoubleInc() {
		double dbl = NextDouble();
		// Remap to 0.0 - 1.0
		dbl /= 0.99999999999999978;
		return dbl;
	}

	public float Range(float minInc, float maxInc) {
		return (((float)NextDoubleInc()) * (maxInc - minInc)) + minInc;
	}

	public double Range(double minInc, double maxInc) {
		return (NextDoubleInc() * (maxInc - minInc)) + minInc;
	}

	public int Range(int minInc, int maxExc) {
		if (minInc == maxExc || minInc == maxExc-1)
			return minInc;
		int val = 0;
		do {
			val = Next(minInc, maxExc);
		} while (lastValueCount >= maxLastValue && val == lastValue);
		if (lastValue == val)
			lastValueCount++;
		else
			lastValueCount = 0;
		lastValue = val;
		return val;
	}
}
