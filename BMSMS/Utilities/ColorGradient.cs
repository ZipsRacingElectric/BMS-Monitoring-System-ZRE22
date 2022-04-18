using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BMSMS.Utilities
{
    public class ColorGradient
    {
        private double lowerLimit;
        private double upperLimit;
        private double adjustedRange;
        Color start;
        Color end;
        int steps;
        List<Color> gradient;

        public ColorGradient(double lower, double upper, Color startColor, Color endColor, int numSteps, bool accurate)
        {
            lowerLimit = lower;
            upperLimit = upper;
            start = startColor;
            end = endColor;
            steps = numSteps;
            adjustedRange = (upperLimit - lowerLimit);

            if (accurate)
            {
                gradient = GetAccurateGradients().ToList();
            }
            else
            {
                gradient = GetGradients().ToList();
            }
        }
        public Windows.UI.Color getCurrentColor(double currentValue)
        {
            if (currentValue <= lowerLimit)
            {
                return Windows.UI.Color.FromArgb(start.A, start.R, start.G, start.B);
            }
            if(currentValue >= upperLimit)
            {
                return Windows.UI.Color.FromArgb(end.A, end.R, end.G, end.B);
            }

            double percent = (currentValue - lowerLimit) / adjustedRange;
            Color curr = gradient.ElementAt((int)(percent * steps));
            return Windows.UI.Color.FromArgb(curr.A, curr.R, curr.G, curr.B);
        }

        private IEnumerable<Color> GetGradients()
        {
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return Color.FromArgb(start.A + (stepA * i),
                                            start.R + (stepR * i),
                                            start.G + (stepG * i),
                                            start.B + (stepB * i));
            }
        }

        private IEnumerable<Color> GetAccurateGradients()
        {
            double stepA = ((double)(end.A - start.A) / (steps - 1));
            double stepR = ((double)(end.R - start.R) / (steps - 1));
            double stepG = ((double)(end.G - start.G) / (steps - 1));
            double stepB = ((double)(end.B - start.B) / (steps - 1));

            double actualA;
            double actualR;
            double actualG;
            double actualB;

            for (int i = 0; i < steps; i++)
            {
                actualA = start.A + (stepA * i);
                actualR = start.R + (stepR * i);
                actualG = start.G + (stepG * i);
                actualB = start.B + (stepB * i);
                yield return Color.FromArgb((int)actualA, (int)actualR, (int)actualG, (int)actualB);
            }
        }

    }
}
