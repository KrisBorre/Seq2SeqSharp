﻿// Copyright (c) Zhongkai Fu. All rights reserved.
// https://github.com/zhongkaifu/Seq2SeqSharp
//
// This file is part of Seq2SeqSharp.
//
// Seq2SeqSharp is licensed under the BSD-3-Clause license found in the LICENSE file in the root directory of this source tree.
//
// Seq2SeqSharp is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the BSD-3-Clause License for more details.

using AdvUtils;
using Seq2SeqSharp.Utils;
using Seq2SeqSharp.Applications;
using Seq2SeqSharp.Enums;
using System.ComponentModel.DataAnnotations;

namespace Seq2SeqSharp.Applications
{
    public class Seq2SeqOptions : Options
    {
        [Arg("The network depth in decoder.", nameof(DecoderLayerDepth))]
        [Range(1, 9999)]
        public int DecoderLayerDepth = 1;

        [Arg("Starting Learning rate factor for decoders", nameof(DecoderStartLearningRateFactor))]
        [Range(0.0f, 1.0f)]
        public float DecoderStartLearningRateFactor = 1.0f;

        [Arg("Decoder type: None, AttentionLSTM, Transformer, GPTDecoder", nameof(DecoderType))]
        [RegularExpression("None|AttentionLSTM|Transformer|GPTDecoder")]
        public DecoderTypeEnums DecoderType = DecoderTypeEnums.Transformer;

        [Arg("Apply coverage model in decoder", nameof(EnableCoverageModel))]
        public bool EnableCoverageModel = false;

        [Arg("It indicates if the decoder is trainable", nameof(IsDecoderTrainable))]
        public bool IsDecoderTrainable = true;

        [Arg("It indicates if the src embedding is trainable", nameof(IsSrcEmbeddingTrainable))]
        public bool IsSrcEmbeddingTrainable = true;
        [Arg("It indicates if the tgt embedding is trainable", nameof(IsTgtEmbeddingTrainable))]
        public bool IsTgtEmbeddingTrainable = true;

        [Arg("Maximum src sentence length in valid set", nameof(MaxValidSrcSentLength))]
        [Range(1, 1280000)]
        public int MaxValidSrcSentLength = 32;

        [Arg("Maximum tgt sentence length in valid set", nameof(MaxValidTgtSentLength))]
        [Range(1, 1280000)]
        public int MaxValidTgtSentLength = 32;

        [Arg("Maximum src sentence length in training and test set", nameof(MaxSrcSentLength))]
        [Range(1, 1280000)]
        public int MaxSrcSentLength = 110;

        [Arg("Maximum tgt sentence length in training and test set", nameof(MaxTgtSentLength))]
        [Range(1, 1280000)]
        public int MaxTgtSentLength = 110;

        [Arg("The metric for sequence generation task. It supports BLEU and RougeL", nameof(SeqGenerationMetric))]
        [RegularExpression("BLEU|RougeL")]
        public string SeqGenerationMetric = "BLEU";

        [Arg("Sharing embeddings between source side and target side", nameof(SharedEmbeddings))]
        public bool SharedEmbeddings = false;

        [Arg("The embedding dim in target side", nameof(TgtEmbeddingDim))]
        [Range(1, 102400)]
        public int TgtEmbeddingDim = 128;

        [Arg("It indicates if pointer generator is enabled or not for seq2seq tasks. It requires shared vocabulary between source and target", nameof(PointerGenerator))]
        public bool PointerGenerator = false;

        [Arg("The file path that outputs alignment to source sequence", nameof(OutputAlignmentsFile))]
        public string OutputAlignmentsFile = null;

        public DecodingOptions CreateDecodingOptions()
        {
            DecodingOptions decodingOptions = new DecodingOptions();
            decodingOptions.DecodingStrategy = DecodingStrategy;
            decodingOptions.TopP = DecodingTopP;
            decodingOptions.RepeatPenalty = DecodingRepeatPenalty;
            decodingOptions.Temperature = DecodingTemperature;
            decodingOptions.BeamSearchSize = BeamSearchSize;

            decodingOptions.MaxSrcSentLength = MaxSrcSentLength;
            decodingOptions.MaxTgtSentLength = MaxTgtSentLength;

            decodingOptions.OutputAligmentsToSrc = !string.IsNullOrEmpty(OutputAlignmentsFile);

            return decodingOptions;
        }
    }
}
