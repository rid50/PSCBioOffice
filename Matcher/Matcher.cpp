//#include "stdafx.h"

//#include "Utils.h"
#include "Matcher.h"

//using namespace Neurotec;
//using namespace Neurotec::Licensing;

//using namespace std;

//BOOL APIENTRY DllMain(HANDLE hModule, 
//                    DWORD  ul_reason_for_call, 
//                    LPVOID lpReserved)
//{
//	switch( ul_reason_for_call ) {
//		case DLL_THREAD_ATTACH:
//			getLicenses();
//		case DLL_THREAD_DETACH:
//			break;
//		case DLL_PROCESS_ATTACH:
//		case DLL_PROCESS_DETACH:
//			break;
//	}
//	return TRUE;
//}


//inline void printStatusStatement(char * statusStatement) {
//	#ifdef _DEBUG
//		//sprintf_s (Matcher::statusStatement, statusStatement);
//		std::cout << statusStatement << std::endl;
//	#endif
//}

namespace Nomad
{
	namespace Bio
	{
		MatcherFacade::~MatcherFacade() {
			delete static_cast<Nomad::Bio::Matcher*>(matcherPtr);
			//std::cout << "Kuku3" << std::endl;
		};

		MatcherFacade::MatcherFacade() {
			//Nomad::Bio::Matcher* matcher = new Nomad::Bio::Matcher();
			matcherPtr = new Nomad::Bio::Matcher();
		};

		void MatcherFacade::enroll(unsigned char *record, unsigned __int32 size) {
			//static_cast<Nomad::Bio::Matcher*>(matcherPtr)->enroll(record, size);
			Matcher::enroll(record, size);
		}

		bool MatcherFacade::match(void *prescannedTemplate, unsigned __int32 prescannedTemplateSize) {
			return static_cast<Nomad::Bio::Matcher*>(matcherPtr)->match(prescannedTemplate, prescannedTemplateSize);
		}

		const NChar * components = N_T("Biometrics.FingerExtraction,Biometrics.FingerMatching");

		License::License() {
			NBool available = NFalse;

			result = NLicenseObtainComponents(N_T("/local"), N_T("5000"), components, &available);
			if (NFailed(result))
			{ 
				char *str = "Biometrics: Failed to obtain licenses";
				//printStatusStatement(str);
				throw std::runtime_error(str);
			}

			if (!available)
			{
				std::stringstream stmt;
				stmt << "Biometrics: Licenses for " << components << " are not available";
				//printStatusStatement(stmt.str().c_str());
				throw std::runtime_error(stmt.str().c_str());
			}
		}

		License::~License() {
			result = NLicenseReleaseComponents(components);
			if (NFailed(result))
			{
				//printStatusStatement("Error releasing license components");
				throw std::runtime_error("Biometrics: Error releasing license components");
			}
		}

		NSizeType	Matcher::enrolledTemplateSize = 0;
		void		*Matcher::enrolledTemplate = 0;
		HNSubject	Matcher::hProbeSubject = NULL;

		Matcher::~Matcher() {
			result = NObjectSet(NULL, (HNObject *)&hProbeSubject);
			if (NFailed(result))
			{
				//printStatusStatement("Error clearing resources");
				throw std::runtime_error("Biometrics: Error clearing resources");
			}

			result = NObjectSet(NULL, (HNObject *)&hBiometricClient);
			if (NFailed(result))
			{
				//printStatusStatement("Error clearing resources");
				throw std::runtime_error("Biometrics: Error clearing resources");
			}
		};

		Matcher::Matcher() {
			hBiometricClient = NULL;
			NInt				matchingThreshold = 48;
			NMatchingSpeed		matchingSpeed =	nmsLow;
			//NMatchingSpeed matchingSpeed = nmsHigh;

			static License license;

			// create biometric client
			result = NBiometricClientCreate(&hBiometricClient);
			if (NFailed(result))
			{
				//printStatusStatement("Biometric client creation failed:", result);
				throw std::runtime_error("Biometric client creation failed");
			}

			// set matching threshold
			result = NObjectSetPropertyP(hBiometricClient, N_T("Matching.Threshold"), N_TYPE_OF(NInt32), naNone, &matchingThreshold, sizeof(matchingThreshold), 1, NTrue);
			if (NFailed(result))
			{
				//printStatusStatement("Error setting matching threshold:", result);
				throw std::runtime_error("Biometrics: Error setting matching threshold");
			}

			// set matching speed
			result = NObjectSetPropertyP(hBiometricClient, N_T("Fingers.MatchingSpeed"), N_TYPE_OF(NMatchingSpeed), naNone, &matchingSpeed, sizeof(matchingSpeed), 1, NTrue);
			if (NFailed(result))
			{
				//printStatusStatement("Error setting matching speed:", result);
				throw std::runtime_error("Biometrics: Error setting matching speed");
			}
		}

