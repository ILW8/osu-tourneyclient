﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Taiko.Objects
{
    public class Hit : TaikoStrongableHitObject
    {
        protected override void UpdateTypeFromSamples()
        {
            base.UpdateTypeFromSamples();

            Type = getRimSamples().Any() ? HitType.Rim : HitType.Centre;
        }

        protected override void UpdateSamplesFromType()
        {
            base.UpdateSamplesFromType();

            var rimSamples = getRimSamples();

            bool isRimType = Type == HitType.Rim;

            if (isRimType != rimSamples.Any())
            {
                if (isRimType)
                    Samples.Add(new HitSampleInfo(HitSampleInfo.HIT_CLAP));
                else
                {
                    foreach (var sample in rimSamples)
                        Samples.Remove(sample);
                }
            }
        }

        /// <summary>
        /// Returns an array of any samples which would cause this object to be a "rim" type hit.
        /// </summary>
        private HitSampleInfo[] getRimSamples() => Samples.Where(s => s.Name == HitSampleInfo.HIT_CLAP || s.Name == HitSampleInfo.HIT_WHISTLE).ToArray();

        protected override StrongNestedHitObject CreateStrongNestedHit(double startTime) => new StrongNestedHit { StartTime = startTime };

        public class StrongNestedHit : StrongNestedHitObject
        {
        }
    }
}
