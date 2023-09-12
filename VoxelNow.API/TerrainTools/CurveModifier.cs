using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelNow.Core.TerrainTools {
    public class CurveModifier {

        public readonly List<Modifier> modifiers;

        public CurveModifier(Modifier[] modifiers) {
            this.modifiers = new List<Modifier>(modifiers);
        }

        public void AddModifier(Modifier modifier) => modifiers.Add(modifier);

        public CurveModifier() {
            this.modifiers = new List<Modifier>();
        }

        public float GetLinearPoint(float position) {

            int minModID = 0;
            int maxModID = 0;

            float minModPos = float.MinValue;
            float maxModPos = float.MaxValue;

            float minAbsPos = float.MinValue;
            float maxAbsPos = float.MaxValue;

            for(int modID = 0; modID < modifiers.Count; modID++) {
                if (modifiers[modID].position > position && modifiers[modID].position < maxModPos) {
                    maxModID = modID;
                    maxModPos = modifiers[modID].position;
                    if(maxModPos == float.MaxValue)
                        maxAbsPos = maxModPos;
                    else if(maxModPos > maxAbsPos)
                        maxAbsPos = maxModPos;
                }

                if (modifiers[modID].position < position && modifiers[modID].position > minModPos) {
                    minModID = modID;
                    minModPos = modifiers[modID].position;
                    if (minAbsPos == float.MinValue)
                        minAbsPos = minModPos;
                    else if (minModPos < minAbsPos)
                        minAbsPos = minModPos;
                }

            }

            position = (position < minAbsPos) ? minAbsPos : (position > maxAbsPos) ? maxAbsPos : position;
            float distanceFromMinToMax = modifiers[maxModID].position - modifiers[minModID].position;
            float relativePositionToMods = (position - modifiers[minModID].position) / distanceFromMinToMax;
            float height = modifiers[minModID].height * (1 - relativePositionToMods) + modifiers[maxModID].height * relativePositionToMods;

            //if (height < 0.1f && position > 0.4f)
            //    Console.WriteLine(minModID + " " + maxModID + " " + position);
            return height;

        }
        float SmothValue(float value) {

            return -(MathF.Cos(MathF.PI * value) - 1) / 2;
        }

        public float GetSmothPoint(float position) {
            int minModID = -1;
            int maxModID = -1;

            float minModPos = float.MinValue;
            float maxModPos = float.MaxValue;

            int minAbsID = 0;
            int maxAbsID = 0;
            float minAbsPos = float.MinValue;
            float maxAbsPos = float.MaxValue;

            for (int modID = 0; modID < modifiers.Count; modID++) {
                if (modifiers[modID].position > position && modifiers[modID].position < maxModPos) {
                    maxModID = modID;
                    maxModPos = modifiers[modID].position;
                }

                if (maxModPos == float.MaxValue) {
                    minAbsID = modID;
                    maxAbsPos = maxModPos;
                }
                else if (maxModPos > maxAbsPos) {
                    minAbsID = modID;
                    maxAbsPos = maxModPos;
                }

                if (modifiers[modID].position <= position && modifiers[modID].position > minModPos) {
                    minModID = modID;
                    minModPos = modifiers[modID].position;
                }

                if (minAbsPos == float.MinValue) {
                    maxAbsID = modID;
                    minAbsPos = minModPos;
                }
                else if (minModPos < minAbsPos) {
                    maxAbsID = modID;
                    minAbsPos = minModPos;
                }
            }

            if (minModID == -1)
                minModID = minAbsID;
            if (maxModID == -1)
                maxModID = maxAbsID;

            if (minModID == maxModID)
                return modifiers[minModID].height;

            position = (position < modifiers[minModID].position) ? modifiers[minModID].position
                : (position > modifiers[maxModID].position) ? modifiers[maxModID].position : position;

            float distanceFromMinToMax = modifiers[maxModID].position - modifiers[minModID].position;
            float relativePositionToMods = (position - modifiers[minModID].position) / distanceFromMinToMax;
            relativePositionToMods = SmothValue(relativePositionToMods);

            float height = modifiers[minModID].height * (1 - relativePositionToMods) + modifiers[maxModID].height * relativePositionToMods;

            //if (height < 0.1f && position > 0.4f)
            //    Console.WriteLine(minModID + " " + maxModID + " " + position);
            return height;
        }

    }

    public class Modifier {

        public float position;
        public float height;

        public Modifier(float position, float height) {
            this.position = position;
            this.height = height;
        }

        public Modifier() { }


    }

}
