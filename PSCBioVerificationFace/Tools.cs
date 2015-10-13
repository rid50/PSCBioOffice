using System;
using System.Collections.Generic;
using System.Text;
using Neurotec.Biometrics;

namespace PSCBioVerificationFace
{
	public class Tools
	{
		private static NLExtractor _extractor = null;
		public static NLExtractor Extractor
		{
			get
			{
				if (_extractor == null)
				{
					_extractor = new NLExtractor();
				}
				return _extractor;
			}
		}

		public static NleTemplateSize EnrollTemplateSize
		{
			get 
			{
				try { return Settings.Default.EnrollTemplateSize; }
				catch { return NleTemplateSize.Large; }
			}
		}

		public static NleTemplateSize IdentificationTemplateSize
		{
			get 
			{
				try { return Settings.Default.IdentificationTemplateSize; }
				catch { return NleTemplateSize.Large; }
			}
		}

		public static int LiveEnrolFrameCount
		{
			get 
			{
				try { return Settings.Default.ExtractorLiveEnrollFrameCount; }
				catch { return 10; }
			}
		}

		public static void SaveExtractorSettings(NleTemplateSize enrollmentTemplateSize, NleTemplateSize identificationTemplateSize, bool detectAllFeaturesNonLive)
		{
			Settings settings = Settings.Default;
			NLExtractor extractor = Extractor;
			settings.ExtractorFaceConfidence = extractor.FaceConfidenceThreshold;
			settings.ExtractorFaceQuality = extractor.FaceQualityThreshold;
			settings.ExtractorMinIod = extractor.MinIod;
			settings.ExtractorMaxIod = extractor.MaxIod;
			settings.ExtractorMaxRollAngleDeviation = extractor.MaxRollAngleDeviation;
			settings.ExtractorFavorLargestFace = extractor.FavorLargestFace;
			settings.ExtractorUseLivenessCheck = extractor.UseLivenessCheck;
			settings.ExtractorLivenessCheckThreshold = extractor.LivenessThreshold;
			settings.EnrollTemplateSize = enrollmentTemplateSize;
			settings.IdentificationTemplateSize = identificationTemplateSize;
			settings.ExtractorLiveEnrollFrameCount = extractor.MaxStreamDurationInFrames;
			settings.ExtractorMaxYawAngle = extractor.MaxYawAngleDeviation;
			settings.ExtractorDetectAllFeaturesNonLive = detectAllFeaturesNonLive;

			settings.Save();
		}

		public static void LoadExtractorSettings()
		{
			NleTemplateSize enrollmentTemplateSize, identificationTemplateSize;
			bool detectAllFeatures;
			LoadExtractorSettings(out enrollmentTemplateSize, out identificationTemplateSize, out detectAllFeatures);
		}

		public static void LoadExtractorSettings(out NleTemplateSize enrollmentTemplateSize, out NleTemplateSize identificationTemplateSize, out bool detectAllFacialFeaturesNonLive)
		{
			NLExtractor extractor = Extractor;
			Settings settings = Settings.Default;
			extractor.Reset();
			try { extractor.FaceConfidenceThreshold = settings.ExtractorFaceConfidence; }
			catch { }
			try { extractor.FaceQualityThreshold = settings.ExtractorFaceQuality; }
			catch { }
			try { extractor.MaxIod = settings.ExtractorMaxIod; }
			catch { }
			try { extractor.MaxRollAngleDeviation = settings.ExtractorMaxRollAngleDeviation; }
			catch { }
			try { extractor.MinIod = settings.ExtractorMinIod; }
			catch { }
			try { extractor.FavorLargestFace = settings.ExtractorFavorLargestFace; }
			catch { }
			try { extractor.UseLivenessCheck = settings.ExtractorUseLivenessCheck; }
			catch { }
			try { extractor.LivenessThreshold = settings.ExtractorLivenessCheckThreshold; }
			catch { }
			try { enrollmentTemplateSize = settings.EnrollTemplateSize; }
			catch { enrollmentTemplateSize = NleTemplateSize.Large; }
			try { identificationTemplateSize = settings.IdentificationTemplateSize; }
			catch { identificationTemplateSize = NleTemplateSize.Large; }
			try { extractor.MaxStreamDurationInFrames = settings.ExtractorLiveEnrollFrameCount; }
			catch { extractor.MaxStreamDurationInFrames = 10; }
			try { extractor.MaxYawAngleDeviation = settings.ExtractorMaxYawAngle; }
			catch { }
			try { detectAllFacialFeaturesNonLive = settings.ExtractorDetectAllFeaturesNonLive; }
			catch { detectAllFacialFeaturesNonLive = true; }
		}

		public static void AddRemoteMatcherParametersFromApplicationSettings(Neurotec.Cluster.MatchingParameters parameters, bool useCompatibilitySettings)
		{
			Settings settings = Settings.Default;
			if (useCompatibilitySettings)
			{
#pragma warning disable
				try { parameters.AddParameter(NMatcher.PartFaces + NlsMatcher.PartNlm, NLMatcher.ParameterMatchingSpeed, (uint)settings.MatcherSpeed); }
				catch { }

				try
				{
					using (NMatcher matcher = new NMatcher())
					{
						matcher.FacesMatchingThresholdNew = settings.MatcherThreshold; // converting matching threshold value to old-style value
						parameters.AddParameter(NMatcher.PartNone, NMatcher.ParameterFacesMatchingThreshold, matcher.FacesMatchingThreshold);
					}
				}
				catch { }
#pragma warning restore
			}
			else
			{
				try { parameters.AddParameter(NMatcher.PartNone, NMatcher.ParameterFacesMatchingSpeed, (uint)settings.MatcherSpeed); }
				catch { }

				try { parameters.AddParameter(NMatcher.PartNone, NMatcher.ParameterFacesMatchingThresholdNew, settings.MatcherThreshold); }
				catch { }
			}
		}

		public static void SaveMatcherSettings(Neurotec.Biometrics.NMatcher matcher)
		{
			Settings settings = Settings.Default;
			settings.MatcherSpeed = matcher.FacesMatchingSpeed;
			settings.MatcherThreshold = matcher.FacesMatchingThresholdNew;
			settings.Save();
		}

		public static void LoadMatcherSettings(Neurotec.Biometrics.NMatcher matcher)
		{
			Settings settings = Settings.Default;
			try { matcher.FacesMatchingSpeed = settings.MatcherSpeed; }
			catch { }
			try { matcher.FacesMatchingThresholdNew = settings.MatcherThreshold; }
			catch { }
		}
	}
}