		void Matcher::enroll(unsigned char *record, unsigned __int32 size) {
			enrolledTemplate = record;
			enrolledTemplateSize = size;

			hProbeSubject = NULL;
			NResult	result;
			NSizeType pSize = 0;
			result = NSubjectCreateFromMemory(enrolledTemplate, (NSizeType)enrolledTemplateSize, 0, &pSize, &hProbeSubject);
			if (NFailed(result))
			{
				throw std::runtime_error("Biometrics: Error creating a subject for enrolled template");
			}
		}

		bool Matcher::match(void *prescannedTemplate, unsigned __int32 prescannedTemplateSize) {
			NInt matchScore = 0;
			NBiometricStatus biometricStatus = nbsNone;

			hMatchingResults = NULL;

			//hProbeSubject = NULL;
			hGallerySubject = NULL;

			//NMMatchDetails **ppMatchDetails = NULL;
			//NSizeType packedSize2 = prescannedTemplateSize;
			//try {

			NSizeType pSize2 = 0;
			result = NSubjectCreateFromMemory(prescannedTemplate, (NSizeType)prescannedTemplateSize, 0, &pSize2, &hGallerySubject);
			if (NFailed(result))
			{
				//printStatusStatement("Error creating a subject for prescanned template:", result);
				throw std::runtime_error("Biometrics: Error creating a subject for prescanned template");
			} 

			//TimedRun([this, buffer2, packedSize2, ppMatchDetails, &score](){NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);}, "Matching: Time elapsed");
			//result = NMVerify(hMatcher, enrolledTemplate, (NSizeType)enrolledTemplateSize, prescannedTemplate, (NSizeType)prescannedTemplateSize, ppMatchDetails, &score);
			result = NBiometricEngineVerifyOffline(hBiometricClient, hProbeSubject, hGallerySubject, &biometricStatus);
			//} catch (std::exception& e) {
			//	throw std::runtime_error("Biometrics Exception");

			//}

			if (NFailed(result))
			{
				//printStatusStatement("Error matching records:", result);
				throw std::runtime_error("Biometrics: Error matching records");
			} else if (biometricStatus == nbsOk) {
				// retrieve matching results from hProbeSubject
				result = NSubjectGetMatchingResult(hProbeSubject, 0, &hMatchingResults);
				if (NFailed(result))
				{
					//printStatusStatement("Error retrieve matching results from emrolled template:", result);
					throw std::runtime_error("Biometrics: Error retrieve matching results from emrolled template");
				}

				// retrieve matching score from matching results
				result = NMatchingResultGetScore(hMatchingResults, &matchScore);
				if (NFailed(result))
				{
					//printStatusStatement("Error retrieve matching score from matching results:", result);
					throw std::runtime_error("Biometrics: Error retrieve matching score from matching results");
				}

				result = NObjectSet(NULL, (HNObject *)&hMatchingResults);
				if (NFailed(result))
				{
					//printStatusStatement("Error clearing resources");
					throw std::runtime_error("Biometrics: Error clearing resources");
				}
			}

			result = NObjectSet(NULL, (HNObject *)&hGallerySubject);
			if (NFailed(result))
			{
				//printStatusStatement("Error clearing resources");
				throw std::runtime_error("Biometrics: Error clearing resources");
			}

			if (matchScore > 0 && biometricStatus == nbsOk)
			{
				return true;
				//matchingStatus = true;
				//printStatusStatement("templates match");
				//sprintf_s (statusStatement, "templates match! score: %d", score);
			}
			else
			{
				return false;
				//printStatusStatement("templates do not match");
				//sprintf_s (statusStatement, "templates do not match. score: %d", score);
			}
		}
	}
}