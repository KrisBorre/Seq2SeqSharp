﻿using AdvUtils;
using Seq2SeqSharp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorSharp;

namespace Seq2SeqSharp.Utils
{
    public class MaskUtils
    {

        public static IWeightTensor BuildPadSelfMask(IComputeGraph g, int paddedLength, List<int> originalLengths, int deviceId)
        {
            float[] buf = new float[originalLengths.Count * paddedLength * paddedLength];
            Array.Fill(buf, -99999999.0f);

            for (int k = 0; k < originalLengths.Count; k++)
            {
                for (int i = 0; i < originalLengths[k]; i++)
                {
                    for (int j = 0; j < originalLengths[k]; j++)
                    {
                        buf[k * (paddedLength * paddedLength) + i * paddedLength + j] = 0.0f;
                    }
                }
            }

            WeightTensor tensor = new WeightTensor(new long[] { originalLengths.Count, paddedLength, paddedLength }, 0.0f, deviceId, $"TriMask_{deviceId}", isTrainable: false);
            tensor.SetWeightArray(buf);

            return tensor;
        }

        public static IWeightTensor BuildPadSelfTriMask(IComputeGraph g, int paddedLength, List<int> originalLengths, int deviceId)
        {
            float[] buf = new float[originalLengths.Count * paddedLength * paddedLength];
            Array.Fill(buf, -99999999.0f);


            for (int k = 0; k < originalLengths.Count; k++)
            {
                int offset_k = k * (paddedLength * paddedLength);
                for (int i = 0; i < originalLengths[k]; i++)
                {
                    int offset_k_i = offset_k + i * paddedLength;
                    for (int j = 0; j < originalLengths[k]; j++)
                    {
                        if (i >= j)
                        {
                            buf[offset_k_i + j] = 0.0f;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            WeightTensor tensor = new WeightTensor(new long[] { originalLengths.Count, paddedLength, paddedLength }, deviceId, $"TriMask_{deviceId}", isTrainable: false);
            tensor.SetWeightArray(buf);

            return tensor;
        }



        public static IWeightTensor BuildSrcTgtMask(IComputeGraph g, int srcPaddedLength, int tgtPaddedLength, List<int> tgtOriginalLengths, List<int> srcOriginalLengths, int deviceId)
        {
            float[] buf = new float[tgtOriginalLengths.Count * tgtPaddedLength * srcPaddedLength];
            Array.Fill(buf, -99999999.0f);


            for (int k = 0; k < tgtOriginalLengths.Count; k++) // batch size
            {
                int offset_k = k * (tgtPaddedLength * srcPaddedLength);
                for (int i = 0; i < tgtOriginalLengths[k]; i++)
                {
                    int offset_k_i = offset_k + i * srcPaddedLength;
                    for (int j = 0; j < srcOriginalLengths[k]; j++)
                    {
                        buf[offset_k_i + j] = 0.0f;
                    }
                }
            }

            WeightTensor tensor = new WeightTensor(new long[] { tgtOriginalLengths.Count, tgtPaddedLength, srcPaddedLength }, deviceId, $"SrcTgtMask_{deviceId}", isTrainable: false);
            tensor.SetWeightArray(buf);

            return tensor;
        }
    }
}
